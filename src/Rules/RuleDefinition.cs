using System.Text;

namespace RightRule.Rules;

/// <summary>
/// Defines the content of a Rule
/// When a RuleDefinition is registered with RightRule, it becomes available for application
/// </summary> 
public class RuleDefinition : Rule
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
        return sb.ToString();
    }
}
