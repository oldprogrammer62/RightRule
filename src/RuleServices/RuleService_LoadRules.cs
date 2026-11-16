using RightRule.RuleServices.RuleRegistration;

namespace RightRule.RuleServices;

internal sealed partial class RuleService : IRuleService
{
    public List<RuleRegistrationResult> LoadRules(string JsonData)
    {
        ArgumentNullException.ThrowIfNullOrWhiteSpace(JsonData, nameof(JsonData));

        var ruleDef = _ruleLoaderService.LoadRules(JsonData);
        if (ruleDef == null)
        {
            return [];
        }
        return RegisterRules(ruleDef);
    }
}
