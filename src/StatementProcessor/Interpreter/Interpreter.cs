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

    internal async Task<List<AppliedRule>> ProcessRule(RegisteredRules rules)
    {
        return await Evaluate(rules);
    }

    private async Task<List<AppliedRule>> Evaluate(RegisteredRules rules)
    {
        return await ProcessFirstRule.Evaluate(_evaluator, rules.Rules, _registeredDatasets);
    }

    internal async Task<List<AppliedRule>> ProcessRules(RegisteredRules rules)
    {
        return await Evaluate(rules.Rules, RulesToReturn.All);
    }

    internal async Task<List<AppliedRule>> ProcessRules(RegisteredRules rules, RulesToReturn rulesToReturn)
    {
        return await Evaluate(rules.Rules, rulesToReturn);
    }

    private async Task<List<AppliedRule>> Evaluate(List<ParsedRule> rules, RulesToReturn rulesToReturn)
    {
        return rulesToReturn switch
        {
            RulesToReturn.First => await ProcessFirstRule.Evaluate(_evaluator, rules, _registeredDatasets),
            RulesToReturn.All   => await ProcessAllRules.Evaluate(_evaluator, rules, _registeredDatasets),
            _                   => [AppliedRule.DefaultRule($"Unknown RulesToReturn value: {rulesToReturn}")]
        };
    }
}
