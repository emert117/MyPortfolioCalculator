using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPortfolioCalculator.Models;

public class Transaction
{
    public string InvestmentId { get; set; }
    public string Type { get; set; }
    public DateTime Date {  get; set; }
    public decimal Value { get; set; }
}
