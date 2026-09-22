using RightRule.LanguageProcessor.Parser;
using RightRule.LanguageProcessor.StatementBuilder;
using RightRule.Models;
using RightRule.Models.RuleModels;
using RightRule.OperatorServices;
using RightRule.RegistrationServices.DataRegistration;
using RightRule.System.Common;

namespace RightRule.LanguageProcessor.Evaluator;

internal sealed class Evaluator : IEvaluator
{
    private readonly IDataRegistrationService _dataService;
    private readonly IOperatorService _operatorService;
    private readonly IStatementBuilder _statementBuilder;

    public Evaluator(IDataRegistrationService dataService, IOperatorService operatorService, IStatementBuilder statementBuilder)
    {
        _dataService = dataService;
        _operatorService = operatorService;
        _statementBuilder = statementBuilder;
    }

    public Task<EvaluationResult> EvaluateAsync(ParsedRule Rule)
    {
        string statementApplied = string.Empty;
        return Task.Run(async () =>
        {
            try
            {
                var nodeResult = await EvaluateNodeAsync(Rule).ConfigureAwait(false);
                if (nodeResult == null)
                {
                    return EvaluationResult.FailureResult(false, false, "Evaluation invalid.", statementApplied);
                }
                if (!nodeResult.Success)
                {
                    statementApplied = await BuildStatementApplied(Rule);
                    return EvaluationResult.FailureResult(nodeResult.Success, nodeResult.Result, nodeResult.Message, statementApplied);
                }

                statementApplied = await BuildStatementApplied(Rule);
                return EvaluationResult.SuccessResult(nodeResult.Success, nodeResult.Result, nodeResult.Message, statementApplied);
            }
            catch (Exception ex)
            {
                return EvaluationResult.ErrorResult(ex.Message);
            }
        });
    }

    private async Task<string> BuildStatementApplied(ParsedRule Rule)
    {
        var node = Rule.Node;

        var diagnostics = Rule.RuleConfiguration?.Diagnostics ?? RightRuleConfiguration.DEFAULT_DIAGNOSTICS;
        var statement = Rule.Statement;
        return diagnostics ? await BuildStatementAsync(statement, node!).ConfigureAwait(false) : string.Empty;
    }

    private async Task<OperatorResult> EvaluateNodeAsync(ParsedRule Rule)
    {
        if (Rule == null)
        {
            return OperatorResult.FailureResult("XXX", "Rule is invalid/empty.");
        }

        var diagnostics = Rule.RuleConfiguration?.Diagnostics ?? RightRuleConfiguration.DEFAULT_DIAGNOSTICS;

        return await EvaluateNodeAsync(Rule.Node, Rule.OptionalData, diagnostics).ConfigureAwait(false);
    }

    private async Task<OperatorResult> EvaluateNodeAsync(Node? Node, object? OptionalData, bool Diagnostics)
    {
        var result = Node switch
        {
            ConditionNode c => await EvaluateConditionAsync(c, OptionalData, Diagnostics).ConfigureAwait(false),
            BinaryNode b => await EvaluateBinaryConditionAsync(b, OptionalData, Diagnostics).ConfigureAwait(false),
            NotNode n => (await EvaluateNodeAsync(n.Inner, OptionalData, Diagnostics).ConfigureAwait(false)).Invert(),
            _ => OperatorResult.FailureResult("XXX", $"Unknown Node Type: {Node}")
        };
        return result;
    }

    private async Task<OperatorResult> EvaluateBinaryConditionAsync(BinaryNode Node, object? OptionalData, bool Diagnostics)
    {
        string op = Node.LogicOp?.ToLowerInvariant() ?? string.Empty;
        if (op != "and" && op != "or")
        {
            return OperatorResult.FailureResult(op, $"Unknown logical operator: {Node.LogicOp}");
        }

        // 1. Evaluate the left side
        var leftResult = await EvaluateNodeAsync(Node.Left, OptionalData, Diagnostics).ConfigureAwait(false);
        if (!leftResult.Success) return leftResult; // Bubble up the error immediately

        // 2. Perform Short-Circuit Checks
        if (op == "and" && !leftResult.Result) return OperatorResult.SuccessResult(op, false);
        if (op == "or" && leftResult.Result) return OperatorResult.SuccessResult(op, true);

        // 3. Evaluate the right side (only if short-circuit didn't trigger)
        var rightResult = await EvaluateNodeAsync(Node.Right, OptionalData, Diagnostics).ConfigureAwait(false);
        if (!rightResult.Success) return rightResult; // Bubble up the error immediately

        // 4. If both succeeded, the final value is just the right side's value
        return OperatorResult.SuccessResult(op, rightResult.Result);
    }

    private async Task<OperatorResult> EvaluateConditionAsync(ConditionNode Node, object? OptionalData, bool Diagnostics)
    {
        string op = Node.Operator?.ToLowerInvariant() ?? string.Empty;

        try
        {
            var left = EvaluatorHelper.ResolveOperandValue(Node.Left, _dataService);
            var right = EvaluatorHelper.ResolveOperandValue(Node.Right, _dataService);

            var result = await ProcessOperator(op, left!, right!, OptionalData, Diagnostics).ConfigureAwait(false);
            return result;
        }
        catch (Exception ex)
        {
            return OperatorResult.FailureResult(op, ex.Message);
        }
    }

    private async Task<OperatorResult> ProcessOperator(string LogicOperator, object Left, object Right, object? OptionalData, bool Diagnostics)
    {
        var op = _operatorService.OperatorAliases.FirstOrDefault(x => x.Key == LogicOperator).Value;
        if (op == default)
        {
            throw new KeyNotFoundException($"Unknown operator: {LogicOperator}");
        }

        try
        {
            return await op.ProcessAsync(Left, Right, OptionalData, Diagnostics).ConfigureAwait(false);
        }
        catch
        {
            throw;
        }
    }

    private async Task<string> BuildStatementAsync(string Statement, Node Node)
    {
        try
        {
            return await _statementBuilder.BuildAppliedStatement(Node, Statement).ConfigureAwait(false);
        }
        catch
        {
            throw;
        }
    }
}
