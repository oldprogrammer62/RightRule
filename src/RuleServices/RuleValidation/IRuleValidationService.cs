using RightRule.RuleServices.RuleModels;

namespace RightRule.RuleServices.RuleValidation;

internal interface IRuleValidationService
{
    RuleValidationResult ValidateRule(DefinedRule rule);
}