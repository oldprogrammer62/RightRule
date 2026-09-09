using RightRule.LanguageProcessor.Evaluator;
using RightRule.Models.BaseModels;
using RightRule.Models.RuleModels;
using RightRule.RegistrationServices.DataRegistration;
using RightRule.RegistrationServices.DefaultRuleRegistration;
using RightRule.RegistrationServices.Rules;
using RightRule.RuleApplication;
using RightRule.System.Common;

namespace RightRule.LanguageProcessor.Interpreter;

public sealed class Interpreter : IInterpreter
{
    private readonly IRegisteredDatasets? _registeredDatasets;
    private readonly IRegisteredDataObjects? _registeredDataObjects;
    private readonly IDefaultRuleRegister _defaultRuleRegister;

    private readonly Dictionary<string, Operator> _operators;
    private readonly IEvaluator _evaluator;

    public Interpreter(IEvaluator evaluator, IRegisteredDatasets? Datasets, IRegisteredDataObjects? DataObjects, Dictionary<string, Operator> operators, IDefaultRuleRegister defaultRuleRegister)
    {
        _registeredDatasets = Datasets ?? new RegisteredDatasets();
        _registeredDataObjects = DataObjects ?? new RegisteredDataObjects();
        _operators = operators ?? new Dictionary<string, Operator> { };
        _defaultRuleRegister = defaultRuleRegister;

        _evaluator = evaluator;
    }

    public async Task<IProcessedRule> EvaluateFirstAsync(IRegisteredRules rules, string context)
    {
        return await EvaluateFirstMatchAsync(rules, context).ConfigureAwait(false);
    }

    public async Task<List<IProcessedRule>> EvaluateAllAsync(IRegisteredRules rules, string Context, RulesToReturn ReturnRules)
    {
        return await EvaluateAsync(rules.Rules, Context, ReturnRules).ConfigureAwait(false);
    }

    private async Task<IProcessedRule> EvaluateFirstMatchAsync(IRegisteredRules rules, string context)
    {
        return await EvaluateFirstMatch.EvaluateAsync(_evaluator, context, rules.Rules, _registeredDatasets, _registeredDataObjects, _defaultRuleRegister).ConfigureAwait(false);
    }

    private async Task<List<IProcessedRule>> EvaluateAsync(List<ParsedRule> rules, string Context, RulesToReturn ReturnRules)
    {
        return ReturnRules switch
        {
            RulesToReturn.First => new List<IProcessedRule> { await EvaluateFirstMatch.EvaluateAsync(_evaluator, Context, rules, _registeredDatasets, _registeredDataObjects, _defaultRuleRegister).ConfigureAwait(false) },
            RulesToReturn.All => await EvaluateAllRules.EvaluateAsync(_evaluator, Context, rules, _registeredDatasets, _registeredDataObjects, _defaultRuleRegister).ConfigureAwait(false),
            _ => [ErrorRule.ErrorResult($"Unknown RulesToReturn value: {ReturnRules}")]
        };
    }
}
