namespace RightRule_Demo.DataManager;

internal class RTRManager
{
    internal static string GetRuleResultType()
    {
        Console.WriteLine("Would you like to see a single rule result or results of all rules?");
        Console.WriteLine("Enter 'S' for a single rule result or 'A' for results of all rules:");
        var input = Console.ReadLine();
        if (string.IsNullOrEmpty(input))
        {
            Console.WriteLine("Input cannot be empty. Please enter 'S' or 'A'.");
            return GetRuleResultType();
        }
        else if (input.ToLower() == "s" || input.ToLower() == "a")
        {
            return input.ToLower();
        }
        else
        {
            Console.WriteLine("Invalid input. Please enter 'S' or 'A'.");
            return GetRuleResultType();
        }
    }
}
