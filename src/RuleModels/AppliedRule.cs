using RightRule.Models.RuleEvaluation;
using RightRule.RuleModels.BaseModels;
using System.Text;

namespace RightRule.RuleModels;

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
            ParsedRule = parsedRule,
            Context = parsedRule.Context,
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
            ParsedRule = parsedRule,
            Context = parsedRule.Context,
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
        sb.AppendLine($"Number: {this.ParsedRule.Number}");
        sb.AppendLine($"Name: {this.ParsedRule.Name}");
        sb.AppendLine($"Context: {this.ParsedRule.Context}");
        sb.AppendLine($"Statement: {this.ParsedRule.Statement}");
        sb.AppendLine($"IsSuccess: {this.IsSuccess}");
        sb.AppendLine($"Result: {this.Result}");
        sb.AppendLine($"Decision: {this.Decision}");
        sb.AppendLine($"DecisionData: {this.DecisionData}");
        sb.AppendLine($"FalseDecision: {this.FalseDecision}");
        sb.AppendLine($"FalseDecisionData: {this.FalseDecisionData}");
        sb.AppendLine($"StatementApplied: {statementApplied}");
        return sb.ToString();
    }
}
