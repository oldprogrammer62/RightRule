using RightRule.RuleServices.DefaultRuleRegistration;
using RightRule.RuleServices.RuleModels.BaseModels;

namespace RightRule.RuleServices.RuleModels;

/// <summary>
/// Represents a rule that has been applied and evaluated as true against a dataset.
/// </summary>
public sealed class DefaultRule : RuleDefault
{
    public DefaultRule() : base()
    {
    }

    internal static DefaultRule DefaultResult()
    {
        return new DefaultRule();
    }

    internal DefaultRuleRegistrationResult Register(IRuleService ruleRegistrationService)
    {
        return ruleRegistrationService.RegisterDefaultRule(this);
    }

    internal static DefaultRule Build(string Name, string Context, string Decision, object? DecisionData)
    {
        return new DefaultRule()
        {
            Name = Name,
            Context = Context,
            Decision = Decision,
            DecisionData = DecisionData
        };
    }
}
