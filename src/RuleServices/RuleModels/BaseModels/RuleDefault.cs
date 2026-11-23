using System.Text;

namespace RightRule.RuleServices.RuleModels.BaseModels;

public abstract class RuleDefault : IProcessedRule
{
    public const string DECISIONWORD = "NODECISION";
    public const string RULENAME = "DefaultRule";

    int IRule.Number { get; } = (int)RuleNumberEnum.DEFAULTRULE;
    public string Name { get; init; } = RULENAME;
    public string Context { get; init; } = string.Empty;

    bool IDecided.Success { get; } = true;
    bool IDecided.Result { get; } = true;
    string IDecided.StatementApplied { get; } = string.Empty;
    string IDecided.Message {  get; } = string.Empty;

    public string Decision { get; init; } = DECISIONWORD;
    public object? DecisionData { get; init; } = null;

    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine($"Number: {(int)RuleNumberEnum.DEFAULTRULE}");
        sb.AppendLine($"Name: {Name}");
        sb.AppendLine($"Context: {Context}");
        sb.AppendLine($"Decision: {Decision}");
        sb.AppendLine($"DecisionData: {DecisionData}");
        return sb.ToString();
    }
}
