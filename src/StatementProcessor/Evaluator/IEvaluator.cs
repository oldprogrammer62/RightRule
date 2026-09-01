using RightRule.DataRegistration;
using RightRule.Models.RuleEvaluation;
using RightRule.RuleModels;

namespace RightRule.StatementProcessor.Evaluator;

public interface IEvaluator
{
    Task<RuleEvaluationResult> EvaluateAsync(ParsedRule ParsedRule, IRegisteredDatasets? Datasets, IRegisteredDataObjects? DataObjects);
}