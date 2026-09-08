using RightRule.RegistrationServices.DataRegistration;
using RightRule.RuleModels.RuleEvaluation;
using RightRule.RuleModels.RuleModels;

namespace RightRule.LanguageProcessor.Evaluator;

public interface IEvaluator
{
    Task<RuleEvaluationResult> EvaluateAsync(ParsedRule ParsedRule, IRegisteredDatasets? Datasets, IRegisteredDataObjects? DataObjects);
}