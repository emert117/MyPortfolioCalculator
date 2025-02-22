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
    public async Task Investor_Got_Only_Stock_Got_Transaction_and_Quoto_at_Exact_Date()
    {
        // Arrange


        // Act
        decimal total = await PortfolioCalculatorService.GetPortfolioValueAsync(new DateTime(2016, 01, 05), "Investor0");

        // Assert
        Assert.AreEqual(228038.5914M, total, 0.001M, "Wrong total");
    }

    [TestMethod]
    public async Task Investor_Got_Only_Stock_Got_No_Exact_Date_Transaction_and_No_Exact_Date_Quote()
    {
        // Arrange

        // Act
        decimal total = await PortfolioCalculatorService.GetPortfolioValueAsync(new DateTime(2016, 01, 08), "Investor0");

        // Assert
        Assert.AreEqual(228038.5914M, total, 0.001M, "Wrong total");
    }

    [TestMethod]
    public async Task Calculate_Simple_RealEstate_Investment()
    {
        decimal total = await PortfolioCalculatorService.GetPortfolioValueAsync(new DateTime(2019, 07, 08), "Investor1");
        Assert.AreEqual(1170894.0M, total, 0.001M, "Wrong total");
    }

    [TestMethod]
    public async Task Calculate_Fonds_Investment()
    {
        // Arrange


        // Act
        decimal total = await PortfolioCalculatorService.GetPortfolioValueAsync(new DateTime(2019, 08, 05), "Fonds0");

        // Assert
        Assert.AreEqual(2404497.270M, total, 0.001M, "Wrong total");
    }

    [TestMethod]
    public async Task Calculate_Investor_Fond_Investment_Who_Has_Only_One_Fond_Investment()
    {
        // Arrange


        // Act
        decimal total = await PortfolioCalculatorService.GetPortfolioValueAsync(new DateTime(2019, 08, 05), "Investor2");

        // Assert
        Assert.AreEqual(50518.487642700M, total, 0.001M, "Wrong total");
    }
}
