using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPortfolioCalculator.Models;

public class Investment
{
    public string InvestorId { get; set; }
    public string InvestmentId { get; set; }
    public string InvestmentType { get; set; }
    public string? ISIN {  get; set; }
    public string? City { get; set; }
    public string? FondsInvestor { get; set; }
}
