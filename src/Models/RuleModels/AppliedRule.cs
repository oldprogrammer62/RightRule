using RightRule.RuleModels.BaseModels;
using RightRule.RuleModels.RuleEvaluation;
using System.Text;

namespace RightRule.RuleModels.RuleModels;

/// <summary>
/// Represents a rule that has been applied and evaluated as true against a dataset.
/// </summary>
public sealed class AppliedRule : RuleApplied
{
    private AppliedRule() : base()
    {
    }

    internal static AppliedRule BuildTrueResult(ParsedRule parsedRule, RuleEvaluationResult result)
    {
        return new AppliedRule()
        {
            Number = parsedRule.Number,
            Name = parsedRule.Name,
            Context = parsedRule.Context,
            Statement = parsedRule.Statement,
            StatementApplied = result.StatementApplied,
            Decision = parsedRule.Decision,
            DecisionData = parsedRule.DecisionData,
            FalseDecision = parsedRule.FalseDecision,
            FalseDecisionData = parsedRule.FalseDecisionData,
            IsSuccess = result.IsSuccess,
            Result = result.Result,
            Message = result.Message
        };
    }

    internal static AppliedRule BuildFalseResult(ParsedRule parsedRule, RuleEvaluationResult result)
    {
        return new AppliedRule()
        {
            Number = parsedRule.Number,
            Name = parsedRule.Name,
            Context = parsedRule.Context,
            Statement = parsedRule.Statement,
            StatementApplied = result.StatementApplied,
            Decision = parsedRule.Decision,
            DecisionData = parsedRule.DecisionData,
            FalseDecision = parsedRule.FalseDecision,
            FalseDecisionData = parsedRule.FalseDecisionData,
            IsSuccess = result.IsSuccess,
            Result = result.Result,
            Message = result.Message
        };
    }

    public override string ToString()
    {
        var statementApplied = !string.IsNullOrWhiteSpace(StatementApplied) ? StatementApplied : "Enable Diagnostics";

        StringBuilder sb = new StringBuilder();
        sb.AppendLine($"Number: {Number}");
        sb.AppendLine($"Name: {Name}");
        sb.AppendLine($"Context: {Context}");
        sb.AppendLine($"Statement: {Statement}");
        sb.AppendLine($"StatementApplied: {statementApplied}");
        sb.AppendLine($"Decision: {Decision}");
        sb.AppendLine($"DecisionData: {DecisionData}");
        sb.AppendLine($"FalseDecision: {FalseDecision}");
        sb.AppendLine($"FalseDecisionData: {FalseDecisionData}");
        sb.AppendLine($"IsSuccess: {IsSuccess}");
        sb.AppendLine($"Result: {Result}");
        sb.AppendLine($"Message: {Message}");
        return sb.ToString();
    }
}
