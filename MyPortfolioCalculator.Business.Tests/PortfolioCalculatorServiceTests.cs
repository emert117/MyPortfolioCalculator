using MyPortfolioCalculator.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyPortfolioCalculator.Business;

namespace MyPortfolioCalculator.Business.Tests;

[TestClass]
public sealed class PortfolioCalculatorServiceTests
{
    ReadOnlyCollection<Investment> investments = null;

    [TestInitialize]
    public async Task Initialize()
    {
        
    }

    [TestMethod]
    public void Investor_Got_Only_Stock_Got_Transaction_and_Quoto_at_Exact_Date()
    {
        // Arrange
        

        // Act
        decimal total = PortfolioCalculatorService.GetPortfolioValue(new DateTime(2016,01, 05), "Investor0");

        // Assert
        Assert.AreEqual(228038.5914M, total, 0.001M, "Wrong total");
    }

    [TestMethod]
    public void Investor_Got_Only_Stock_Got_No_Exact_Date_Transaction_and_No_Exact_Date_Quote()
    {
        // Arrange

        // Act
        decimal total = PortfolioCalculatorService.GetPortfolioValue(new DateTime(2016, 01, 08), "Investor0");

        // Assert
        Assert.AreEqual(228038.5914M, total, 0.001M, "Wrong total");
    }
}
