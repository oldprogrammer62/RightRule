using RightRule.LanguageProcessor.Evaluator;
using RightRule.LanguageProcessor.Interpreter;
using RightRule.RuleServices.DefaultRuleRegistration;
using RightRule.RuleServices.RuleModels;
using RightRule.RuleServices.Rules;
using Xunit;

namespace RightRuleTests;

public sealed class InterpreterTests
{
    private const string Context = "orders";

    [Fact]
    public async Task EvaluateFirstAsync_ReturnsDefaultRule_WhenThereAreNoRegisteredRules()
    {
        var defaultRule = DefaultRule.Build("default", Context, "fallback", null);
        var defaultRegister = new FakeDefaultRuleRegister(defaultRule);
        var evaluator = new FakeEvaluator();
        var interpreter = new Interpreter(evaluator, defaultRegister);

        var result = await interpreter.EvaluateFirstAsync(new RegisteredRules(), Context);

        Assert.Same(defaultRule, result);
        Assert.NotEmpty(defaultRegister.RequestedContexts);
        Assert.All(defaultRegister.RequestedContexts, requestedContext => Assert.Equal(Context, requestedContext));
        Assert.Equal(0, evaluator.CallCount);
    }

    [Fact]
    public async Task EvaluateAnyAsync_ReturnsDefaultRule_WhenThereAreNoRegisteredRules()
    {
        var defaultRule = DefaultRule.Build("default", Context, "fallback", null);
        var defaultRegister = new FakeDefaultRuleRegister(defaultRule);
        var evaluator = new FakeEvaluator();
        var interpreter = new Interpreter(evaluator, defaultRegister);

        var results = await interpreter.EvaluateAnyAsync(new RegisteredRules(), Context, default);

        var result = Assert.Single(results);
        Assert.Same(defaultRule, result);
        Assert.Equal(Context, Assert.Single(defaultRegister.RequestedContexts));
        Assert.Equal(0, evaluator.CallCount);
    }

    [Fact]
    public async Task EvaluateAllAsync_ReturnsDefaultRule_WhenThereAreNoRegisteredRules()
    {
        var defaultRule = DefaultRule.Build("default", Context, "fallback", null);
        var defaultRegister = new FakeDefaultRuleRegister(defaultRule);
        var evaluator = new FakeEvaluator();
        var interpreter = new Interpreter(evaluator, defaultRegister);

        var results = await interpreter.EvaluateAllAsync(new RegisteredRules(), Context, default);

        var result = Assert.Single(results);
        Assert.Same(defaultRule, result);
        Assert.Equal(Context, Assert.Single(defaultRegister.RequestedContexts));
        Assert.Equal(0, evaluator.CallCount);
    }

    private sealed class FakeEvaluator : IEvaluator
    {
        public int CallCount { get; private set; }

        public Task<EvaluationResult> EvaluateAsync(ParsedRule rule)
        {
            CallCount++;
            return Task.FromException<EvaluationResult>(
                new InvalidOperationException("The evaluator should not be called when there are no rules."));
        }
    }

    private sealed class FakeDefaultRuleRegister(DefaultRule defaultRule) : IDefaultRuleRegister
    {
        public List<string> RequestedContexts { get; } = [];

        public List<DefaultRulePerContext>? GetDefaultRules() => [];

        public bool SetDefaultRule(DefaultRule rule) => true;

        public DefaultRule GetDefaultRule(string context)
        {
            RequestedContexts.Add(context);
            return defaultRule;
        }
    }
}