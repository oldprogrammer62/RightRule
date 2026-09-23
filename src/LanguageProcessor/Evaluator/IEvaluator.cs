using RightRule.RuleServices.RuleModels;

namespace RightRule.LanguageProcessor.Evaluator;

public interface IEvaluator
{
    Task<EvaluationResult> EvaluateAsync(ParsedRule Rule);
}
