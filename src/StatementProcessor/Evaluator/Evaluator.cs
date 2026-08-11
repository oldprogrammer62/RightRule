using RightRule.Data;
using RightRule.Models;
using RightRule.RuleModels;
using RightRule.StatementProcessor.Parser;
using RightRule.System.Common;

namespace RightRule.StatementProcessor.Evaluator;

internal sealed class Evaluator
{
    private readonly Dictionary<string, Operator> _operators;

    private readonly RightRuleConfiguration _rightRuleConfig;

    internal Evaluator(Dictionary<string, Operator> Operators, RightRuleConfiguration rightRuleConfig)
    {
        _operators = Operators ?? new Dictionary<string, Operator> { };
        _rightRuleConfig = rightRuleConfig;
    }

    internal Task<RuleResult> EvaluateAsync(ParsedRule parsedRule, RegisteredDatasets? Datasets, RegisteredDataObjects? DataObjects)
    {
        var diagnostics = _rightRuleConfig.DiagnosticsEnabled;

        return Task.Run(async () =>
        {
            try
            {
                var evaluationResult = await EvaluateNodeAsync(Datasets, DataObjects, parsedRule).ConfigureAwait(false);
                if (evaluationResult == null)
                {
                    return RuleResult.Failure(evaluationResult!, "Evaluation invalid.");
                }
                if (!evaluationResult.IsSuccess)
                {
                    return RuleResult.Failure(evaluationResult, evaluationResult.Message);
                }

                var node = parsedRule.Node;
                var statement = parsedRule.Statement;

                string message = string.Empty;
                string statementApplied = diagnostics ? await BuildStatementAsync(statement, Datasets, DataObjects, node!).ConfigureAwait(false) : string.Empty;
                string syntaxTree = diagnostics ? await BuildSyntaxTreeAsync(parsedRule).ConfigureAwait(false) : string.Empty;
                return RuleResult.Success(evaluationResult, message, statementApplied, syntaxTree);
            }
            catch (Exception ex)
            {
                return RuleResult.Failure(ex.Message);
            }
        });
    }

    private async Task<EvaluationResult> EvaluateNodeAsync(RegisteredDatasets? datasets, RegisteredDataObjects? dataObjects, ParsedRule rule)
    {
        if (rule == null)
        {
            return EvaluationResult.Failure("Rule is invalid.");
        }

        var node = rule.Node;
        var config = rule.OptionalData;

        return await EvaluateNodeAsync(datasets, dataObjects, node, config).ConfigureAwait(false);
    }

    private async Task<EvaluationResult> EvaluateNodeAsync(RegisteredDatasets? datasets, RegisteredDataObjects? dataObjects, Node? node, object? Config)
    {
        return node switch
        {
            ConditionNode c => await EvaluateConditionAsync(datasets, dataObjects, c, Config).ConfigureAwait(false),
            BinaryNode b => await EvaluateBinaryAsync(datasets, dataObjects, b, Config).ConfigureAwait(false),
            NotNode n => (await EvaluateNodeAsync(datasets, dataObjects, n.Inner, Config).ConfigureAwait(false)).Invert(),
            _ => EvaluationResult.Failure($"Unknown Node Type: {node}")
        };
    }

    private async Task<EvaluationResult> EvaluateBinaryAsync(RegisteredDatasets? datasets, RegisteredDataObjects? dataObjects, BinaryNode b, object? Config)
    {
        string op = b.LogicOp?.ToLowerInvariant() ?? string.Empty;
        if (op != "and" && op != "or")
        {
            return EvaluationResult.Failure($"Unknown logical operator: {b.LogicOp}");
        }

        // 1. Evaluate the left side
        var leftResult = await EvaluateNodeAsync(datasets, dataObjects, b.Left, Config).ConfigureAwait(false);
        if (!leftResult.IsSuccess) return leftResult; // Bubble up the error immediately

        // 2. Perform Short-Circuit Checks
        if (op == "and" && !leftResult.Value) return EvaluationResult.Success(false);
        if (op == "or" && leftResult.Value) return EvaluationResult.Success(true);

        // 3. Evaluate the right side (only if short-circuit didn't trigger)
        var rightResult = await EvaluateNodeAsync(datasets, dataObjects, b.Right, Config).ConfigureAwait(false);
        if (!rightResult.IsSuccess) return rightResult; // Bubble up the error immediately

        // 4. If both succeeded, the final value is just the right side's value
        return EvaluationResult.Success(rightResult.Value);
    }

    private async Task<EvaluationResult> EvaluateConditionAsync(RegisteredDatasets? datasets, RegisteredDataObjects? dataObjects, ConditionNode c, object? OptionalData)
    {
        try
        {
            var left = EvaluatorHelper.ResolveOperandValue(datasets, dataObjects, c.Left);
            var right = EvaluatorHelper.ResolveOperandValue(datasets, dataObjects, c.Right);

            var result = await ProcessOperator(c.Operator, left!, right!, OptionalData).ConfigureAwait(false);
            return result;
        }
        catch (Exception ex)
        {
            return EvaluationResult.Failure(ex.Message);
        }
    }

    private async Task<EvaluationResult> ProcessOperator(string logicOperator, object left, object right, object? OptionalData)
    {
        if (!_operators.TryGetValue(logicOperator, out var op))
            throw new KeyNotFoundException($"Unknown operator: {logicOperator}");

        try
        {
            return await op.ProcessAsync(left, right, OptionalData).ConfigureAwait(false);
        }
        catch
        {
            throw;
        }
    }

    private async static Task<string> BuildStatementAsync(string Statement, RegisteredDatasets? Datasets, RegisteredDataObjects? dataObjects, Node Node)
    {
        try
        {
            return await StatementBuilder.BuildAppliedStatement(Datasets, dataObjects, Node, Statement).ConfigureAwait(false);
        }
        catch
        {
            throw;
        }
    }

    private async static Task<string> BuildSyntaxTreeAsync(ParsedRule rule)
    {
        try
        {
            return await rule.ToTreeTextAsync().ConfigureAwait(false);
        }
        catch
        {
            throw;
        }
    }
}