using Microsoft.Extensions.Logging.Abstractions;
using RightRule.Models;
using RightRule.RuleServices;
using RightRule.RuleServices.RuleModels;
using RightRule.RuleServices.RuleParsingService;
using RightRule.RuleServices.RuleRegistration;
using RightRule.RuleServices.Rules;
using RightRule.RuleServices.RuleValidation;
using System.Reflection;

namespace RightRuleTests;

public sealed class RuleServiceTests
{
    private readonly RegisteredRules _registeredRules = new();
    private readonly StubValidationService _validationService = new();
    private readonly StubParseRuleService _parseService = new();
    private readonly RuleService _sut;

    public RuleServiceTests()
    {
        _sut = new RuleService(
            new RightRuleOptions(),
            _validationService,
            _parseService,
            _registeredRules,
            NullLogger<RuleService>.Instance);
    }

    [Fact]
    public void RegisteredRules_ReturnsTheInjectedInstance()
    {
        Assert.Same(_registeredRules, _sut.RegisteredRules);
    }

    [Fact]
    public void GetRules_ReturnsRegisteredRules()
    {
        var rule = CreateParsedRule(1, "First", "Sales", RightRuleStatusEnum.SUCCESS);
        _registeredRules.AddRule(rule);

        Assert.Same(_registeredRules.Rules, _sut.GetRules());
    }

    [Fact]
    public void GetRule_ByName_MatchesContextAndNameCaseInsensitively()
    {
        var rule = CreateParsedRule(1, "FirstRule", "Sales", RightRuleStatusEnum.SUCCESS);
        _registeredRules.AddRule(rule);

        Assert.Same(rule, _sut.GetRule("sAlEs", "fIrStRuLe"));
    }

    [Fact]
    public void GetRule_ByNumber_MatchesContextCaseInsensitively()
    {
        var rule = CreateParsedRule(42, "FirstRule", "Sales", RightRuleStatusEnum.SUCCESS);
        _registeredRules.AddRule(rule);

        Assert.Same(rule, _sut.GetRule("SALES", 42));
    }

    [Fact]
    public void GetRule_ByName_ThrowsWhenNameIsNull()
    {
        var exception = Assert.Throws<ArgumentNullException>(
            () => _sut.GetRule("Sales", (string)null!));

        Assert.Equal("RuleName", exception.ParamName);
    }

    [Fact]
    public void GetRules_ByContext_FiltersCaseInsensitively()
    {
        var salesRule = CreateParsedRule(1, "SalesRule", "Sales", RightRuleStatusEnum.SUCCESS);
        _registeredRules.AddRule(salesRule);
        _registeredRules.AddRule(CreateParsedRule(2, "SupportRule", "Support", RightRuleStatusEnum.SUCCESS));

        Assert.Equal([salesRule], _sut.GetRules("sAlEs"));
    }

    [Fact]
    public void RegisterRule_WhenValidationReturnsError_DoesNotParseRule()
    {
        _validationService.Result = new RuleValidationResult
        {
            Valid = RightRuleStatusEnum.ERROR,
            Message = "Invalid rule"
        };

        var result = _sut.RegisterRule(CreateDefinedRule());

        Assert.Equal(RightRuleStatusEnum.ERROR, result.Status);
        Assert.Null(result.RegisteredRule);
        Assert.Equal("Invalid rule", result.Message);
        Assert.Equal(0, _parseService.CallCount);
    }

    [Fact]
    public void RegisterRule_WhenParsingReturnsNull_ReturnsError()
    {
        _parseService.Result = null;

        var result = _sut.RegisterRule(CreateDefinedRule());

        Assert.Equal(RightRuleStatusEnum.ERROR, result.Status);
        Assert.Null(result.RegisteredRule);
        Assert.Equal("Error: Rule Failed to be Parsed", result.Message);
    }

    [Theory]
    [InlineData(RightRuleStatusEnum.FAILURE, "Parsing failed", "Parsing failed")]
    [InlineData(RightRuleStatusEnum.FAILURE, null, "Registration failed, reason unknown!")]
    [InlineData(RightRuleStatusEnum.ERROR, "Parse error", "Parse error")]
    [InlineData(RightRuleStatusEnum.ERROR, null, "Error: Registration failed, reason unknown!")]
    public void RegisterRule_WhenParsedRuleIsFailureOrError_ReturnsItsStatusAndMessage(
        RightRuleStatusEnum status,
        string? parsedMessage,
        string expectedMessage)
    {
        var parsedRule = CreateParsedRule(1, "Rule", "Sales", status, parsedMessage);
        _parseService.Result = parsedRule;

        var result = _sut.RegisterRule(CreateDefinedRule());

        Assert.Equal(status, result.Status);
        Assert.Same(parsedRule, result.RegisteredRule);
        Assert.Equal(expectedMessage, result.Message);
        Assert.Empty(_registeredRules.Rules);
    }

    [Theory]
    [InlineData(RightRuleStatusEnum.SUCCESS)]
    [InlineData(RightRuleStatusEnum.DEFAULT)]
    public void RegisterRule_WhenParsedRuleIsSuccessOrDefault_AddsItToRegisteredRules(
        RightRuleStatusEnum status)
    {
        var parsedRule = CreateParsedRule(1, "Rule", "Sales", status);
        _parseService.Result = parsedRule;

        var result = _sut.RegisterRule(CreateDefinedRule());

        Assert.Equal(status, result.Status);
        Assert.Same(parsedRule, result.RegisteredRule);
        Assert.Contains(parsedRule, _registeredRules.Rules);
    }

    [Fact]
    public void RegisterRules_ReturnsOnlySuccessfulRegistrations()
    {
        var successfulRule = CreateParsedRule(1, "Success", "Sales", RightRuleStatusEnum.SUCCESS);
        var failedRule = CreateParsedRule(2, "Failure", "Sales", RightRuleStatusEnum.FAILURE);
        _parseService.Results = new Queue<ParsedRule?>(
        [
            successfulRule,
            failedRule
        ]);

        var results = _sut.RegisterRules(
        [
            CreateDefinedRule(1, "Success"),
            CreateDefinedRule(2, "Failure")
        ]);

        Assert.Single(results);
        Assert.Same(successfulRule, results[0].RegisteredRule);
        Assert.Equal(2, _parseService.CallCount);
    }

    [Fact]
    public void RegisterRules_ThrowsWhenRulesIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => _sut.RegisterRules(null!));
    }

    [Fact]
    public void RegisterRule_ThrowsWhenRuleIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => _sut.RegisterRule(null!));
    }

    [Fact]
    public void DeRegisterRules_ByContext_RejectsNullOrWhitespace()
    {
        Assert.Throws<ArgumentNullException>(() => _sut.DeRegisterRules(null!));
        Assert.Throws<ArgumentException>(() => _sut.DeRegisterRules(""));
        Assert.Throws<ArgumentException>(() => _sut.DeRegisterRules("   "));
    }

    [Fact]
    public void DeRegisterMethods_RemoveRulesByContextNameAndNumber()
    {
        var salesByName = CreateParsedRule(1, "First", "Sales", RightRuleStatusEnum.SUCCESS);
        var salesByNumber = CreateParsedRule(2, "Second", "Sales", RightRuleStatusEnum.SUCCESS);
        var supportRule = CreateParsedRule(3, "Third", "Support", RightRuleStatusEnum.SUCCESS);
        _registeredRules.AddRule(salesByName);
        _registeredRules.AddRule(salesByNumber);
        _registeredRules.AddRule(supportRule);

        Assert.True(_sut.DeRegisterRule("Sales", "FIRST"));
        Assert.True(_sut.DeRegisterRule("Sales", 2));
        Assert.True(_sut.DeRegisterRules("support"));
        Assert.Empty(_registeredRules.Rules);
    }

    [Fact]
    public void DeRegisterRules_WithoutContext_RemovesAllRules()
    {
        _registeredRules.AddRule(CreateParsedRule(1, "First", "Sales", RightRuleStatusEnum.SUCCESS));

        Assert.True(_sut.DeRegisterRules());
        Assert.Empty(_registeredRules.Rules);
    }

    [Fact]
    public void RuleMetadataMethods_ReturnRegisteredRuleMetadata()
    {
        _registeredRules.AddRule(CreateParsedRule(2, "Second", "Sales", RightRuleStatusEnum.SUCCESS));
        _registeredRules.AddRule(CreateParsedRule(1, "First", "Sales", RightRuleStatusEnum.SUCCESS));

        Assert.Equal(["Sales"], _sut.GetRuleContexts());
        Assert.Equal([(2, "Second"), (1, "First")], _sut.GetRuleNumbers("Sales"));
        Assert.Equal(["First", "Second"], _sut.GetRuleNames("Sales"));
    }

    private static DefinedRule CreateDefinedRule(int number = 1, string name = "Rule") =>
        DefinedRule.Build(number, name, "Sales", "true", "Yes", null, "No", null, null);

    private static ParsedRule CreateParsedRule(
        int number,
        string name,
        string context,
        RightRuleStatusEnum status,
        string? message = null)
    {
        var rule = (ParsedRule)Activator.CreateInstance(typeof(ParsedRule), nonPublic: true)!;

        SetProperty(rule, nameof(ParsedRule.Number), number);
        SetProperty(rule, nameof(ParsedRule.Name), name);
        SetProperty(rule, nameof(ParsedRule.Context), context);
        SetProperty(rule, nameof(ParsedRule.Statement), "true");
        SetProperty(rule, nameof(ParsedRule.TrueDecision), "Yes");
        SetProperty(rule, nameof(ParsedRule.TrueDecisionData), null);
        SetProperty(rule, nameof(ParsedRule.FalseDecision), "No");
        SetProperty(rule, nameof(ParsedRule.FalseDecisionData), null);
        SetProperty(rule, nameof(ParsedRule.Status), status);
        SetProperty(rule, nameof(ParsedRule.Node), null);
        SetProperty(rule, nameof(ParsedRule.StatementPath), string.Empty);
        SetProperty(rule, nameof(ParsedRule.Message), message);
        SetProperty(rule, nameof(ParsedRule.OptionalData), null);

        return rule;
    }

    private static void SetProperty(ParsedRule rule, string propertyName, object? value) =>
        typeof(ParsedRule).GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Public)!
            .SetValue(rule, value);

    private sealed class StubValidationService : IRuleValidationService
    {
        public RuleValidationResult Result { get; set; } = new()
        {
            Valid = RightRuleStatusEnum.SUCCESS,
            Message = "Valid"
        };

        public RuleValidationResult ValidateRule(DefinedRule rule) => Result;
    }

    private sealed class StubParseRuleService : IParseRuleService
    {
        public ParsedRule? Result { get; set; } =
            CreateParsedRule(1, "Rule", "Sales", RightRuleStatusEnum.SUCCESS);

        public Queue<ParsedRule?>? Results { get; set; }
        public int CallCount { get; private set; }

        public ParsedRule? ParseRule(DefinedRule rule, bool diagnostics)
        {
            CallCount++;

            if (Results is { Count: > 0 })
            {
                return Results.Dequeue();
            }

            return Result;
        }
    }
}