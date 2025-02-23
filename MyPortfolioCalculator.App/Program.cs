using MyPortfolioCalculator.Business;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MyPortfolioCalculator.App;

internal class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("Portfolio Calculator");
        Console.Write("Please enter date and investor id: ");

        var line = Console.ReadLine();
        while (!string.IsNullOrWhiteSpace(line))
        {
            (bool userInputIsValid, DateTime date, string investorId) = ParseUserInput(line);
            if(userInputIsValid)
            {
                Console.WriteLine("Calculating...");
                decimal totalPortfolio = await PortfolioCalculatorService.GetPortfolioValueAsync(date, investorId);
                Console.WriteLine($"Total portfolio: {totalPortfolio}");

                Console.WriteLine("---------------");
            }
            else 
            {
                Console.WriteLine("Please enter a valid input.");
            }
            Console.Write("Please enter date and investor id (to exit the program just press Enter): ");
            line = Console.ReadLine();
        }

    }

    private static (bool userInputIsValid, DateTime date, string investorId) ParseUserInput(string line)
    {
        if (string.IsNullOrWhiteSpace(line))
        {
            return (false, DateTime.MinValue, null);
        }

        var input = line.Split(';');
        if (input.Length != 2)
        {
            return (false, DateTime.MinValue, null);
        }

        if (!DateTime.TryParse(input[0], out DateTime date))
        {
            return (false, DateTime.MinValue, null);
        }

        string investorId = input[1]?.Trim();
        if (string.IsNullOrWhiteSpace(investorId))
        {
            return (false, DateTime.MinValue, null);
        }

        return (true, date, investorId);
    }
}
