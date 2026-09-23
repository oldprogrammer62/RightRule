using RightRule.LanguageProcessor.Evaluator;
using RightRule.RuleServices.DefaultRuleRegistration;
using RightRule.RuleServices.RuleModels.BaseModels;
using RightRule.RuleServices.Rules;

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

    public async Task<IProcessedRule> EvaluateFirstAsync(IRegisteredRules Rules, string Context)
    {
        return await EvaluateFirstMatch.EvaluateAsync(_evaluator, Context, Rules.Rules, _defaultRuleRegister).ConfigureAwait(false);
    }

    public async Task<List<IProcessedRule>> EvaluateAnyAsync(IRegisteredRules Rules, string Context, EvaluationTypeEnum EvaluationType)
    {
        return await EvaluateAnyMatch.EvaluateAsync(_evaluator, Context, Rules.Rules, _defaultRuleRegister).ConfigureAwait(false);
    }

    public async Task<List<IProcessedRule>> EvaluateAllAsync(IRegisteredRules Rules, string Context, EvaluationTypeEnum EvaluationType)
    {
        return await EvaluateAllMatches.EvaluateAsync(_evaluator, Context, Rules.Rules, _defaultRuleRegister).ConfigureAwait(false);
    }
}
