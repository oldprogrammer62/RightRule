namespace RightRule_SyntaxChecker.DataManager;

internal class StatementManager
{
    internal static string GetRuleStatement()
    {
        Console.WriteLine("Please enter a RightRule statement to check for syntax errors:");
        var statement = Console.ReadLine();
        if (string.IsNullOrEmpty(statement))
        {
            Console.WriteLine("Statement cannot be empty.");
            return string.Empty;
        }
        return statement;
    }
}
