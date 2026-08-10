using RightRule.Data;
using RightRule.Models;
using RightRule.RuleModels;
using RightRule.StatementProcessor.Evaluator;
using RightRule.System.Common;

namespace RightRule.StatementProcessor.Interpreter;

internal sealed class Interpreter
{
    private readonly RegisteredDatasets? _registeredDatasets;
    private readonly Dictionary<string, Operator> _operators;
    private readonly Evaluator.Evaluator _evaluator;

    internal Interpreter(RegisteredDatasets? datasets, Dictionary<string, Operator> operators, RightRuleConfiguration rightRuleConfig)
    {
        _registeredDatasets = datasets ?? new RegisteredDatasets();
        _operators = operators ?? new Dictionary<string, Operator> { };
        _evaluator = new Evaluator.Evaluator(_operators, rightRuleConfig);
    }

    internal async Task<List<AppliedRule>> ProcessRuleAsync(RegisteredRules rules)
    {
        return await EvaluateAsync(rules).ConfigureAwait(false);
    }

    private async Task<List<AppliedRule>> EvaluateAsync(RegisteredRules rules)
    {
        return await ProcessFirstRule.EvaluateAsync(_evaluator, rules.Rules, _registeredDatasets).ConfigureAwait(false);
    }

    internal async Task<List<AppliedRule>> ProcessRulesAsync(RegisteredRules rules)
    {
        return await EvaluateAsync(rules.Rules, RulesToReturn.All).ConfigureAwait(false);
    }

    internal async Task<List<AppliedRule>> ProcessRulesAsync(RegisteredRules rules, RulesToReturn rulesToReturn)
    {
        return await EvaluateAsync(rules.Rules, rulesToReturn).ConfigureAwait(false);
    }

    private async Task<List<AppliedRule>> EvaluateAsync(List<ParsedRule> rules, RulesToReturn rulesToReturn)
    {
        return rulesToReturn switch
        {
            RulesToReturn.First => await ProcessFirstRule.EvaluateAsync(_evaluator, rules, _registeredDatasets).ConfigureAwait(false),
            RulesToReturn.All   => await ProcessAllRules.EvaluateAsync(_evaluator, rules, _registeredDatasets).ConfigureAwait(false),
            _                   => [AppliedRule.ErrorResult($"Unknown RulesToReturn value: {rulesToReturn}")]
        };
    }
}
