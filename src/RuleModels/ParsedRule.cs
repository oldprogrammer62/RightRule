using RightRule.RegistrationServices;
using RightRule.RuleModels.BaseModels;
using RightRule.StatementProcessor.Parser;
using System.Text;

namespace RightRule.RuleModels;

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

    internal static ParsedRule? SuccessResult(DefinedRule? ruleDefinition, Node node, string message)
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
            Status = RegistrationStatus.SUCCESS,
            Message = message,
            StatementPath = BuildTreeText(ruleDefinition, node),
            Node = node,
            OptionalData = ruleDefinition.OptionalData,
            RuleConfiguration = ruleDefinition.RuleConfiguration
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
            Decision = ruleDefinition.Decision,
            DecisionData = ruleDefinition.DecisionData,
            FalseDecision = ruleDefinition.FalseDecision,
            FalseDecisionData = ruleDefinition.FalseDecisionData,
            Statement = ruleDefinition.Statement,
            Status = RegistrationStatus.FAILED,
            Message = message,
            StatementPath = string.Empty,
            Node = null,
            OptionalData = ruleDefinition.OptionalData,
            RuleConfiguration = ruleDefinition.RuleConfiguration
        };
    }

    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine($"Number: {this.Number}");
        sb.AppendLine($"Name: {this.Name}");
        sb.AppendLine($"Context: {this.Context}");
        sb.AppendLine($"Statement: {this.Statement}");
        sb.AppendLine($"Configuration: {this.OptionalData}");
        sb.AppendLine($"Decision: {this.Decision}");
        sb.AppendLine($"DecisionData: {this.DecisionData}");
        sb.AppendLine($"FalseDecision: {this.FalseDecision}");
        sb.AppendLine($"FalseDecisionData: {this.FalseDecisionData}");
        sb.AppendLine($"OptionalData: {this.OptionalData}");
        sb.AppendLine($"StatementPath: {this.StatementPath}");
        sb.AppendLine($"Message: {this.Message}");
        return sb.ToString();
    }

    private static string BuildTreeText(DefinedRule? ruleDefinition, Node node)
    {
        bool diagnostics = Helper.GetDiagnosticsSetting(ruleDefinition);
        if (diagnostics)
        {
            return StatementProcessor.StatementPathBuilder.BuildTreeText(node);
        }
        return string.Empty;
    }
}
