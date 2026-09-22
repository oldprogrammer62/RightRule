using RightRule.Models.RuleModels;

namespace RightRule.RegistrationServices.DefaultRuleRegistration;

internal class DefaultRulePerContext
{
    public required DefaultRule DefaultRule { get; init;}
    public required string Context { get; init; }
}
