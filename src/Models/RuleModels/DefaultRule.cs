using RightRule.Models.BaseModels;
using RightRule.RegistrationServices.Rules;
using System.Text;

namespace RightRule.Models.RuleModels;

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

    public static DefaultRule Build(string Name, string Context, string Decision, object? DecisionData)
    {
        return new DefaultRule()
        {
            Name = Name,
            Context = Context,
            Decision = Decision,
            DecisionData = DecisionData
        };
    }

    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine($"Number: {(int)RuleNumberEnum.DefaultRule}");
        sb.AppendLine($"Name: {Name}");
        sb.AppendLine($"Context: {Context}");
        sb.AppendLine($"Decision: {Decision}");
        sb.AppendLine($"DecisionData: {DecisionData}");
        return sb.ToString();
    }
}
