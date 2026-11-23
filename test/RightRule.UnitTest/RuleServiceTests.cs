using Microsoft.Extensions.Logging.Abstractions;
using RightRule;
using RightRule.DataServices;
using RightRule.DataTransformer;
using RightRule.LanguageProcessor.Evaluator;
using RightRule.Models;
using RightRule.RuleServices;
using RightRule.RuleServices.DefaultRuleRegistration;
using RightRule.RuleServices.RuleApplication;
using RightRule.RuleServices.RuleLoader;
using RightRule.RuleServices.RuleModels;
using RightRule.RuleServices.RuleModels.BaseModels;
using RightRule.RuleServices.RuleParsingService;
using RightRule.RuleServices.RuleRegistration;
using RightRule.RuleServices.Rules;
using RightRule.RuleServices.RuleSerializer;
using RightRule.RuleServices.RuleValidation;
using System.Reflection;

namespace RightRuleTests;

public sealed class RuleServiceTests
{
    private readonly RegisteredRules _registeredRules = new();
    private readonly StubValidationService _validationService = new();
    private readonly StubParseRuleService _parseService = new();
    private readonly RuleService _sut;

    // Add inside RuleServiceTests:
    private sealed class StubRuleLoaderService : IRuleLoaderService
    {
        public List<DefinedRule>? LoadRules(string JsonData) => [];
    }

    public RuleServiceTests()
    {
        _sut = new RuleService(
            new RightRuleOptions(),
            new StubRuleApplicationService(),
            _validationService,
            _parseService,
            _registeredRules,
            new StubRuleLoaderService(),
            new RuleSerializerService(),
            new DefaultRuleRegister(NullLogger<DefaultRuleRegister>.Instance),
            new StubInterpolatorService(),
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
        var rule = CreateParsedRule(1, "First", "Sales", RightRuleResultEnum.SUCCESS);
        _registeredRules.AddRule(rule);

        Assert.Same(_registeredRules.Rules, _sut.GetRegisteredRules());
    }

    [Fact]
    public void GetRule_ByName_MatchesContextAndNameCaseInsensitively()
    {
        var rule = CreateParsedRule(1, "FirstRule", "Sales", RightRuleResultEnum.SUCCESS);
        _registeredRules.AddRule(rule);

        Assert.Same(rule, _sut.GetRegisteredRule("sAlEs", "fIrStRuLe"));
    }

    [Fact]
    public void GetRule_ByNumber_MatchesContextCaseInsensitively()
    {
        var rule = CreateParsedRule(42, "FirstRule", "Sales", RightRuleResultEnum.SUCCESS);
        _registeredRules.AddRule(rule);

        Assert.Same(rule, _sut.GetRegisteredRule("SALES", 42));
    }

    [Fact]
    public void GetRule_ByName_ThrowsWhenNameIsNull()
    {
        var exception = Assert.Throws<ArgumentNullException>(
            () => _sut.GetRegisteredRule("Sales", (string)null!));

        Assert.Equal("RuleName", exception.ParamName);
    }

    [Fact]
    public void GetRules_ByContext_FiltersCaseInsensitively()
    {
        var salesRule = CreateParsedRule(1, "SalesRule", "Sales", RightRuleResultEnum.SUCCESS);
        _registeredRules.AddRule(salesRule);
        _registeredRules.AddRule(CreateParsedRule(2, "SupportRule", "Support", RightRuleResultEnum.SUCCESS));

        Assert.Equal([salesRule], _sut.GetRegisteredRules("sAlEs"));
    }

    [Fact]
    public void RegisterRule_WhenValidationReturnsError_DoesNotParseRule()
    {
        _validationService.Result = new RuleValidationResult
        {
            Valid = RightRuleResultEnum.ERROR,
            Message = "Invalid rule"
        };

        var result = _sut.RegisterRule(CreateDefinedRule());

        Assert.Equal(RegistrationStatusEnum.ERROR, result.Status);
        Assert.Null(result.RegisteredRule);
        Assert.Equal("Invalid rule", result.Message);
        Assert.Equal(0, _parseService.CallCount);
    }

    [Fact]
    public void RegisterRule_WhenParsingReturnsNull_ReturnsError()
    {
        _parseService.Result = null;

        var result = _sut.RegisterRule(CreateDefinedRule());

        Assert.Equal(RegistrationStatusEnum.ERROR, result.Status);
        Assert.Null(result.RegisteredRule);
        Assert.Equal("Error: Rule Failed to be Parsed", result.Message);
    }

    [Theory]
    [InlineData(RightRuleResultEnum.FAILURE, "Parsing failed", "Parsing failed")]
    [InlineData(RightRuleResultEnum.FAILURE, null, "Registration failed, reason unknown!")]
    [InlineData(RightRuleResultEnum.ERROR, "Parse error", "Parse error")]
    [InlineData(RightRuleResultEnum.ERROR, null, "Error: Registration failed, reason unknown!")]
    public void RegisterRule_WhenParsedRuleIsFailureOrError_ReturnsItsStatusAndMessage(
        RightRuleResultEnum status,
        string? parsedMessage,
        string expectedMessage)
    {
        var parsedRule = CreateParsedRule(1, "Rule", "Sales", status, parsedMessage);
        _parseService.Result = parsedRule;

        var result = _sut.RegisterRule(CreateDefinedRule());

        Assert.Equal(status.ParserToRegistrationStatus(), result.Status);
        Assert.Same(parsedRule, result.RegisteredRule);
        Assert.Equal(expectedMessage, result.Message);
        Assert.Empty(_registeredRules.Rules);
    }

    [Theory]
    [InlineData(RightRuleResultEnum.SUCCESS)]
    [InlineData(RightRuleResultEnum.DEFAULT)]
    public void RegisterRule_WhenParsedRuleIsSuccessOrDefault_AddsItToRegisteredRules(
        RightRuleResultEnum status)
    {
        var parsedRule = CreateParsedRule(1, "Rule", "Sales", status);
        _parseService.Result = parsedRule;

        var result = _sut.RegisterRule(CreateDefinedRule());

        Assert.Equal(status.ParserToRegistrationStatus(), result.Status);
        Assert.Same(parsedRule, result.RegisteredRule);
        Assert.Contains(parsedRule, _registeredRules.Rules);
    }

    [Fact]
    public void RegisterRules_ReturnsAllRegistrationsSuccessAndFailures()
    {
        var successfulRule = CreateParsedRule(1, "Success", "Sales", RightRuleResultEnum.SUCCESS);
        var failedRule = CreateParsedRule(2, "Failure", "Sales", RightRuleResultEnum.FAILURE);
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

        Assert.Equal(2, results.Count);
        Assert.Same(successfulRule, results[0].RegisteredRule);
        Assert.Same(failedRule, results[1].RegisteredRule);

        Assert.Equal(2, _parseService.CallCount);
    }

    [Fact]
    public void RegisterRules_ReturnsAllRegistrationsApplyOnlySuccess()
    {
        var rr = new RightRule.RightRule().CreateSession();
        Assert.NotNull(rr);

        var definedRule1 = new DefinedRule
        {
            Number = 1,
            Context = "Sales",
            Name = "Rule1",
            Statement = "a.b = c.d",
            Decision = "DONE",
        };
        var definedRule2 = new DefinedRule
        {
            Number = 1,
            Context = "Sales",
            Name = "Rule2",
            Statement = "a.b = c.d",
            Decision = "DONE",
        };
        var ruleRegistrations = rr.RuleService.RegisterRules(
        [
            definedRule1,
            definedRule2
        ]);

        Assert.NotNull(ruleRegistrations);
        Assert.Equal(2, ruleRegistrations.Count);
        Assert.Equal(RegistrationStatusEnum.SUCCESS, ruleRegistrations[0].Status);
        Assert.Equal(RegistrationStatusEnum.FAILURE, ruleRegistrations[1].Status);

        var outcome = rr.RuleService.ApplyRules("Sales");
        Assert.NotNull(outcome);
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
        var salesByName = CreateParsedRule(1, "First", "Sales", RightRuleResultEnum.SUCCESS);
        var salesByNumber = CreateParsedRule(2, "Second", "Sales", RightRuleResultEnum.SUCCESS);
        var supportRule = CreateParsedRule(3, "Third", "Support", RightRuleResultEnum.SUCCESS);
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
        _registeredRules.AddRule(CreateParsedRule(1, "First", "Sales", RightRuleResultEnum.SUCCESS));

        Assert.True(_sut.DeRegisterRules());
        Assert.Empty(_registeredRules.Rules);
    }

    [Fact]
    public void RuleMetadataMethods_ReturnRegisteredRuleMetadata()
    {
        _registeredRules.AddRule(CreateParsedRule(2, "Second", "Sales", RightRuleResultEnum.SUCCESS));
        _registeredRules.AddRule(CreateParsedRule(1, "First", "Sales", RightRuleResultEnum.SUCCESS));

        Assert.Equal(["Sales"], _sut.GetRuleContexts());
        Assert.Equal([(2, "Second"), (1, "First")], _sut.GetRuleNumbers("Sales"));
        Assert.Equal(["First", "Second"], _sut.GetRuleNames("Sales"));
    }

    [Fact]
    public void RegisterRules_WhenValidationReturnsError_ReturnsErrorForEachRuleWithoutParsing()
    {
        _validationService.Result = new RuleValidationResult
        {
            Valid = RightRuleResultEnum.ERROR,
            Message = "Invalid rule"
        };

        var results = _sut.RegisterRules(
        [
            CreateDefinedRule(1, "First"),
        CreateDefinedRule(2, "Second")
        ]);

        Assert.Equal(2, results.Count);
        Assert.All(results, result =>
        {
            Assert.Equal(RegistrationStatusEnum.ERROR, result.Status);
            Assert.Null(result.RegisteredRule);
            Assert.Equal("Invalid rule", result.Message);
        });
        Assert.Equal(0, _parseService.CallCount);
        Assert.Empty(_registeredRules.Rules);
    }

    [Fact]
    public void RegisterRules_WhenParsedRuleIsError_ReportsErrorAndContinuesRegistering()
    {
        var errorRule = CreateParsedRule(
            1, "ErrorRule", "Sales", RightRuleResultEnum.ERROR, "Parse error");
        var successfulRule = CreateParsedRule(
            2, "SuccessfulRule", "Sales", RightRuleResultEnum.SUCCESS);

        var parseResults = new Queue<ParsedRule?>();
        parseResults.Enqueue(errorRule);
        parseResults.Enqueue(successfulRule);
        _parseService.Results = parseResults;

        var results = _sut.RegisterRules(
        [
            CreateDefinedRule(1, "ErrorRule"),
        CreateDefinedRule(2, "SuccessfulRule")
        ]);

        Assert.Equal(2, results.Count);
        Assert.Equal(RegistrationStatusEnum.ERROR, results[0].Status);
        Assert.Same(errorRule, results[0].RegisteredRule);
        Assert.Equal("Parse error", results[0].Message);

        Assert.Equal(RegistrationStatusEnum.SUCCESS, results[1].Status);
        Assert.Same(successfulRule, results[1].RegisteredRule);
        Assert.Contains(successfulRule, _registeredRules.Rules);
        Assert.DoesNotContain(errorRule, _registeredRules.Rules);
        Assert.Equal(2, _parseService.CallCount);
    }

    [Fact]
    public void RegisterRule_WhenValidationReturnsError_PreservesExistingRegisteredRules()
    {
        var existingRule = CreateParsedRule(
            99, "ExistingRule", "Sales", RightRuleResultEnum.SUCCESS);
        _registeredRules.AddRule(existingRule);

        _validationService.Result = new RuleValidationResult
        {
            Valid = RightRuleResultEnum.ERROR,
            Message = "Invalid rule"
        };

        var result = _sut.RegisterRule(CreateDefinedRule());

        Assert.Equal(RegistrationStatusEnum.ERROR, result.Status);
        Assert.Null(result.RegisteredRule);
        Assert.Equal("Invalid rule", result.Message);
        Assert.Equal(0, _parseService.CallCount);
        Assert.Equal([existingRule], _registeredRules.Rules);
    }

    [Fact]
    public void RegisterRule_WhenParsedRuleReturnsError_PreservesExistingRegisteredRules()
    {
        var existingRule = CreateParsedRule(
            99, "ExistingRule", "Sales", RightRuleResultEnum.SUCCESS);
        var errorRule = CreateParsedRule(
            1, "ErrorRule", "Sales", RightRuleResultEnum.ERROR, "Parse error");
        _registeredRules.AddRule(existingRule);
        _parseService.Result = errorRule;

        var result = _sut.RegisterRule(CreateDefinedRule(1, "ErrorRule"));

        Assert.Equal(RegistrationStatusEnum.ERROR, result.Status);
        Assert.Same(errorRule, result.RegisteredRule);
        Assert.Equal("Parse error", result.Message);
        Assert.Equal([existingRule], _registeredRules.Rules);
    }

    public static TheoryData<RightRuleResultEnum> AllRightRuleStatuses
    {
        get
        {
            var data = new TheoryData<RightRuleResultEnum>();
            foreach (var status in Enum.GetValues<RightRuleResultEnum>())
            {
                data.Add(status);
            }

            return data;
        }
    }

    [Theory]
    [MemberData(nameof(AllRightRuleStatuses))]
    public void RegisterRule_HandlesEveryParsedRuleStatus(RightRuleResultEnum status)
    {
        var parsedRule = CreateParsedRule(
            1, "Rule", "Sales", status, "Status result");
        _parseService.Result = parsedRule;

        var result = _sut.RegisterRule(CreateDefinedRule());

        switch (status)
        {
            case RightRuleResultEnum.SUCCESS:
            case RightRuleResultEnum.DEFAULT:
                Assert.Equal(status.ParserToRegistrationStatus(), result.Status);
                Assert.Same(parsedRule, result.RegisteredRule);
                Assert.Contains(parsedRule, _registeredRules.Rules);
                break;

            case RightRuleResultEnum.FAILURE:
            case RightRuleResultEnum.ERROR:
                Assert.Equal(status.ParserToRegistrationStatus(), result.Status);
                Assert.Same(parsedRule, result.RegisteredRule);
                Assert.Equal("Status result", result.Message);
                Assert.DoesNotContain(parsedRule, _registeredRules.Rules);
                break;

            default:
                Assert.Equal(RegistrationStatusEnum.FAILURE, result.Status);
                Assert.Null(result.RegisteredRule);
                Assert.Equal("Rule Failed to register", result.Message);
                Assert.Empty(_registeredRules.Rules);
                break;
        }
    }

    private static DefinedRule CreateDefinedRule(int number = 1, string name = "Rule") =>
        DefinedRule.Build(number, name, "Sales", "true", "Yes", null, null);

    private static ParsedRule CreateParsedRule(
        int number,
        string name,
        string context,
        RightRuleResultEnum status,
        string? message = null)
    {
        var rule = (ParsedRule)Activator.CreateInstance(typeof(ParsedRule), nonPublic: true)!;

        SetProperty(rule, nameof(ParsedRule.Number), number);
        SetProperty(rule, nameof(ParsedRule.Name), name);
        SetProperty(rule, nameof(ParsedRule.Context), context);
        SetProperty(rule, nameof(ParsedRule.Statement), "true");
        SetProperty(rule, nameof(ParsedRule.Decision), "Yes");
        SetProperty(rule, nameof(ParsedRule.DecisionData), null);
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
            Valid = RightRuleResultEnum.SUCCESS,
            Message = "Valid"
        };

        public RuleValidationResult ValidateRule(DefinedRule rule) => Result;

        public bool ValidateRules(RuleService ruleService, List<DefinedRule> rules)
        {
            foreach (var rule in rules)
            {
                var validationResult = ValidateRule(rule);
                if (validationResult.Valid != RightRuleResultEnum.SUCCESS)
                {
                    return false;
                }
            }
            return true;
        }
    }

    private sealed class StubParseRuleService : IParseRuleService
    {
        public ParsedRule? Result { get; set; } =
            CreateParsedRule(1, "Rule", "Sales", RightRuleResultEnum.SUCCESS);

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

    private sealed class StubRuleApplicationService : IRuleApplicationService
    {
        public Task<List<IProcessedRule>> ApplyRulesAsync(
            string Context,
            EvaluationTypeEnum evaluationType) =>
            Task.FromResult<List<IProcessedRule>>([]);
    }

    private sealed class StubInterpolatorService : IInterpolatorService
    {
        public object? Interpolate(object? DataToInterpolate) => DataToInterpolate;
    }
}