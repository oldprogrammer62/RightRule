using RightRule.Models.RuleRegistration;

namespace RightRule.RuleModels.BaseModels;

public abstract class RuleDefault : IProcessedRule
{
    public const string DECISIONWORD = "NODECISION";
    public const string FALSEDECISIONWORD = "NODECISION";

    public int Number { get; init; } = (int)RuleNumberEnum.DefaultRule;
    public string Name { get; init; } = "DefaultRule";

    public bool IsSuccess { get; init; } = true;
    public bool Result { get; init; } = false;

    public string StatementApplied { get; init; } = string.Empty;

    public string Decision { get; } = DECISIONWORD;
    public object? DecisionData { get; init; } = null;

    public string FalseDecision { get; } = FALSEDECISIONWORD;
    public object? FalseDecisionData { get; init; } = null;
}
