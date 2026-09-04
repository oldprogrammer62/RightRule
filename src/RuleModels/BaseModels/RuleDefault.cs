using RightRule.Models.RuleRegistration;

namespace RightRule.RuleModels.BaseModels;

public abstract class RuleDefault : IProcessedRule
{
    public const string DECISIONWORD = "NODECISION";
    public const string RULENAME = "DEDFAULTRULE";

    int IRule.Number { get; } = (int)RuleNumberEnum.DefaultRule;
    public string Name { get; init; } = RULENAME;
    public string Context { get; init; } = string.Empty;

    bool IDecided.IsSuccess { get; } = true;
    bool IDecided.Result { get; } = true;
    string IDecided.StatementApplied { get; } = string.Empty;
    string IDecided.Message {  get; } = string.Empty;

    public string Decision { get; init; } = DECISIONWORD;
    public object? DecisionData { get; init; } = null;

    string IDecideFalse.FalseDecision { get; } = string.Empty;
    object? IDecideFalse.FalseDecisionData { get; } = null;
}
