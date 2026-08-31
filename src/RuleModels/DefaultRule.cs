using RightRule.RuleModels.BaseModels;
using System.Text;

namespace RightRule.RuleModels;

/// <summary>
/// Represents a rule that has been applied and evaluated as true against a dataset.
/// </summary>
public sealed class DefaultRule : RuleDefault
{
    private DefaultRule() : base()
    {
    }

    internal static DefaultRule DefaultResult()
    {
        return new DefaultRule();
    }

    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine($"Decision: {RuleDefault.DECISIONWORD}");
        sb.AppendLine($"DecisionData: {this.DecisionData}");
        return sb.ToString();
    }
}
