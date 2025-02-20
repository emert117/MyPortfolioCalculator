using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPortfolioCalculator.Models;

public class Quote
{
    public string ISIN { get; set; }
    public DateTime Date { get; set; }
    public decimal PricePerShare { get; set; }
}
