using RightRule.LanguageProcessor.Parser;
using RightRule.LanguageProcessor.StatementBuilder;
using RightRule.Models;
using RightRule.RuleServices.RuleModels.BaseModels;
using System.Text;

namespace RightRule.RuleServices.RuleModels;

/// <summary>
/// A ParsedRule is a Rule that has been parsed and has an associated Node representing the parsed rule statement.
/// It is used during the evaluation process to evaluate the rule statement against the provided data.
/// The Node property allows the interpreter to traverse the abstract syntax tree (AST) of the parsed rule statement and evaluate it accordingly.
/// This class is internal
/// </summary>
public sealed class ParsedRule : RuleParsed
{
    private ParsedRule() : base()
    {
    }

    internal static ParsedRule? SuccessResult(DefinedRule? ruleDefinition, Node node, string message, bool Diagnostics)
    {
        if (ruleDefinition == null)
        {
            return null;
        }

        return new ParsedRule()
        {
            Number = ruleDefinition.Number,
            Name = ruleDefinition.Name,
            Context = ruleDefinition.Context,
            Statement = ruleDefinition.Statement,
            Decision = ruleDefinition.Decision,
            DecisionData = ruleDefinition.DecisionData,
            FalseDecision = ruleDefinition.FalseDecision,
            FalseDecisionData = ruleDefinition.FalseDecisionData,
            Status = RightRuleStatusEnum.SUCCESS,
            Message = message,
            StatementPath = BuildTreeText(node, Diagnostics),
            Node = node,
            OptionalData = ruleDefinition.OptionalData
        };
    }

    internal static ParsedRule? ErrorResult(DefinedRule? ruleDefinition, string message)
    {
        if (ruleDefinition == null)
        {
            return null;
        }

        return new ParsedRule()
        {
            Number = ruleDefinition.Number,
            Name = ruleDefinition.Name,
            Context = ruleDefinition.Context,
            Statement = ruleDefinition.Statement,
            Decision = ruleDefinition.Decision,
            DecisionData = ruleDefinition.DecisionData,
            FalseDecision = ruleDefinition.FalseDecision,
            FalseDecisionData = ruleDefinition.FalseDecisionData,
            Status = RightRuleStatusEnum.FAILURE,
            StatementPath = string.Empty,
            Message = message,
            Node = null,
            OptionalData = ruleDefinition.OptionalData
        };
    }

    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine($"Number: {Number}");
        sb.AppendLine($"Name: {Name}");
        sb.AppendLine($"Context: {Context}");
        sb.AppendLine($"Statement: {Statement}");
        sb.AppendLine($"Decision: {Decision}");
        sb.AppendLine($"DecisionData: {DecisionData}");
        sb.AppendLine($"FalseDecision: {FalseDecision}");
        sb.AppendLine($"FalseDecisionData: {FalseDecisionData}");
        sb.AppendLine($"Status: {Status}");
        sb.AppendLine($"StatementPath: {StatementPath}");
        sb.AppendLine($"OptionalData: {OptionalData}");
        sb.AppendLine($"Message: {Message}");
        return sb.ToString();
    }

    private static string BuildTreeText(Node node, bool Diagnostics)
    {
        if (Diagnostics)
        {
            return StatementPathBuilder.BuildTreeText(node);
        }
        return string.Empty;
    }
}
