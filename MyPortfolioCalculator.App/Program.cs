using MyPortfolioCalculator.Business;

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
            var input = line.Split(';');
            var date = DateTime.Parse(input[0]);
            var investorId = input[1];

            Console.WriteLine("Calculating...");
            decimal totalPortfolio = await PortfolioCalculatorService.GetPortfolioValueAsync(date, investorId);
            Console.WriteLine($"Total portfolio: {totalPortfolio}");

            Console.WriteLine("---------------");
            Console.Write("Please enter date and investor id (to exit the program just press Enter): ");
            line = Console.ReadLine();
        }

    }
}
