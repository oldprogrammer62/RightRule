using System.Text;

namespace RightRule.RuleModels;

/// <summary>
/// Defines of a Rule for registration with RightRule.
/// <para>Context: Binds the Rule to a specific event context defined in code.</para>
/// <para>Number: Identifies the Rule by number. Used when ordering rules for evaluation. Ordering is low to high.</para>
/// <para>Name: Identifies the Rule by name.</para>
/// <para>Statement: The logic of the Rule, evaluated to determine if the rule is true or false. The statement must conform to the RightRule statement syntax.</para>
/// <para>OperatorConfig: Optional data that will be passed to the Operator when evaluating the Rule. It is the responsibility of the Operator to validate the configuration details and use it as required.</para>
/// <para>Decision: String value returned if the Rule evaluates to True. Note: the calling application should handle all possible decision strings for the specified Context.</para>
/// <para>DecisionData: Optional data returned when the Rule is evaluated to true. Note: the calling application should be capable of using this data when taking the appropriate action based on the decision.</para>
/// </summary> 
public sealed class RuleDefinition : Rule
{
    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine($"Number: {this.Number}");
        sb.AppendLine($"Name: {this.Name}");
        sb.AppendLine($"Context: {this.Context}");
        sb.AppendLine($"Statement: {this.Statement}");
        sb.AppendLine($"Decision: {this.Decision}");
        sb.AppendLine($"DecisionData: {this.DecisionData}");
        sb.AppendLine($"Configuration: {this.OperatorConfig}");
        return sb.ToString();
    }
}
