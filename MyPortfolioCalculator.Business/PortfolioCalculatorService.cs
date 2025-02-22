using MyPortfolioCalculator.Helpers;
using MyPortfolioCalculator.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPortfolioCalculator.Business;

public static class PortfolioCalculatorService
{
    private static readonly IReadOnlyCollection<Investment> _allInvestments;
    private static readonly IReadOnlyCollection<Transaction> _allTransactions;
    private static readonly IReadOnlyCollection<Quote> _allQuotes;
    private static readonly ParallelOptions _parallelOptions;

    static PortfolioCalculatorService()
    {
        _allInvestments = CSVHelper.GetListOfType<Investment>("Investments.csv");
        _allTransactions = CSVHelper.GetListOfType<Transaction>("Transactions.csv");
        _allQuotes = CSVHelper.GetListOfType<Quote>("Quotes.csv");

        // Configure parallel options - adjust MaxDegreeOfParallelism based on your needs
        _parallelOptions = new ParallelOptions
        {
            MaxDegreeOfParallelism = Environment.ProcessorCount
        };
    }

    public static async Task<decimal> GetPortfolioValueAsync(DateTime date, string investorId)
    {
        var investorInvestments = _allInvestments
            .Where(i => i.InvestorId == investorId)
            .ToList()
            .AsReadOnly();

        // Calculate different portfolio types in parallel
        var tasks = new[]
        {
            Task.Run(() => CalculateStockPortfolio(date, investorInvestments)),
            Task.Run(() => CalculateRealEstatePortfolio(date, investorInvestments)),
            Task.Run(() => CalculateFondsPortfolio(date, investorInvestments))
        };

        var results = await Task.WhenAll(tasks);
        return results.Sum();
    }

    private static decimal CalculateFondsPortfolio(DateTime date, IReadOnlyCollection<Investment> investments)
    {
        var fondsInvestments = investments.Where(i => i.InvestmentType == "Fonds").ToList();

        // Use ConcurrentDictionary to safely store results from parallel operations
        var results = new ConcurrentDictionary<string, decimal>();

        Parallel.ForEach(fondsInvestments, _parallelOptions, investment =>
        {
            var totalPercentage = GetTransactionsTotal(investment.InvestmentId, date);
            var portfolioValue = CalculateStockPortfolio(date, fondsInvestments);
            results.TryAdd(investment.InvestmentId, totalPercentage * portfolioValue);
        });

        return results.Values.Sum();
    }

    private static decimal CalculateRealEstatePortfolio(DateTime date, IReadOnlyCollection<Investment> investments)
    {
        var realEstateInvestments = investments.Where(i => i.InvestmentType == "RealEstate").ToList();

        return realEstateInvestments
            .AsParallel()
            .WithDegreeOfParallelism(Environment.ProcessorCount)
            .Select(investment => GetTransactionsTotal(investment.InvestmentId, date))
            .Sum();
    }

    private static decimal CalculateStockPortfolio(DateTime date, IReadOnlyCollection<Investment> investments)
    {
        var stockInvestments = investments.Where(i => i.InvestmentType == "Stock").ToList();
        var results = new ConcurrentDictionary<string, decimal>();

        Parallel.ForEach(stockInvestments, _parallelOptions, investment =>
        {
            var shareTotal = GetTransactionsTotal(investment.InvestmentId, date);
            var stockPrice = GetStockPrice(investment.ISIN, date);
            results.TryAdd(investment.InvestmentId, shareTotal * stockPrice);
        });

        return results.Values.Sum();
    }

    private static decimal GetTransactionsTotal(string investmentId, DateTime date)
    {
        // Using parallel processing for large transaction sets
        return _allTransactions
            .AsParallel()
            .Where(t => t.InvestmentId == investmentId && t.Date <= date)
            .Sum(t => t.Value);
    }

    private static decimal GetStockPrice(string isin, DateTime date)
    {
        var relevantQuotes = _allQuotes
            .Where(q => q.ISIN == isin && q.Date <= date)
            .ToList();

        var currentDayQuote = relevantQuotes
            .FirstOrDefault(v => v.Date == date)?
            .PricePerShare;

        if (currentDayQuote.HasValue)
            return currentDayQuote.Value;

        var mostRecentQuote = relevantQuotes
            .OrderByDescending(v => v.Date)
            .FirstOrDefault()?
            .PricePerShare;

        if (!mostRecentQuote.HasValue)
            throw new InvalidOperationException($"No stock price found for ISIN {isin} on or before {date}");

        return mostRecentQuote.Value;
    }
    
}