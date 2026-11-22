using RightRule.LanguageProcessor.SyntaxChecker;
using RightRule.Models;
using RightRule.RightRuleSession;
using RightRule.RuleServices.RuleModels;

namespace RightRule_SyntaxChecker;

internal class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Welcome to the RightRule Syntax Checker!");
        Console.WriteLine("This tool allows you to check the syntax of RightRule statements.");
        Console.WriteLine();

        RightRuleOptions options = new RightRuleOptions()
        {
            DataCheck = false,
            Diagnostics = true,
            LoadDefaultOperators = true,
            LoadCustomOperators = true
        };
        var rr = new RightRule.RightRule(options).CreateSession();

        DoCheckProcessAsync(rr).GetAwaiter().GetResult();

        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();
    }

    private async static Task DoCheckProcessAsync(IRightRuleSession rightRule)
    {
        var statement = DataManager.StatementManager.GetRuleStatement();
        var result = await rightRule.StatementSyntaxCheckAsync(statement).ConfigureAwait(false);

        if (result == null)
        {
            Console.WriteLine("An error occurred while checking the statement.");
            return;
        }
        if (!result.IsValid)
        {
            ShowInvalidResult(result);
        }

        if (result.IsValid)
        {
            DefinedRule rule = new DefinedRule()
            {
                Number = 1,
                Name = "Rule being Checked",
                Statement = statement,
                Context = "Default Context",
                TrueDecision = "Default Decision",
            };
            _ = rightRule.RuleService.RegisterRule(rule);

            ShowResult(statement, result, rightRule.RuleService.GetRegisteredRules());
        }

        Console.WriteLine("Check another statement? (y/n)");
        var input = Console.ReadLine();
        if (input?.ToLower() == "n")
        {
            return;
        }
        rightRule.RuleService.DeRegisterRules(); // Clear registered rules before checking another statement
        await DoCheckProcessAsync(rightRule).ConfigureAwait(false); // Restart the process
    }

    private static void ShowResult(string statement, CheckStatementResult result, List<ParsedRule> parsedRules)
    {
        Console.WriteLine("The statement is valid.");
        Console.WriteLine();
        Console.WriteLine($"Statement \"{statement}\"");
        Console.WriteLine("Evaluated as follows:");

        foreach (var rule in parsedRules)
        {
            Console.WriteLine(rule.StatementPath);
            Console.WriteLine();
        }
    }

    private static void ShowInvalidResult(CheckStatementResult result)
    {
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
