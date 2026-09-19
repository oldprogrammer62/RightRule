using RightRule.LanguageProcessor.Evaluator;
using RightRule.Models.BaseModels;
using RightRule.RegistrationServices.DefaultRuleRegistration;
using RightRule.RegistrationServices.RuleRegistration.Rules;

namespace RightRule.LanguageProcessor.Interpreter;

internal sealed class Interpreter : IInterpreter
{
    private readonly IDefaultRuleRegister _defaultRuleRegister;
    private readonly IEvaluator _evaluator;

    public Interpreter(IEvaluator evaluator, IDefaultRuleRegister defaultRuleRegister)
    {
        _defaultRuleRegister = defaultRuleRegister;
        _evaluator = evaluator;
    }

    public async Task<IProcessedRule> EvaluateFirstAsync(IRegisteredRules rules, string Context)
    {
        return await EvaluateFirstMatch.EvaluateAsync(_evaluator, Context, rules.Rules, _defaultRuleRegister).ConfigureAwait(false);
    }

    public async Task<List<IProcessedRule>> EvaluateAnyAsync(IRegisteredRules rules, string Context, EvaluationTypeEnum EvaluationType)
    {
        return await EvaluateAnyMatch.EvaluateAsync(_evaluator, Context, rules.Rules, _defaultRuleRegister).ConfigureAwait(false);
    }

    public async Task<List<IProcessedRule>> EvaluateAllAsync(IRegisteredRules rules, string Context, EvaluationTypeEnum EvaluationType)
    {
        return await EvaluateAllMatches.EvaluateAsync(_evaluator, Context, rules.Rules, _defaultRuleRegister).ConfigureAwait(false);
    }
}
