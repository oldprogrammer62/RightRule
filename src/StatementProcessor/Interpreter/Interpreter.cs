using RightRule.Data;
using RightRule.RuleModels;
using RightRule.RuleRegistration;
using RightRule.StatementProcessor.Evaluator;
using RightRule.System.Common;

namespace RightRule.StatementProcessor.Interpreter;

public sealed class Interpreter : IInterpreter
{
    private readonly IRegisteredDatasets? _registeredDatasets;
    private readonly IRegisteredDataObjects? _registeredDataObjects;

    private readonly Dictionary<string, Operator> _operators;
    private readonly IEvaluator _evaluator;

    public Interpreter(IEvaluator evaluator, IRegisteredDatasets? Datasets, IRegisteredDataObjects? DataObjects, Dictionary<string, Operator> operators)
    {
        _registeredDatasets = Datasets ?? new RegisteredDatasets();
        _registeredDataObjects = DataObjects ?? new RegisteredDataObjects();
        _operators = operators ?? new Dictionary<string, Operator> { };

        _evaluator = evaluator;
    }

    public async Task<List<AppliedRule>> ProcessRuleAsync(IRegisteredRules rules)
    {
        return await EvaluateAsync(rules).ConfigureAwait(false);
    }

    public async Task<List<AppliedRule>> ProcessRulesAsync(IRegisteredRules rules, RulesToReturn ReturnRules)
    {
        return await EvaluateAsync(rules.Rules, ReturnRules).ConfigureAwait(false);
    }

    private async Task<List<AppliedRule>> EvaluateAsync(IRegisteredRules rules)
    {
        return await ProcessFirstRule.EvaluateAsync(_evaluator, rules.Rules, _registeredDatasets, _registeredDataObjects).ConfigureAwait(false);
    }

    private async Task<List<AppliedRule>> EvaluateAsync(List<ParsedRule> rules, RulesToReturn ReturnRules)
    {
        return ReturnRules switch
        {
            RulesToReturn.First => await ProcessFirstRule.EvaluateAsync(_evaluator, rules, _registeredDatasets, _registeredDataObjects).ConfigureAwait(false),
            RulesToReturn.All => await ProcessAllRules.EvaluateAsync(_evaluator, rules, _registeredDatasets, _registeredDataObjects).ConfigureAwait(false),
            _ => [AppliedRule.ErrorResult($"Unknown RulesToReturn value: {ReturnRules}")]
        };
    }
}
