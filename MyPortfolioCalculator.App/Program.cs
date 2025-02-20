using MyPortfolioCalculator.Business;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MyPortfolioCalculator.App;

internal class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Portfolio Calculator");
        Console.Write("Please enter date and investor id: ");

        //Console.WriteLine(PortfolioCalculatorService.GetPortfolioValue(new DateTime(2019,11,17), "Investor0"));

        var line = Console.ReadLine();
        while (!string.IsNullOrWhiteSpace(line))
        {
            var input = line.Split(';');
            var date = DateTime.Parse(input[0]);
            var investorId = input[1];

            /* your code here */
            Console.WriteLine("Calculating...");
            decimal totalPortfolio = PortfolioCalculatorService.GetPortfolioValue(date, investorId);
            Console.WriteLine($"Total portfolio: {totalPortfolio}");

            Console.WriteLine("---------------");
            Console.Write("Please enter date and investor id (to exit the program just press Enter): ");
            line = Console.ReadLine();
        }

    }
}
