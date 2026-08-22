using RightRule.DataRegistration;
using RightRule.Models;
using RightRule.RuleModels;
using RightRule.StatementProcessor.Parser;
using RightRule.System.Common;

namespace RightRule.StatementProcessor.Evaluator;

internal sealed class Evaluator : IEvaluator
{
    private readonly Dictionary<string, Operator> _operators;

    public Evaluator(Dictionary<string, Operator> Operators)
    {
        _operators = Operators ?? new Dictionary<string, Operator> { };
    }

    public Task<RuleEvaluationResult> EvaluateAsync(ParsedRule ParsedRule, IRegisteredDatasets? Datasets, IRegisteredDataObjects? DataObjects)
    {
        return Task.Run(async () =>
        {
            try
            {
                var evaluationResult = await EvaluateNodeAsync(Datasets, DataObjects, ParsedRule).ConfigureAwait(false);
                if (evaluationResult == null)
                {
                    return RuleEvaluationResult.Failure(evaluationResult!, "Evaluation invalid.");
                }
                if (!evaluationResult.IsSuccess)
                {
                    return RuleEvaluationResult.Failure(evaluationResult, evaluationResult.Message);
                }

                string message = string.Empty;
                string statementApplied = await BuildStatementApplied(ParsedRule, Datasets, DataObjects);
                return RuleEvaluationResult.Success(evaluationResult, message, statementApplied);
            }
            catch (Exception ex)
            {
                return RuleEvaluationResult.Failure(ex.Message);
            }
        });
    }

    private async Task<string> BuildStatementApplied(ParsedRule rule, IRegisteredDatasets? Datasets, IRegisteredDataObjects? DataObjects)
    {
        var node = rule.Node;

        string message = string.Empty;

        var diagnostics = rule.RuleConfiguration?.Diagnostics ?? RuleConfiguration.DEFAULT_DIAGNOSTICS;
        var statement = rule.Statement;
        return diagnostics ? await BuildStatementAsync(statement, Datasets, DataObjects, node!).ConfigureAwait(false) : string.Empty;
    }

    private async Task<EvaluationResult> EvaluateNodeAsync(IRegisteredDatasets? Datasets, IRegisteredDataObjects? DataObjects, ParsedRule rule)
    {
        if (rule == null)
        {
            return EvaluationResult.Failure("Rule is invalid.");
        }

        var node = rule.Node;
        var optionalData = rule.OptionalData;
        var diagnostics = rule.RuleConfiguration?.Diagnostics ?? RuleConfiguration.DEFAULT_DIAGNOSTICS;

        return await EvaluateNodeAsync(Datasets, DataObjects, node, optionalData, diagnostics).ConfigureAwait(false);
    }

    private async Task<EvaluationResult> EvaluateNodeAsync(IRegisteredDatasets? Datasets, IRegisteredDataObjects? DataObjects, Node? node, object? OptionalData, bool Diagnostics)
    {
        return node switch
        {
            ConditionNode c => await EvaluateConditionAsync(Datasets, DataObjects, c, OptionalData, Diagnostics).ConfigureAwait(false),
            BinaryNode b => await EvaluateBinaryAsync(Datasets, DataObjects, b, OptionalData, Diagnostics).ConfigureAwait(false),
            NotNode n => (await EvaluateNodeAsync(Datasets, DataObjects, n.Inner, OptionalData, Diagnostics).ConfigureAwait(false)).Invert(),
            _ => EvaluationResult.Failure($"Unknown Node Type: {node}")
        };
    }

    private async Task<EvaluationResult> EvaluateBinaryAsync(IRegisteredDatasets? Datasets, IRegisteredDataObjects? DataObjects, BinaryNode b, object? OptionalData, bool Diagnostics)
    {
        string op = b.LogicOp?.ToLowerInvariant() ?? string.Empty;
        if (op != "and" && op != "or")
        {
            return EvaluationResult.Failure($"Unknown logical operator: {b.LogicOp}");
        }

        // 1. Evaluate the left side
        var leftResult = await EvaluateNodeAsync(Datasets, DataObjects, b.Left, OptionalData, Diagnostics).ConfigureAwait(false);
        if (!leftResult.IsSuccess) return leftResult; // Bubble up the error immediately

        // 2. Perform Short-Circuit Checks
        if (op == "and" && !leftResult.Value) return EvaluationResult.Success(false);
        if (op == "or" && leftResult.Value) return EvaluationResult.Success(true);

        // 3. Evaluate the right side (only if short-circuit didn't trigger)
        var rightResult = await EvaluateNodeAsync(Datasets, DataObjects, b.Right, OptionalData, Diagnostics).ConfigureAwait(false);
        if (!rightResult.IsSuccess) return rightResult; // Bubble up the error immediately

        // 4. If both succeeded, the final value is just the right side's value
        return EvaluationResult.Success(rightResult.Value);
    }

    private async Task<EvaluationResult> EvaluateConditionAsync(IRegisteredDatasets? Datasets, IRegisteredDataObjects? DataObjects, ConditionNode c, object? OptionalData, bool Diagnostics)
    {
        try
        {
            var left = EvaluatorHelper.ResolveOperandValue(Datasets, DataObjects, c.Left);
            var right = EvaluatorHelper.ResolveOperandValue(Datasets, DataObjects, c.Right);

            var result = await ProcessOperator(c.Operator, left!, right!, OptionalData, Diagnostics).ConfigureAwait(false);
            return result;
        }
        catch (Exception ex)
        {
            return EvaluationResult.Failure(ex.Message);
        }
    }

    private async Task<EvaluationResult> ProcessOperator(string logicOperator, object left, object right, object? OptionalData, bool Diagnostics)
    {
        if (!_operators.TryGetValue(logicOperator, out var op))
            throw new KeyNotFoundException($"Unknown operator: {logicOperator}");

        try
        {
            return await op.ProcessAsync(left, right, OptionalData, Diagnostics).ConfigureAwait(false);
        }
        catch
        {
            throw;
        }
    }

    private async static Task<string> BuildStatementAsync(string Statement, IRegisteredDatasets? Datasets, IRegisteredDataObjects? DataObjects, Node Node)
    {
        try
        {
            return await StatementBuilder.BuildAppliedStatement(Datasets, DataObjects, Node, Statement).ConfigureAwait(false);
        }
        catch
        {
            throw;
        }
    }
}