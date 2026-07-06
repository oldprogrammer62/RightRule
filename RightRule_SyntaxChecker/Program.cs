using RightRule.Extensions;
using RightRule.SyntaxChecker;

namespace RightRule_SyntaxChecker;

internal class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Welcome to the RightRule Syntax Checker!");
        Console.WriteLine("This tool allows you to check the syntax of RightRule statements.");
        Console.WriteLine();

        var rr = new RightRule.RightRule();
        DoCheckProcess(rr);

        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();
    }

    private static void DoCheckProcess(RightRule.RightRule rightRule)
    {
        var statement = DataManager.StatementManager.GetRuleStatement();
        var result = rightRule.CheckStatement(statement);
        ShowResult(result);

        Console.WriteLine("Check another statement? (y/n)");
        var input = Console.ReadLine();
        if (input?.ToLower() == "y")
        {
            DoCheckProcess(rightRule); // Restart the process
        }
    }

    private static void ShowResult(CheckStatementResult result)
    {
        if (result.IsValid)
        {
            Console.WriteLine("The statement is valid.");
            Console.WriteLine();
            return;
        }

        Console.WriteLine();
        Console.WriteLine("The statement is invalid.");
        Console.WriteLine("Errors:");
        foreach (var error in result.Diagnostics)
        {
            Console.WriteLine($"- {error}");
        }
        Console.WriteLine();
    }
}
