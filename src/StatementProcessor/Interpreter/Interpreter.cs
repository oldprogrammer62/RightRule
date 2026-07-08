using RightRule.Data;
using RightRule.Rules;
using RightRule.System.Common;

namespace RightRule.StatementProcessor.Interpreter;

public sealed class Interpreter
{
    private readonly RegisteredDatasets _registeredDatasets;
    private readonly Dictionary<string, IOperator> _operators;
    private readonly Evaluator.Evaluator _evaluator;

    public Interpreter(RegisteredDatasets data, Dictionary<string, IOperator> operators, bool SilentErrors = false)
    {
        _registeredDatasets = data ?? new RegisteredDatasets();
        _operators = operators ?? new Dictionary<string, IOperator> { };
        _evaluator = new Evaluator.Evaluator(_operators, SilentErrors);
    }

    internal AppliedRule ProcessRule(ParsedRules rules)
    {
        return Evaluate(rules);
    }

    private AppliedRule Evaluate(ParsedRules rules)
    {
        return ProcessFirstRule.EvaluateFirst(_evaluator, rules.Rules, _registeredDatasets);
    }

    internal AppliedRule ProcessRule(ParsedRules rules, string datasetName)
    {
        return Evaluate(rules, datasetName);
    }

    private AppliedRule Evaluate(ParsedRules rules, string datasetName)
    {
        return ProcessFirstRule.EvaluateFirst(_evaluator, rules.Rules, _registeredDatasets, datasetName);
    }

    internal List<AppliedRule> ProcessRules(ParsedRules rules)
    {
        return Evaluate(rules.Rules, RulesToReturn.All);
    }

    internal List<AppliedRule> ProcessRules(ParsedRules rules, RulesToReturn rulesToReturn)
    {
        return Evaluate(rules.Rules, rulesToReturn);
    }

    private List<AppliedRule> Evaluate(List<ParsedRule> rules, RulesToReturn rulesToReturn)
    {
        switch (rulesToReturn)
        {
            case RulesToReturn.First:
                return [ProcessFirstRule.EvaluateFirst(_evaluator, rules, _registeredDatasets)];

            case RulesToReturn.All:
                return ProcessAllRules.EvaluateAll(_evaluator, rules, _registeredDatasets);

            default:
                return [AppliedRule.DefaultRule($"Unknown RulesToReturn value: {rulesToReturn}")];
        }
    }

    internal List<AppliedRule> ProcessRules(ParsedRules rules, string datasetName)
    {
        return Evaluate(rules.Rules, datasetName, RulesToReturn.All);
    }

    internal List<AppliedRule> ProcessRules(ParsedRules rules, string datasetName, RulesToReturn rulesToReturn)
    {
        return Evaluate(rules.Rules, datasetName, rulesToReturn);
    }

    private List<AppliedRule> Evaluate(List<ParsedRule> rules, string datasetName, RulesToReturn rulesToReturn)
    {
        switch (rulesToReturn)
        {
            case RulesToReturn.First:
                return [ProcessFirstRule.EvaluateFirst(_evaluator, rules, _registeredDatasets, datasetName)];

            case RulesToReturn.All:
                return ProcessAllRules.EvaluateAll(_evaluator, rules, _registeredDatasets, datasetName);

            default:
                return [AppliedRule.DefaultRule($"Unknown RulesToReturn value: {rulesToReturn}")];
        }
    }
}
