using RightRule.Models.RuleEvaluation;
using RightRule.Models.RuleModels;
using RightRule.RegistrationServices.DataRegistration;

namespace RightRule.LanguageProcessor.Evaluator;

public interface IEvaluator
{
    Task<RuleEvaluationResult> EvaluateAsync(ParsedRule ParsedRule, IRegisteredDatasets? Datasets, IRegisteredDataObjects? DataObjects);
}