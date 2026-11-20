using RightRule.RuleServices.RuleModels;
using RightRule.RuleServices.RuleRegistration;

namespace RightRule.RuleServices;

internal sealed partial class RuleService : IRuleService
{
    public List<RuleRegistrationResult> LoadRules(string JsonData)
    {
        ArgumentNullException.ThrowIfNullOrWhiteSpace(JsonData, nameof(JsonData));

        var rules = _ruleLoaderService.LoadRules(JsonData);
        if (rules == null)
        {
            return [];
        }
        if (ValidateRules(rules))
        {
            return RegisterRules(rules);
        }
        return [];
    }

    private bool ValidateRules(List<DefinedRule> rules)
    {
        return _ruleValidationService.ValidateRules(this, rules);
    }
}
