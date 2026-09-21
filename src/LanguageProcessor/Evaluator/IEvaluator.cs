using RightRule.Models.RuleModels;

namespace RightRule.LanguageProcessor.Evaluator;

public interface IEvaluator
{
    Task<EvaluationResult> EvaluateAsync(ParsedRule Rule);
}
