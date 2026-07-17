using RightRule.Data;
using RightRule.Models;
using RightRule.Rules;
using RightRule.StatementProcessor.Evaluator;
using RightRule.System.Common;

namespace RightRule.StatementProcessor.Interpreter;

public sealed class Interpreter
{
    private readonly RegisteredDatasets _registeredDatasets;
    private readonly Dictionary<string, IOperator> _operators;
    private readonly Evaluator.Evaluator _evaluator;

    public Interpreter(RegisteredDatasets datasets, Dictionary<string, IOperator> operators, RightRuleConfiguration rightRuleConfig)
    {
        _registeredDatasets = datasets ?? new RegisteredDatasets();
        _operators = operators ?? new Dictionary<string, IOperator> { };
        _evaluator = new Evaluator.Evaluator(_operators, rightRuleConfig);
    }

    internal List<AppliedRule> ProcessRule(ParsedRules rules)
    {
        return Evaluate(rules);
    }

    private List<AppliedRule> Evaluate(ParsedRules rules)
    {
        return ProcessFirstRule.Evaluate(_evaluator, rules.Rules, _registeredDatasets);
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
        return rulesToReturn switch
        {
            RulesToReturn.First => ProcessFirstRule.Evaluate(_evaluator, rules, _registeredDatasets),
            RulesToReturn.All   => ProcessAllRules.Evaluate(_evaluator, rules, _registeredDatasets),
            _                   => [AppliedRule.DefaultRule($"Unknown RulesToReturn value: {rulesToReturn}")]
        };
    }
}
