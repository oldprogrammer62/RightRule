using RightRule.Models.RuleModels;
using RightRule.RegistrationServices.DataRegistration;

namespace RightRule.LanguageProcessor.Evaluator;

public interface IEvaluator
{
    Task<EvaluationResult> EvaluateAsync(ParsedRule ParsedRule, IRegisteredDatasets? Datasets, IRegisteredDataObjects? DataObjects, InewGetDataService getDataService);
}
