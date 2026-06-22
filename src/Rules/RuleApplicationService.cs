using RightRule.Data;
using RightRule.HelperFunctions;
using RightRule.StatementProcessor.Interpreter;
using RightRule.System.Common;

namespace RightRule.Rules;

internal class RuleApplicationService
{
    internal static AppliedRule ApplyRule(string Context, RegisteredDatasets Data, ParsedRules rules, Dictionary<string, IOperator> Operators)
    {
        return ApplyRuleToData(Context, Data, rules, Operators);
    }

    private static AppliedRule ApplyRuleToData(string Context, RegisteredDatasets Data, ParsedRules rules, Dictionary<string, IOperator> Operators)
    {
        var interpreter = new Interpreter(Data, Operators);
        var filteredRules = Helpers.FilterRules(Context, rules.RightRules);

        return interpreter.ProcessRule(filteredRules);
    }

    internal static AppliedRule ApplyRule(string Context, RegisteredDatasets Data, ParsedRules Rules, Dictionary<string, IOperator> Operators, string DatasetName)
    {
        return ApplyRuleToData(Context, Data, Rules, Operators, DatasetName);
    }

    private static AppliedRule ApplyRuleToData(string Context, RegisteredDatasets Data, ParsedRules Rules, Dictionary<string, IOperator> Operators, string DatasetName)
    {
        var interpreter = new Interpreter(Data, Operators);
        var filteredRules = Helpers.FilterRules(Context, Rules.RightRules);

        return interpreter.ProcessRule(filteredRules, DatasetName);
    }

    internal static List<AppliedRule> ApplyRules(string Context, RegisteredDatasets Data, ParsedRules rules, Dictionary<string, IOperator> Operators)
    {
        return ApplyRulesToData(Context, Data, rules, Operators);
    }

    private static List<AppliedRule> ApplyRulesToData(string Context, RegisteredDatasets Data, ParsedRules rules, Dictionary<string, IOperator> Operators)
    {
        var interpreter = new Interpreter(Data, Operators);
        var filteredRules = Helpers.FilterRules(Context, rules.RightRules);

        return interpreter.ProcessRules(filteredRules);
    }

    internal static List<AppliedRule> ApplyRules(string Context, RegisteredDatasets Data, ParsedRules rules, RulesToReturn rulesToReturn, Dictionary<string, IOperator> Operators)
    {
        return ApplyRulesToData(Context, Data, rules, rulesToReturn, Operators);
    }

    private static List<AppliedRule> ApplyRulesToData(string Context, RegisteredDatasets Data, ParsedRules rules, RulesToReturn rulesToReturn, Dictionary<string, IOperator> Operators)
    {
        var interpreter = new Interpreter(Data, Operators);
        var filteredRules = Helpers.FilterRules(Context, rules.RightRules);

        return interpreter.ProcessRules(filteredRules, rulesToReturn);
    }

    internal static List<AppliedRule> ApplyRules(string Context, RegisteredDatasets Data, ParsedRules rules, Dictionary<string, IOperator> Operators, string DatasetName)
    {
        return ApplyRulesToData(Context, Data, rules, Operators, DatasetName);
    }

    private static List<AppliedRule> ApplyRulesToData(string Context, RegisteredDatasets Data, ParsedRules rules, Dictionary<string, IOperator> Operators, string DatasetName)
    {
        var interpreter = new Interpreter(Data, Operators);
        var filteredRules = Helpers.FilterRules(Context, rules.RightRules);

        return interpreter.ProcessRules(filteredRules, DatasetName);
    }

    internal static List<AppliedRule> ApplyRules(string Context, RegisteredDatasets Data, ParsedRules rules, RulesToReturn rulesToReturn, Dictionary<string, IOperator> Operators, string DatasetName)
    {
        return ApplyRulesToData(Context, Data, rules, rulesToReturn, Operators, DatasetName);
    }

    private static List<AppliedRule> ApplyRulesToData(string Context, RegisteredDatasets Data, ParsedRules rules, RulesToReturn rulesToReturn, Dictionary<string, IOperator> Operators, string DatasetName)
    {
        var interpreter = new Interpreter(Data, Operators);
        var filteredRules = Helpers.FilterRules(Context, rules.RightRules);

        return interpreter.ProcessRules(filteredRules, DatasetName, rulesToReturn);
    }
}