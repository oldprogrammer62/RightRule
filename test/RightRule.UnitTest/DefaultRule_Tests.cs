using RightRule;
using RightRule.LanguageProcessor.Evaluator;
using RightRule.Models;
using RightRule.RuleServices.RuleModels;

namespace RightRuleTests;

public class DefaultRule_Tests
{
    [Fact]
    public void DefaultRule1_SingleMatch_NoResult()
    {
        var rr = new RightRule.RightRule().CreateSession();

        var ruleDef1 = rr.RuleService.BuildRule()
            .Number(1)
            .Name("Rule1")
            .Context("Test")
            .Statement("11 >= 19")
            .Decision("true")
            .Build();
        var rule1 = rr.RuleService.RegisterRule(ruleDef1);
        var result1 = rr.RuleService.ApplyRule("Test");
        Assert.NotNull(result1);
    }

    [Fact]
    public void DefaultRule2()
    {
        var rr = new RightRule.RightRule().CreateSession();

        var ruleDef1 = rr.RuleService.BuildRule()
            .Number(1)
            .Name("Rule1")
            .Context("Test")
            .Statement("11 >= 19")
            .Decision("true")
            .Build();
        var rule1 = rr.RuleService.RegisterRule(ruleDef1);
        var result1 = rr.RuleService.ApplyRules("Test", EvaluationTypeEnum.ANY);
        Assert.NotNull(result1);
    }

    [Fact]
    public void RegisterNewDefaultRule()
    {
        var rr = new RightRule.RightRule().CreateSession();

        string context = "Hello";

        var defaultRule1 = rr.RuleService.GetDefaultRule(context);
        Assert.NotNull(defaultRule1);

        rr.DataService.RegisterData("x", 100);

        var newDefaultRule = new DefaultRule()
        {
            Name = "DefaultRule1",
            Context = context,
            Decision = "dec",
            DecisionData = "DefaultDecision $x"
        };  
        var result = rr.RuleService.RegisterDefaultRule(newDefaultRule);
        Assert.Equal(RightRuleStatusEnum.SUCCESS, result.Status);

        var defaultRule2 = rr.RuleService.GetDefaultRule(context);
        Assert.NotNull(defaultRule2);
    }

    [Fact]
    public void RegisterNewDefaultRuleAndUseIt()
    {
        var rr = new RightRule.RightRule().CreateSession();

        string context = "Hello";

        var defaultRule1 = rr.RuleService.GetDefaultRule(context);
        Assert.NotNull(defaultRule1);

        rr.DataService.RegisterData("x", 100);

        var newDefaultRule = new DefaultRule()
        {
            Name = "HOWDYDOODY",
            Context = context,
            Decision = "GOTITNOW",
            DecisionData = "HELLO $x WORLD"
        };
        var result = rr.RuleService.RegisterDefaultRule(newDefaultRule);
        Assert.Equal(RightRuleStatusEnum.SUCCESS, result.Status);

        var defaultRule2 = rr.RuleService.GetDefaultRule(context);
        Assert.NotNull(defaultRule2);

        var ruleDef1 = rr.RuleService.BuildRule()
            .Number(1)
            .Name("Rule1")
            .Context(context)
            .Statement("11 >= 19")
            .Decision("true")
            .Build();
        var rule1 = rr.RuleService.RegisterRule(ruleDef1);
        var result1 = rr.RuleService.ApplyRule(context);
        Assert.NotNull(result1);
    }

    [Fact]
    public void RegisterNewDefaultMoreThanOnce()
    {
        var rr = new RightRule.RightRule().CreateSession();

        string context1 = "Hello";
        string context2 = "world";

        var originalDefaultRule = rr.RuleService.GetDefaultRule(context1);
        Assert.NotNull(originalDefaultRule);

        var newDefaultRule_First = new DefaultRule()
        {
            Name = "DefaultRuleContext_First",
            Context = context1,
            Decision = "DecisionContext_First",
            DecisionData = "DefaultDecision"
        };
        var firstResult = rr.RuleService.RegisterDefaultRule(newDefaultRule_First);
        Assert.Equal(RightRuleStatusEnum.SUCCESS, firstResult.Status);
        var foundDefaultRule_First = rr.RuleService.GetDefaultRule(context1);
        Assert.NotNull(foundDefaultRule_First);
        Assert.Equal(foundDefaultRule_First.Name, newDefaultRule_First.Name);

        var newDefaultRule_Second = new DefaultRule()
        {
            Name = "DefaultRuleContext_Second",
            Context = context1,
            Decision = "DecisionContext_Second",
            DecisionData = "DefaultDecision"
        };
        var secondResult = rr.RuleService.RegisterDefaultRule(newDefaultRule_Second);
        Assert.Equal(RightRuleStatusEnum.SUCCESS, secondResult.Status);
        var foundDefaultRule_Second = rr.RuleService.GetDefaultRule(context1);
        Assert.NotNull(foundDefaultRule_Second);
        Assert.Equal(foundDefaultRule_Second.Name, newDefaultRule_Second.Name);

        var newDefaultRule_Third = new DefaultRule()
        {
            Name = "DefaultRuleContext_Third",
            Context = context2,
            Decision = "DecisionContext_Third",
            DecisionData = "DefaultDecision"
        };
        var thirdResult = rr.RuleService.RegisterDefaultRule(newDefaultRule_Third);
        Assert.Equal(RightRuleStatusEnum.SUCCESS, thirdResult.Status);
        var foundDefaultRule_Third = rr.RuleService.GetDefaultRule(context2);
        Assert.NotNull(foundDefaultRule_Third);
        Assert.Equal(foundDefaultRule_Third.Name, newDefaultRule_Third.Name);

        var DefaultRuleContext1 = rr.RuleService.GetDefaultRule(context1);
        Assert.NotNull(DefaultRuleContext1);
        Assert.Equal(DefaultRuleContext1.Context, context1);

        var DefaultRuleContext2 = rr.RuleService.GetDefaultRule(context2);
        Assert.NotNull(DefaultRuleContext2);
        Assert.Equal(DefaultRuleContext2.Context, context2);

        var DefaultRuleContext3 = rr.RuleService.GetDefaultRule("NotFound");
        Assert.NotNull(DefaultRuleContext3);
        Assert.Equal(DefaultRuleContext3.Context, string.Empty);
    }

    [Fact]
    public void DefaultRuleBuilder_Test()
    {
        var rr = new RightRule.RightRule().CreateSession();

        string Context = "ContextFromBuilder";

        var defaultRuleDefn = rr.RuleService.BuildDefaultRule()
            .Name("DefaultName")
            .Context(Context)
            .Decision("WhatEver")
            .Build();

        var defRuleRegistered = rr.RuleService.RegisterDefaultRule(defaultRuleDefn);
        var defaultRule = rr.RuleService.GetDefaultRule(Context);
        Assert.NotNull(defaultRule);
    }

    [Fact]
    public void ApplyRule_WhenRegisteredRuleEvaluatesFalse_ReturnsConfiguredDefault()
    {
        using var session = new RightRule.RightRule().CreateSession();
        const string context = "Sales";

        var defaultRule = new DefaultRule
        {
            Name = "SalesDefault",
            Context = context,
            Decision = "NoMatch",
            DecisionData = "Use the default decision"
        };
        var defaultRegistration = session.RuleService.RegisterDefaultRule(defaultRule);

        var dataRegistration = session.DataService.RegisterData(
            "Orders",
            new Dictionary<string, object>
            {
                ["count"] = 3
            });

        var rule = session.RuleService.BuildRule()
            .Number(1)
            .Name("MinimumOrderCount")
            .Context(context)
            .Statement("Orders.count equals 5")
            .Decision("Eligible")
            .Build();
        var ruleRegistration = session.RuleService.RegisterRule(rule);

        var result = session.RuleService.ApplyRule(context);

        Assert.Equal(RightRuleStatusEnum.SUCCESS, defaultRegistration.Status);
        Assert.Equal(RightRuleStatusEnum.SUCCESS, dataRegistration.Status);
        Assert.Equal(RightRuleStatusEnum.SUCCESS, ruleRegistration.Status);
        Assert.Equal(RightRuleStatusEnum.DEFAULT, result.Status);
        Assert.Equal("SalesDefault", result.RuleName);
        Assert.Equal("NoMatch", result.Decision);
        Assert.Equal("Use the default decision", result.DecisionData);
    }
}
