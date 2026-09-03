using RightRule.Models.RuleRegistration;

namespace RightRule.RuleModels.BaseModels;

public abstract class RuleError : IProcessedRule
{
    public const string DECISIONWORD = "ERROR";
    public const string FALSEDECISIONWORD = "ERROR";
    public const string RULENAME = "ErrorRule";

    public int Number { get; init; } = (int)RuleNumberEnum.ErrorRule;
    public string Name { get; init; } = RULENAME;

    public bool IsSuccess { get; } = false;
    public bool Result { get; } = false;

    public string StatementApplied { get; } = string.Empty;

    public string Decision { get; } = DECISIONWORD;
    public object? DecisionData { get; }

    public string FalseDecision { get; } = FALSEDECISIONWORD;
    public object? FalseDecisionData { get; }

    public required string Message { get; init; } = string.Empty;
}
