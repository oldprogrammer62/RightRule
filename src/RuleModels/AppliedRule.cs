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
            Number = parsedRule.RuleDefinition.Number,
            Name = parsedRule.RuleDefinition.Name,
            ParsedRule = parsedRule,
            StatementApplied = result.StatementApplied,
            Decision = parsedRule.RuleDefinition.Decision,
            DecisionData = parsedRule.RuleDefinition.DecisionData,
            FalseDecision = parsedRule.RuleDefinition.FalseDecision,
            FalseDecisionData = parsedRule.RuleDefinition.FalseDecisionData,
            IsSuccess = result.IsSuccess,
            Result = result.Result
        };
    }

    internal static AppliedRule BuildFalseResult(ParsedRule parsedRule, RuleEvaluationResult result)
    {
        return new AppliedRule()
        {
            Number = parsedRule.RuleDefinition.Number,
            Name = parsedRule.RuleDefinition.Name,
            ParsedRule = parsedRule,
            StatementApplied = result.StatementApplied,
            Decision = parsedRule.RuleDefinition.Decision,
            DecisionData = parsedRule.RuleDefinition.DecisionData,
            FalseDecision = parsedRule.RuleDefinition.FalseDecision,
            FalseDecisionData = parsedRule.RuleDefinition.FalseDecisionData,
            IsSuccess = result.IsSuccess,
            Result = result.Result
        };
    }

    public override string ToString()
    {
        var statementApplied = !string.IsNullOrWhiteSpace(StatementApplied) ? StatementApplied : "Enable Diagnostics";

        StringBuilder sb = new StringBuilder();
        sb.AppendLine($"Number: {this.ParsedRule.RuleDefinition.Number}");
        sb.AppendLine($"Name: {this.ParsedRule.RuleDefinition.Name}");
        sb.AppendLine($"Context: {this.ParsedRule.RuleDefinition.Context}");
        sb.AppendLine($"Statement: {this.ParsedRule.RuleDefinition.Statement}");
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
