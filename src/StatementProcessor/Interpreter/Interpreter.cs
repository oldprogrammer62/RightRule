using RightRule.Data;
using RightRule.Models;
using RightRule.RuleModels;
using RightRule.RuleRegistration;
using RightRule.StatementProcessor.Evaluator;
using RightRule.System.Common;

namespace RightRule.StatementProcessor.Interpreter;

internal sealed class Interpreter
{
    private readonly IRegisteredDatasets? _registeredDatasets;
    private readonly IRegisteredDataObjects? _registeredDataObjects;

    private readonly Dictionary<string, Operator> _operators;
    private readonly Evaluator.Evaluator _evaluator;

    internal Interpreter(IRegisteredDatasets? Datasets, IRegisteredDataObjects? DataObjects, Dictionary<string, Operator> operators, RightRuleConfiguration rightRuleConfig)
    {
        _registeredDatasets = Datasets ?? new RegisteredDatasets();
        _registeredDataObjects = DataObjects ?? new RegisteredDataObjects();
        _operators = operators ?? new Dictionary<string, Operator> { };
        _evaluator = new Evaluator.Evaluator(_operators, rightRuleConfig);
    }

    internal async Task<List<AppliedRule>> ProcessRuleAsync(IRegisteredRules rules)
    {
        return await EvaluateAsync(rules).ConfigureAwait(false);
    }

    private async Task<List<AppliedRule>> EvaluateAsync(IRegisteredRules rules)
    {
        return await ProcessFirstRule.EvaluateAsync(_evaluator, rules.Rules, _registeredDatasets, _registeredDataObjects).ConfigureAwait(false);
    }

    internal async Task<List<AppliedRule>> ProcessRulesAsync(IRegisteredRules rules)
    {
        return await EvaluateAsync(rules.Rules, RulesToReturn.All).ConfigureAwait(false);
    }

    internal async Task<List<AppliedRule>> ProcessRulesAsync(IRegisteredRules rules, RulesToReturn ReturnRules)
    {
        return await EvaluateAsync(rules.Rules, ReturnRules).ConfigureAwait(false);
    }

    private async Task<List<AppliedRule>> EvaluateAsync(List<ParsedRule> rules, RulesToReturn ReturnRules)
    {
        return ReturnRules switch
        {
            RulesToReturn.First => await ProcessFirstRule.EvaluateAsync(_evaluator, rules, _registeredDatasets, _registeredDataObjects).ConfigureAwait(false),
            RulesToReturn.All   => await ProcessAllRules.EvaluateAsync(_evaluator, rules, _registeredDatasets, _registeredDataObjects).ConfigureAwait(false),
            _                   => [AppliedRule.ErrorResult($"Unknown RulesToReturn value: {ReturnRules}")]
        };
    }
}
