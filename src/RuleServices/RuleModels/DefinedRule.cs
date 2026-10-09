using RightRule.ConfigurationService;
using RightRule.RuleServices.RuleModels.BaseModels;
using RightRule.RuleServices.RuleRegistration;
using System.Text;

namespace RightRule.RuleServices.RuleModels;

/// <summary>
/// Defines of a Rule for registration with RightRule.
/// <para>Number: Identifies the Rule by number. Used when ordering rules for evaluation. Ordering is low to high.</para>
/// <para>Name: Identifies the Rule by name.</para>
/// <para>Context: Binds the Rule to a specific event context defined in code.</para>
/// <para>Statement: The logic of the Rule, evaluated to determine if the rule is true or false. The statement must conform to the RightRule statement syntax.</para>
/// <para>OptionalData: Optional data that will be passed to the Operator when evaluating the Rule. It is the responsibility of the Operator to validate the configuration details and use it as required.</para>
/// <para>Decision: String value returned if the Rule evaluates to True. Note: the calling application should handle all possible decision strings for the specified Context.</para>
/// <para>DecisionData: Optional data returned when the Rule is evaluated to true. Note: the calling application should be capable of using this data when taking the appropriate action based on the decision.</para>
/// </summary> 
public sealed class DefinedRule : RuleDefinition
{
    public DefinedRule() : base()
    {
    }

    public RuleRegistrationResult Register(IRuleService ruleRegistrationService)
    {
        return ruleRegistrationService.RegisterRule(this);
    }

    public static DefinedRule Build(int Number, string Name, string Context, string Statement, string Decision, object? DecisionData, string FalseDecision, object? FalseDecisionData, object? OptionalData, RightRuleConfiguration ruleConfiguration)
    {
        return new DefinedRule()
        {
            Number = Number,
            Name = Name,
            Context = Context,
            Statement = Statement,
            Decision = Decision,
            DecisionData = DecisionData,
            FalseDecision = FalseDecision,
            FalseDecisionData = FalseDecisionData,
            OptionalData = OptionalData,
            RuleConfiguration = ruleConfiguration
        };
    }

    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine($"Number: {Number}");
        sb.AppendLine($"Name: {Name}");
        sb.AppendLine($"Context: {Context}");
        sb.AppendLine($"Statement: {Statement}");
        sb.AppendLine($"Decision: {Decision}");
        sb.AppendLine($"DecisionData: {DecisionData}");
        sb.AppendLine($"FalseDecision: {FalseDecision}");
        sb.AppendLine($"FalseDecisionData: {FalseDecisionData}");
        sb.AppendLine($"OptionalData: {OptionalData}");
        sb.AppendLine($"Configuration - Diagnostics: {RuleConfiguration?.Diagnostics}");
        return sb.ToString();
    }
}
