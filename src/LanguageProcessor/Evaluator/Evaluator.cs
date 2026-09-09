using RightRule.LanguageProcessor.Parser;
using RightRule.Models;
using RightRule.Models.RuleEvaluation;
using RightRule.Models.RuleModels;
using RightRule.RegistrationServices.DataRegistration;
using RightRule.System.Common;

namespace RightRule.LanguageProcessor.Evaluator;

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
                var nodeResult = await EvaluateNodeAsync(Datasets, DataObjects, ParsedRule).ConfigureAwait(false);
                if (nodeResult == null)
                {
                    return RuleEvaluationResult.FailureResult(false, false, "Evaluation invalid.");
                }
                if (!nodeResult.Success)
                {
                    return RuleEvaluationResult.FailureResult(nodeResult.Success, nodeResult.Result, nodeResult.Message);
                }

                string message = string.Empty;
                string statementApplied = await BuildStatementApplied(ParsedRule, Datasets, DataObjects);
                return RuleEvaluationResult.SuccessResult(nodeResult.Success, nodeResult.Result, message, statementApplied);
            }
            catch (Exception ex)
            {
                return RuleEvaluationResult.ErrorResult(ex.Message);
            }
        });
    }

    private async Task<string> BuildStatementApplied(ParsedRule rule, IRegisteredDatasets? Datasets, IRegisteredDataObjects? DataObjects)
    {
        var node = rule.Node;

        var diagnostics = rule.RuleConfiguration?.Diagnostics ?? RightRuleConfiguration.DEFAULT_DIAGNOSTICS;
        var statement = rule.Statement;
        return diagnostics ? await BuildStatementAsync(statement, Datasets, DataObjects, node!).ConfigureAwait(false) : string.Empty;
    }

    private async Task<OperatorResult> EvaluateNodeAsync(IRegisteredDatasets? Datasets, IRegisteredDataObjects? DataObjects, ParsedRule rule)
    {
        if (rule == null)
        {
            return OperatorResult.FailureResult("XXX", "Rule is invalid/empty.");
        }

        var diagnostics = rule.RuleConfiguration?.Diagnostics ?? RightRuleConfiguration.DEFAULT_DIAGNOSTICS;

        return await EvaluateNodeAsync(Datasets, DataObjects, rule.Node, rule.OptionalData, diagnostics).ConfigureAwait(false);
    }

    private async Task<OperatorResult> EvaluateNodeAsync(IRegisteredDatasets? Datasets, IRegisteredDataObjects? DataObjects, Node? node, object? OptionalData, bool Diagnostics)
    {
        var result = node switch
        {
            ConditionNode c => await EvaluateConditionAsync(Datasets, DataObjects, c, OptionalData, Diagnostics).ConfigureAwait(false),
            BinaryNode b => await EvaluateBinaryConditionAsync(Datasets, DataObjects, b, OptionalData, Diagnostics).ConfigureAwait(false),
            NotNode n => (await EvaluateNodeAsync(Datasets, DataObjects, n.Inner, OptionalData, Diagnostics).ConfigureAwait(false)).Invert(),
            _ => OperatorResult.FailureResult("XXX", $"Unknown Node Type: {node}")
        };
        return result;
    }

    private async Task<OperatorResult> EvaluateBinaryConditionAsync(IRegisteredDatasets? Datasets, IRegisteredDataObjects? DataObjects, BinaryNode b, object? OptionalData, bool Diagnostics)
    {
        string op = b.LogicOp?.ToLowerInvariant() ?? string.Empty;
        if (op != "and" && op != "or")
        {
            return OperatorResult.FailureResult(op, $"Unknown logical operator: {b.LogicOp}");
        }

        // 1. Evaluate the left side
        var leftResult = await EvaluateNodeAsync(Datasets, DataObjects, b.Left, OptionalData, Diagnostics).ConfigureAwait(false);
        if (!leftResult.Success) return leftResult; // Bubble up the error immediately

        // 2. Perform Short-Circuit Checks
        if (op == "and" && !leftResult.Result) return OperatorResult.SuccessResult(op, false);
        if (op == "or" && leftResult.Result) return OperatorResult.SuccessResult(op, true);

        // 3. Evaluate the right side (only if short-circuit didn't trigger)
        var rightResult = await EvaluateNodeAsync(Datasets, DataObjects, b.Right, OptionalData, Diagnostics).ConfigureAwait(false);
        if (!rightResult.Success) return rightResult; // Bubble up the error immediately

        // 4. If both succeeded, the final value is just the right side's value
        return OperatorResult.SuccessResult(op, rightResult.Result);
    }

    private async Task<OperatorResult> EvaluateConditionAsync(IRegisteredDatasets? Datasets, IRegisteredDataObjects? DataObjects, ConditionNode node, object? OptionalData, bool Diagnostics)
    {
        string op = node.Operator?.ToLowerInvariant() ?? string.Empty;

        try
        {
            var left = EvaluatorHelper.ResolveOperandValue(Datasets, DataObjects, node.Left);
            var right = EvaluatorHelper.ResolveOperandValue(Datasets, DataObjects, node.Right);

            var result = await ProcessOperator(op, left!, right!, OptionalData, Diagnostics).ConfigureAwait(false);
            return result;
        }
        catch (Exception ex)
        {
            return OperatorResult.FailureResult(op, ex.Message);
        }
    }

    private async Task<OperatorResult> ProcessOperator(string logicOperator, object left, object right, object? OptionalData, bool Diagnostics)
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