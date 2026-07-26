using RightRule.Data;
using RightRule.Models;
using RightRule.Rules;
using RightRule.System.Common;
using RightRule.System.Common.Exceptions;

namespace RightRule.StatementProcessor.Evaluator;

public sealed class Evaluator
{
    private readonly Dictionary<string, IOperator> _operators;

    private readonly RightRuleConfiguration _rightRuleConfig;
    internal RightRuleConfiguration RightRuleConfig => _rightRuleConfig;

    internal Evaluator(Dictionary<string, IOperator> Operators, RightRuleConfiguration rightRuleConfig)
    {
        _operators = Operators ?? new Dictionary<string, IOperator> { };
        _rightRuleConfig = rightRuleConfig;
    }

    internal async Task<EvalResult> Evaluate(ParsedRule parsedRule, RegisteredDatasets Datasets)
    {
        var diagnostics = _rightRuleConfig.DiagnosticsEnabled;

        try
        {
            var node = parsedRule.Node;
            var statement = parsedRule.Statement;

            bool result = await EvaluateNode(Datasets, node);
            string message = string.Empty;
            string statementApplied = diagnostics ? await BuildStatement(statement, Datasets, node!) : string.Empty;
            string syntaxTree = diagnostics ? await BuildSyntaxTree(parsedRule) : string.Empty;
            return new EvalResult(result, message, statementApplied, syntaxTree);
        }
        catch
        {
            throw;
        }
    }

    private Task<bool> EvaluateNode(RegisteredDatasets datasets, Node? node)
    {
        return Task.Run<bool>(async () =>
        {
            return node switch
            {
                ConditionNode c => await EvaluateCondition(datasets, c),
                BinaryNode b => await EvaluateBinary(datasets, b),
                NotNode n => ! await EvaluateNode(datasets, n.Inner),
                _ => throw new Exception("Unknown Node type.")
            };
        });
    }

    private async Task<bool> EvaluateBinary(RegisteredDatasets datasets, BinaryNode b)
    {
        if (b.LogicOp.ToLower() == "and")
        {
            var left = await EvaluateNode(datasets, b.Left);
            return left && await EvaluateNode(datasets, b.Right);
        }
        if (b.LogicOp.ToLower() == "or")
        {
            var left = await EvaluateNode(datasets, b.Left);
            return left || await EvaluateNode(datasets, b.Right);
        }
        throw new Exception($"Unknown logical operator: {b.LogicOp}");
    }

    private async Task<bool> EvaluateCondition(RegisteredDatasets datasets, ConditionNode c)
    {
        if (!_operators.TryGetValue(c.Operator, out var op))
            throw new KeyNotFoundException($"Unknown operator: {c.Operator}");

        var left = await ResolveOperandValue(datasets, c.Left);
        var right = await ResolveOperandValue(datasets, c.Right);
        return op.Process(left!, right!);
    }

    private static Task<object> ResolveIdentifier(RegisteredDatasets datasets, string name)
    {
        return Task.Run(() =>
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
        });
    }

    private async static Task<object?> ResolveOperandValue(RegisteredDatasets datasets, Operand op)
    {
        return await ResolveOperand(datasets, op);
    }

    private async static Task<object?> ResolveOperand(RegisteredDatasets datasets, Operand op)
    {
        object opValue = ResolveOperatorValue(op);

        return op.Kind switch
        {
            OperandKind.Identifier => opValue is string sId ? await ResolveIdentifier(datasets, sId) : ResolveIdentifier(datasets, op.Raw),
            OperandKind.Number => opValue is string sNum ? ParseNumber(sNum) : opValue,
            OperandKind.Boolean => opValue is string sBool ? ParseBoolean(sBool) : opValue,
            OperandKind.String => opValue is string s ? s : (opValue?.ToString() ?? string.Empty),
            _ => opValue
        };
    }

    private static string ResolveOperatorValue(Operand operand)
    {
        return operand.Raw;
    }

    private static object ParseBoolean(string raw)
    {
        return Helper.ParseBoolean(raw);
    }

    private static object ParseNumber(string raw)
    {
        return Helper.ParseNumber(raw);
    }

    private async static Task<string> BuildStatement(string Statement, RegisteredDatasets Datasets, Node Node)
    {
        try
        {
            return await StatementBuilder.BuildAppliedStatement(Datasets, Node, Statement);
        }
        catch (Exception ex)
        {
            return $"Error building statement: {ex.Message}";
        }
    }

    private async static Task<string> BuildSyntaxTree(ParsedRule rule)
    {
        return await rule.ToTreeText();
    }
}