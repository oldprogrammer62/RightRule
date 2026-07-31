using RightRule.Data;
using RightRule.Models;
using RightRule.RuleModels;
using RightRule.System.Common;
using RightRule.System.Common.Exceptions;

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

    internal Task<RuleResult> EvaluateAsync(ParsedRule parsedRule, RegisteredDatasets Datasets)
    {
        var diagnostics = _rightRuleConfig.DiagnosticsEnabled;

        return Task.Run(async () =>
        {
            try
            {
                var node = parsedRule.Node;
                var statement = parsedRule.Statement;

                var evaluationResult = await EvaluateNodeAsync(Datasets, node).ConfigureAwait(false);
                if (!evaluationResult.IsSuccess)
                {
                    return RuleResult.Failure(evaluationResult.ErrorMessage);
                }

                string message = string.Empty;
                string statementApplied = diagnostics ? await BuildStatementAsync(statement, Datasets, node!).ConfigureAwait(false) : string.Empty;
                string syntaxTree = diagnostics ? await BuildSyntaxTreeAsync(parsedRule).ConfigureAwait(false) : string.Empty;
                return new RuleResult(evaluationResult, message, statementApplied, syntaxTree);
            }
            catch (Exception ex)
            {
                return RuleResult.Failure(ex.Message);
            }
        });
    }

    private async Task<EvaluationResult> EvaluateNodeAsync(RegisteredDatasets datasets, Node? node)
    {
        return node switch
        {
            ConditionNode c => await EvaluateConditionAsync(datasets, c).ConfigureAwait(false),
            BinaryNode b => await EvaluateBinaryAsync(datasets, b).ConfigureAwait(false),
            NotNode n => (await EvaluateNodeAsync(datasets, n.Inner).ConfigureAwait(false)).Invert(),
            _ => EvaluationResult.Failure($"Unknown Node Type: {node}")
        };
    }

    private async Task<EvaluationResult> EvaluateBinaryAsync(RegisteredDatasets datasets, BinaryNode b)
    {
        string op = b.LogicOp?.ToLowerInvariant() ?? string.Empty;
        if (op != "and" && op != "or")
        {
            return EvaluationResult.Failure($"Unknown logical operator: {b.LogicOp}");
        }

        // 1. Evaluate the left side
        var leftResult = await EvaluateNodeAsync(datasets, b.Left).ConfigureAwait(false);
        if (!leftResult.IsSuccess) return leftResult; // Bubble up the error immediately

        // 2. Perform Short-Circuit Checks
        if (op == "and" && !leftResult.Value) return EvaluationResult.Success(false);
        if (op == "or" && leftResult.Value) return EvaluationResult.Success(true);

        // 3. Evaluate the right side (only if short-circuit didn't trigger)
        var rightResult = await EvaluateNodeAsync(datasets, b.Right).ConfigureAwait(false);
        if (!rightResult.IsSuccess) return rightResult; // Bubble up the error immediately

        // 4. If both succeeded, the final value is just the right side's value
        return EvaluationResult.Success(rightResult.Value);
    }

    private async Task<EvaluationResult> EvaluateConditionAsync(RegisteredDatasets datasets, ConditionNode c)
    {
        if (!_operators.TryGetValue(c.Operator, out var op))
            throw new KeyNotFoundException($"Unknown operator: {c.Operator}");

        try
        {
            var left = ResolveOperandValue(datasets, c.Left);
            var right = ResolveOperandValue(datasets, c.Right);

            var result = await op.ProcessAsync(left!, right!).ConfigureAwait(false);
            return EvaluationResult.Success(result);
        }
        catch (Exception ex)
        {
            return EvaluationResult.Failure(ex.Message);
        }
    }

    private static object ResolveIdentifier(RegisteredDatasets datasets, string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            throw new InvalidIdentifierException(name, -1, $"Identifier '{name}' cannot be null or empty.");
        }
        if (name.Split('.').Length != 2)
        {
            throw new InvalidIdentifierException(name, -1, $"Identifier '{name}' must be in the format 'datasetName.identifierName'.");
        }

        var split = name.Split('.');
        var datasetname = split[0];
        var identifiername = split[1];
        var ds = datasets.GetDataset(datasetname);
        if (ds is null)
        {
            throw new KeyNotFoundException($"Unknown dataset name '{datasetname}'.");
        }

        if (!ds.Data.TryGetValue(identifiername, out _))
        {
            throw new KeyNotFoundException($"Unknown variable '{identifiername}' in Registered Dataset {datasetname}.");
        }

        ds.Data.TryGetValue(identifiername, out var result);
        return result!;
    }

    private static object? ResolveOperandValue(RegisteredDatasets datasets, Operand op)
    {
        try
        {
            return ResolveOperand(datasets, op);
        }
        catch
        {
            throw;
        }
    }

    private static object? ResolveOperand(RegisteredDatasets datasets, Operand op)
    {
        try
        {
            object opValue = ResolveOperatorValue(op);

            return op.Kind switch
            {
                OperandKind.Identifier => opValue is string sId ? ResolveIdentifier(datasets, sId) : ResolveIdentifier(datasets, op.Raw),
                OperandKind.Number => opValue is string sNum ? ParseNumber(sNum) : opValue,
                OperandKind.Boolean => opValue is string sBool ? ParseBoolean(sBool) : opValue,
                OperandKind.String => opValue is string s ? s : (opValue?.ToString() ?? string.Empty),
                _ => opValue
            };
        }
        catch
        {
            throw;
        }
    }

    private static string ResolveOperatorValue(Operand operand)
    {
        try
        {
            return operand.Raw;
        }
        catch
        {
            throw;
        }
    }

    private static object ParseBoolean(string raw)
    {
        try
        {
            return Helper.ParseBoolean(raw);
        }
        catch
        {
            throw;
        }
    }

    private static object ParseNumber(string raw)
    {
        try
        {
            return Helper.ParseNumber(raw);
        }
        catch
        {
            throw;
        }
    }

    private async static Task<string> BuildStatementAsync(string Statement, RegisteredDatasets Datasets, Node Node)
    {
        try
        {
            return await StatementBuilder.BuildAppliedStatement(Datasets, Node, Statement).ConfigureAwait(false);
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
            return await rule.ToTreeText().ConfigureAwait(false);
        }
        catch
        {
            throw;
        }
    }
}