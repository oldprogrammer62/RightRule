using RightRule.Data;
using RightRule.LanguageProcessor.Evaluator;
using RightRule.Models.BaseModels;
using RightRule.Models.RuleModels;
using RightRule.OperatorServices;
using RightRule.RegistrationServices.DefaultRuleRegistration;
using RightRule.RegistrationServices.RuleRegistration;
using RightRule.RuleApplication;

namespace RightRule.LanguageProcessor.Interpreter;

public sealed class Interpreter : IInterpreter
{
    private readonly IDefaultRuleRegister _defaultRuleRegister;
    private readonly IDataService _dataService;
    private readonly IOperatorService _operatorService;
    private readonly IEvaluator _evaluator;

    public Interpreter(IEvaluator evaluator, IOperatorService operatorService, IDefaultRuleRegister defaultRuleRegister, IDataService DataService)
    {
        _operatorService = operatorService;
        _defaultRuleRegister = defaultRuleRegister;
        _dataService = DataService;
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
        return await EvaluateFirstMatch.EvaluateAsync(_evaluator, context, rules.Rules, _defaultRuleRegister).ConfigureAwait(false);
    }

    private async Task<List<IProcessedRule>> EvaluateAsync(List<ParsedRule> rules, string Context, RulesToReturn ReturnRules)
    {
        return ReturnRules switch
        {
            RulesToReturn.FIRST => new List<IProcessedRule> { await EvaluateFirstMatch.EvaluateAsync(_evaluator, Context, rules, _defaultRuleRegister).ConfigureAwait(false) },
            RulesToReturn.ALL => await EvaluateAllRules.EvaluateAsync(_evaluator, Context, rules, _defaultRuleRegister, _dataService).ConfigureAwait(false),
            _ => [ErrorRule.ErrorResult($"Unknown RulesToReturn value: {ReturnRules}")]
        };
    }
}
