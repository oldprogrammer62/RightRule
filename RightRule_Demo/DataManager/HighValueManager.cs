namespace RightRule_Demo.DataManager;

internal class HighValueManager
{
    internal static decimal? GetHighValue()
    {
        Console.WriteLine("Please enter a High Value amount...");

        var input = Console.ReadLine();
        if (string.IsNullOrEmpty(input))
        {
            Console.WriteLine("High Value amount cannot be empty.");
            return default;
        }
        Console.WriteLine();
        if (decimal.TryParse(input, out var highValue))
        {
            return highValue;
        }
        else
        {
            Console.WriteLine("Invalid High Value amount.");
            return default;
        }
    }
}
