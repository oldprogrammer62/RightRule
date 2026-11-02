using System.Globalization;
using Microsoft.Extensions.Logging.Testing;
using RightRule.DataServices;
using RightRule.DataServices.DataValidator;
using RightRule.LanguageProcessor.Parser;
using RightRule.LanguageProcessor.StatementBuilder;

namespace RightRuleTests;

public class StatementBuilderTests2
{
    [Fact]
    public async Task BuildAppliedStatement_NullNodeReturnsOriginalStatement()
    {
        var builder = CreateBuilder();

        var result = await builder.BuildAppliedStatement(null, "Customer.Age GreaterThan 18");

        Assert.Equal("Customer.Age GreaterThan 18", result);
    }

    [Fact]
    public async Task BuildAppliedStatement_NullStatementWithNullNodeReturnsEmptyString()
    {
        var builder = CreateBuilder();

        var result = await builder.BuildAppliedStatement(null, null!);

        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public async Task BuildAppliedStatement_RendersIdentifierAndFormatsResolvedValues()
    {
        var dataService = CreateDataService();
        dataService.AddData("Customer", "Name", "A \"quote\"\\path");
        var builder = new StatementBuilder(dataService);
        var node = Condition(
            OperandKindEnum.IDENTIFIER, "Customer.Name",
            OperandKindEnum.NUMBER, "18.5",
            "equals");

        var result = await builder.BuildAppliedStatement(node, "ignored");

        Assert.Equal("\"A \\\"quote\\\"\\\\path\" equals 18.5", result);
    }

    [Fact]
    public async Task BuildAppliedStatement_ResolvesVariablesAndFormatsBooleans()
    {
        var dataService = CreateDataService();
        dataService.AddData("Threshold", "Threshold", 21);
        var builder = new StatementBuilder(dataService);
        var node = Condition(
            OperandKindEnum.VARIABLE, "Threshold",
            OperandKindEnum.BOOLEAN, "true",
            "is");

        var result = await builder.BuildAppliedStatement(node, "ignored");

        Assert.Equal("21 is true", result);
    }

    [Fact]
    public async Task BuildAppliedStatement_RendersArrayLiterals()
    {
        var builder = CreateBuilder();
        var node = Condition(
            OperandKindEnum.ARRAY, "[\"one\", \"two\"]",
            OperandKindEnum.ARRAY, "[1, 2.5]",
            "contains");

        var result = await builder.BuildAppliedStatement(node, "ignored");

        Assert.Equal("[\"one\", \"two\"] contains [1, 2.5]", result);
    }

    [Fact]
    public async Task BuildAppliedStatement_RendersBinaryAndNotNodesRecursively()
    {
        var builder = CreateBuilder();
        var node = new NotNode
        {
            Inner = new BinaryNode
            {
                LogicOp = "and",
                Left = Condition(
                    OperandKindEnum.STRING, "left",
                    OperandKindEnum.STRING, "right",
                    "equals"),
                Right = Condition(
                    OperandKindEnum.BOOLEAN, "false",
                    OperandKindEnum.NUMBER, "0",
                    "is")
            }
        };

        var result = await builder.BuildAppliedStatement(node, "ignored");

        Assert.Equal("(NOT (\"left\" equals \"right\" AND false is 0))", result);
    }

    [Fact]
    public async Task BuildAppliedStatement_RendersMissingValuesAsNull()
    {
        var builder = CreateBuilder();
        var node = Condition(
            OperandKindEnum.IDENTIFIER, "Missing.Value",
            OperandKindEnum.STRING, "expected",
            "equals");

        var result = await builder.BuildAppliedStatement(node, "ignored");

        Assert.Equal("null equals \"expected\"", result);
    }

    [Fact]
    public async Task BuildAppliedStatement_UsesInvariantNumberFormatting()
    {
        var builder = CreateBuilder();
        var node = Condition(
            OperandKindEnum.NUMBER, "12.5",
            OperandKindEnum.NUMBER, "1000",
            "equals");
        var originalCulture = CultureInfo.CurrentCulture;

        try
        {
            CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("fr-FR");

            var result = await builder.BuildAppliedStatement(node, "ignored");

            Assert.Equal("12.5 equals 1000", result);
        }
        finally
        {
            CultureInfo.CurrentCulture = originalCulture;
        }
    }

    [Fact]
    public async Task BuildAppliedStatement_UnknownNodeReturnsEmptyString()
    {
        var builder = CreateBuilder();

        var result = await builder.BuildAppliedStatement(new UnknownNode(), "ignored");

        Assert.Equal(string.Empty, result);
    }

    private static StatementBuilder CreateBuilder() =>
        new(CreateDataService());

    private static DataService CreateDataService() =>
        new(new FakeLogger<DataService>(), new DataValidatorService());

    private static ConditionNode Condition(
        OperandKindEnum leftKind,
        string leftRaw,
        OperandKindEnum rightKind,
        string rightRaw,
        string operation) =>
        new()
        {
            Left = new Operand(leftKind, leftRaw),
            Operator = operation,
            Right = new Operand(rightKind, rightRaw)
        };

    private sealed class UnknownNode : Node;
}