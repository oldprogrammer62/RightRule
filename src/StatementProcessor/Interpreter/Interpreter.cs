using RightRule.Data;
using RightRule.Rules;
using RightRule.StatementProcessor.Evaluator;
using RightRule.System.Common;

namespace RightRule.StatementProcessor.Interpreter;

public sealed class Interpreter
{
    private readonly RegisteredDatasets _registeredDatasets;
    private readonly Dictionary<string, IOperator> _operators;
    private readonly Evaluator.Evaluator _evaluator;

    public Interpreter(RegisteredDatasets datasets, Dictionary<string, IOperator> operators, bool SilentErrors = false)
    {
        _registeredDatasets = datasets ?? new RegisteredDatasets();
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
}
