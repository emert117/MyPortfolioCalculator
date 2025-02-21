using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyPortfolioCalculator.Helpers;
using MyPortfolioCalculator.Models;

namespace MyPortfolioCalculator.Business;

public static class PortfolioCalculatorService
{

    private static readonly ReadOnlyCollection<Investment> allInvestments;
    private static readonly ReadOnlyCollection<Transaction> allTransactions;
    private static readonly ReadOnlyCollection<Quote> allQuotes;

    static PortfolioCalculatorService()
    {
        allInvestments = CSVHelper.GetListOfType<Investment>("Investments.csv");
        allTransactions = CSVHelper.GetListOfType<Transaction>("Transactions.csv");
        allQuotes = CSVHelper.GetListOfType<Quote>("Quotes.csv");
    }

    public static decimal GetPortfolioValue(DateTime date, string investorId)
    {
        // get investments of the investor
        ReadOnlyCollection<Investment> investmentsOfInvestor = allInvestments.Where(i => i.InvestorId == investorId).ToList().AsReadOnly();
        
        // calculate share value
        decimal totalStockPortfolio = CalculateStockPortfolio(date, investmentsOfInvestor);
        
        // calculate realestate value
        decimal totalRealEstatePortfolio = CalculateRealEstatePortfolio(date, investmentsOfInvestor);

        // get funds and percentage of funds of the investor
        decimal totalFondsPortfolio = CalculateFondsPortfolio(date, investmentsOfInvestor);

        decimal totalPortfolio = totalStockPortfolio + totalRealEstatePortfolio + totalFondsPortfolio;
        return totalPortfolio;
    }

    private static decimal CalculateFondsPortfolio(DateTime date, ReadOnlyCollection<Investment> investmentsOfInvestor)
    {
        List<Investment> fondsInvestment = investmentsOfInvestor.Where(i => i.InvestmentType == "Fonds").ToList();
        decimal totalFondsPortfolio = 0;

        foreach (var investment in fondsInvestment) 
        {
            // get transactions
            var transactions = allTransactions.Where(t => t.InvestmentId == investment.InvestmentId
                && t.Date <= date).ToList();

            decimal totalPercentage = 0;
            foreach (var transaction in transactions) 
            {
                totalPercentage += transaction.Value;
            }

            totalFondsPortfolio = totalPercentage * GetPortfolioValue(date, investment.FondsInvestor);
        }

        return totalFondsPortfolio;
    }

    private static decimal CalculateRealEstatePortfolio(DateTime date, ReadOnlyCollection<Investment> investments)
    {
        var realEstateInvestments = investments.Where(i => i.InvestmentType == "RealEstate").ToList().AsReadOnly();
        decimal totalRealEstatePortfolio = 0;

        foreach (var investment in realEstateInvestments)
        {
            var transactions = allTransactions.Where(t => t.InvestmentId == investment.InvestmentId
                && t.Date <= date).ToList();

            decimal totalOfRealEstate = 0;
            foreach (var transaction in transactions) 
            {
                totalOfRealEstate += transaction.Value;
            }
            totalRealEstatePortfolio += totalOfRealEstate;
        }

        return totalRealEstatePortfolio;
    }

    private static decimal CalculateStockPortfolio(DateTime date, ReadOnlyCollection<Investment> investments)
    {
        var stockInvestments = investments.Where(i => i.InvestmentType == "Stock").ToList().AsReadOnly();
        decimal totalStockPortfolio = 0;

        foreach (var investment in stockInvestments)
        {
            var transactions = allTransactions.Where(t => t.InvestmentId == investment.InvestmentId && t.Date <= date)
                .ToList();

            decimal totalOfShare = 0;
            foreach (var transaction in transactions)
            {
                totalOfShare += transaction.Value;
            }

            //  get the value of the stock at the specific date, if not found get the previous day's value
            List<Quote> valuesOfStock = allQuotes.Where(q => q.ISIN == investment.ISIN && q.Date <= date)
                .ToList();


            decimal? currentValueOfStock = valuesOfStock.FirstOrDefault(v => v.Date == date)?.PricePerShare;
            if (currentValueOfStock == null)
            {
                // there is no Stock value for the specified day
                // get the closest day's value
                var closestValue = valuesOfStock.Where(v => v.Date < date).Max(v => v.Date);
                currentValueOfStock = valuesOfStock.FirstOrDefault(v => v.Date == closestValue)?.PricePerShare;
            }

            totalStockPortfolio += totalOfShare * currentValueOfStock.Value;
        }
        return totalStockPortfolio;
    }

}
