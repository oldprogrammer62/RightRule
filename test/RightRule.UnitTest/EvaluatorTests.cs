using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Logging.Testing;
using RightRule.DataServices;
using RightRule.DataServices.DataValidator;
using RightRule.LanguageProcessor.Evaluator;
using RightRule.LanguageProcessor.Parser;
using RightRule.LanguageProcessor.StatementBuilder;
using RightRule.Models;
using RightRule.OperatorServices;
using RightRule.OperatorServices.OperatorLoader.CustomOperators;
using RightRule.OperatorServices.OperatorLoader.DefaultOperators;
using RightRule.RuleServices.RuleModels;
using RightRule.System.Common.Operator.Models;

namespace RightRuleTests;

public class EvaluatorTests
{
    private static CustomOperatorLoader CreateCustomLoader() =>
        new(NullLogger<CustomOperatorLoader>.Instance);

    private static DefaultOperatorLoader CreateDefaultLoader() =>
        new(NullLogger<DefaultOperatorLoader>.Instance);

    [Fact]
    public async Task EvaluateAsync_EvaluatesConditionAndPassesOperandsAndOptionalData()
    {
        var optionalData = new { Threshold = 10 };
        var (evaluator, operation, statementBuilder) = CreateEvaluator(
            (_, _, _) => OperatorResult.SuccessResult("check", true),
            diagnostics: true);
        var rule = CreateRule(Condition("check", "left", "right"), optionalData);

        var result = await evaluator.EvaluateAsync(rule);

        Assert.True(result.Success);
        Assert.True(result.Result);
        Assert.Equal("left", operation.LastLeft);
        Assert.Equal("right", operation.LastRight);
        Assert.Same(optionalData, operation.LastOptionalData);
        Assert.Equal("applied statement", result.StatementApplied);
        Assert.Equal(1, statementBuilder.CallCount);
    }

    [Fact]
    public async Task EvaluateAsync_UnknownOperatorReturnsFailure()
    {
        var (evaluator, _, statementBuilder) = CreateEvaluator(
            (_, _, _) => OperatorResult.SuccessResult("check", true));
        var rule = CreateRule(Condition("missing", "left", "right"));

        var result = await evaluator.EvaluateAsync(rule);

        Assert.False(result.Success);
        Assert.False(result.Result);
        Assert.Equal("Unknown operator: missing", result.Message);
        Assert.Empty(result.StatementApplied);
        Assert.Equal(0, statementBuilder.CallCount);
    }

    [Theory]
    [InlineData("and", false, false)]
    [InlineData("or", true, true)]
    public async Task EvaluateAsync_BinaryNodeShortCircuits(
        string logicOperator,
        bool leftValue,
        bool expectedResult)
    {
        var (evaluator, operation, _) = CreateEvaluator(
            (left, _, _) => OperatorResult.SuccessResult("check", bool.Parse((string)left)));
        var rule = CreateRule(new BinaryNode
        {
            LogicOp = logicOperator,
            Left = Condition("check", leftValue.ToString(), "unused"),
            Right = Condition("missing", "right", "unused")
        });

        var result = await evaluator.EvaluateAsync(rule);

        Assert.True(result.Success);
        Assert.Equal(expectedResult, result.Result);
        Assert.Equal(1, operation.CallCount);
    }

    [Theory]
    [InlineData("and", true, false, false)]
    [InlineData("or", false, true, true)]
    public async Task EvaluateAsync_BinaryNodeEvaluatesRightWhenNeeded(
        string logicOperator,
        bool leftValue,
        bool rightValue,
        bool expectedResult)
    {
        var (evaluator, operation, _) = CreateEvaluator(
            (left, _, _) => OperatorResult.SuccessResult("check", bool.Parse((string)left)));
        var rule = CreateRule(new BinaryNode
        {
            LogicOp = logicOperator,
            Left = Condition("check", leftValue.ToString(), "unused"),
            Right = Condition("check", rightValue.ToString(), "unused")
        });

        var result = await evaluator.EvaluateAsync(rule);

        Assert.True(result.Success);
        Assert.Equal(expectedResult, result.Result);
        Assert.Equal(2, operation.CallCount);
    }

    [Fact]
    public async Task EvaluateAsync_NotNodeInvertsResult()
    {
        var (evaluator, _, _) = CreateEvaluator(
            (_, _, _) => OperatorResult.SuccessResult("check", false));
        var rule = CreateRule(new NotNode
        {
            Inner = Condition("check", "left", "right")
        });

        var result = await evaluator.EvaluateAsync(rule);

        Assert.True(result.Success);
        Assert.True(result.Result);
    }

    [Fact]
    public async Task EvaluateAsync_StatementBuilderExceptionReturnsErrorResult()
    {
        var (evaluator, _, _) = CreateEvaluator(
            (_, _, _) => OperatorResult.SuccessResult("check", true),
            diagnostics: true,
            statementBuilder: new ThrowingStatementBuilder("statement failed"));
        var rule = CreateRule(Condition("check", "left", "right"));

        var result = await evaluator.EvaluateAsync(rule);

        Assert.False(result.Success);
        Assert.False(result.Result);
        Assert.Equal("statement failed", result.Message);
        Assert.Empty(result.StatementApplied);
    }

    private static (Evaluator Evaluator, DelegateOperator Operator, RecordingStatementBuilder StatementBuilder)
        CreateEvaluator(
            Func<object, object, object?, OperatorResult> process,
            bool diagnostics = false,
            IStatementBuilder? statementBuilder = null)
    {
        var dataService = new DataService(new FakeLogger<DataService>(), new DataValidatorService());
        var operatorService = new OperatorService(CreateDefaultLoader(), CreateCustomLoader(), new RightRuleOptions
        {
            LoadDefaultOperators = false,
            LoadCustomOperators = false
        });
        var operation = new DelegateOperator(process);
        operatorService.RegisterOperator(operation);

        var recordingBuilder = statementBuilder as RecordingStatementBuilder
            ?? (statementBuilder is null ? new RecordingStatementBuilder() : null);

        return (
            new Evaluator(
                dataService,
                operatorService,
                statementBuilder ?? recordingBuilder!,
                new FakeLogger<Evaluator>(),
                new RightRuleOptions { Diagnostics = diagnostics }),
            operation,
            recordingBuilder!);
    }

    private static ParsedRule CreateRule(Node node, object? optionalData = null)
    {
        var definition = DefinedRule.Build(
            1,
            "Evaluator test",
            "test",
            "source statement",
            "True",
            null,
            "False",
            null,
            optionalData);

        return ParsedRule.SuccessResult(definition, node, "parsed", false)!;
    }

    private static ConditionNode Condition(string operatorName, string left, string right) =>
        new()
        {
            Operator = operatorName,
            Left = new Operand(OperandKindEnum.STRING, left),
            Right = new Operand(OperandKindEnum.STRING, right)
        };

    private sealed class DelegateOperator(
        Func<object, object, object?, OperatorResult> process) : Operator
    {
        public override List<string> Aliases => ["check"];

        public int CallCount { get; private set; }
        public object? LastLeft { get; private set; }
        public object? LastRight { get; private set; }
        public object? LastOptionalData { get; private set; }

        protected override Task<OperatorResult> ExecuteAsync(
            object Left,
            object Right,
            object? OptionalData)
        {
            CallCount++;
            LastLeft = Left;
            LastRight = Right;
            LastOptionalData = OptionalData;
            return Task.FromResult(process(Left, Right, OptionalData));
        }
    }

    private sealed class RecordingStatementBuilder : IStatementBuilder
    {
        public int CallCount { get; private set; }

        public Task<string> BuildAppliedStatement(Node? Node, string Statement)
        {
            CallCount++;
            return Task.FromResult("applied statement");
        }
    }

    private sealed class ThrowingStatementBuilder(string message) : IStatementBuilder
    {
        public Task<string> BuildAppliedStatement(Node? Node, string Statement) =>
            Task.FromException<string>(new InvalidOperationException(message));
    }
}