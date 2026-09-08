using BeSoft;
using RightRule.RegistrationServices;
using RightRule.RuleModels;
using RightRule.RuleModels.RuleModels;

namespace RightRuleTests;

public class DefaultRule_Tests
{
    [Fact]
    public void DefaultRule1_SingleMatch_NoResult()
    {
        IRightRule rr = new BeSoft.RightRule();

        var ruleDef1 = rr.BuildRule()
            .Number(1)
            .Name("Rule1")
            .Context("Test")
            .Statement("11 >= 19")
            .Decision("true")
            .FalseDecision("False")
            .Build();
        var rule1 = rr.RegisterRule(ruleDef1);
        var result1 = rr.ApplyRules("Test");
        Assert.NotNull(result1);
    }

    [Fact]
    public void DefaultRule2()
    {
        IRightRule rr = new BeSoft.RightRule();

        var ruleDef1 = rr.BuildRule()
            .Number(1)
            .Name("Rule1")
            .Context("Test")
            .Statement("11 >= 19")
            .Decision("true")
            .FalseDecision("False")
            .Build();
        var rule1 = rr.RegisterRule(ruleDef1);
        var result1 = rr.ApplyRules("Test", RightRule.System.Common.RulesToReturn.All);
        Assert.NotNull(result1);
    }

    [Fact]
    public void RegisterNewDefaultRule()
    {
        IRightRule rr = new BeSoft.RightRule();

        string context = "Hello";

        var defaultRule1 = rr.GetDefaultRule(context);
        Assert.NotNull(defaultRule1);

        var newDefaultRule = new DefaultRule()
        {
            Name = "DefaultRule1",
            Context = context,
            Decision = "dec",
            DecisionData = "DefaultDecision"
        };  
        var result = rr.RegisterDefaultRule(newDefaultRule);
        Assert.Equal(RegistrationStatus.SUCCESS, result.Status);

        var defaultRule2 = rr.GetDefaultRule(context);
        Assert.NotNull(defaultRule2);
    }

    [Fact]
    public void RegisterNewDefaultRuleAndUseIt()
    {
        IRightRule rr = new BeSoft.RightRule();

        string context = "Hello";

        var defaultRule1 = rr.GetDefaultRule(context);
        Assert.NotNull(defaultRule1);

        var newDefaultRule = new DefaultRule()
        {
            Name = "HOWDYDOODY",
            Context = context,
            Decision = "GOTITNOW",
            DecisionData = "HELLO WORLD"
        };
        var result = rr.RegisterDefaultRule(newDefaultRule);
        Assert.Equal(RegistrationStatus.SUCCESS, result.Status);

        var defaultRule2 = rr.GetDefaultRule(context);
        Assert.NotNull(defaultRule2);

        var ruleDef1 = rr.BuildRule()
            .Number(1)
            .Name("Rule1")
            .Context(context)
            .Statement("11 >= 19")
            .Decision("true")
            .FalseDecision("False")
            .Build();
        var rule1 = rr.RegisterRule(ruleDef1);
        var result1 = rr.ApplyRules(context);
        Assert.NotNull(result1);
    }

    [Fact]
    public void RegisterNewDefaultMoreThanOnce()
    {
        IRightRule rr = new BeSoft.RightRule();

        string context1 = "Hello";
        string context2 = "world";

        var originalDefaultRule = rr.GetDefaultRule(context1);
        Assert.NotNull(originalDefaultRule);

        var newDefaultRule_First = new DefaultRule()
        {
            Name = "DefaultRuleContext_First",
            Context = context1,
            Decision = "DecisionContext_First",
            DecisionData = "DefaultDecision"
        };
        var firstResult = rr.RegisterDefaultRule(newDefaultRule_First);
        Assert.Equal(RegistrationStatus.SUCCESS, firstResult.Status);
        var foundDefaultRule_First = rr.GetDefaultRule(context1);
        Assert.NotNull(foundDefaultRule_First);
        Assert.Equal(foundDefaultRule_First.Name, newDefaultRule_First.Name);

        var newDefaultRule_Second = new DefaultRule()
        {
            Name = "DefaultRuleContext_Second",
            Context = context1,
            Decision = "DecisionContext_Second",
            DecisionData = "DefaultDecision"
        };
        var secondResult = rr.RegisterDefaultRule(newDefaultRule_Second);
        Assert.Equal(RegistrationStatus.SUCCESS, secondResult.Status);
        var foundDefaultRule_Second = rr.GetDefaultRule(context1);
        Assert.NotNull(foundDefaultRule_Second);
        Assert.Equal(foundDefaultRule_Second.Name, newDefaultRule_Second.Name);

        var newDefaultRule_Third = new DefaultRule()
        {
            Name = "DefaultRuleContext_Third",
            Context = context2,
            Decision = "DecisionContext_Third",
            DecisionData = "DefaultDecision"
        };
        var thirdResult = rr.RegisterDefaultRule(newDefaultRule_Third);
        Assert.Equal(RegistrationStatus.SUCCESS, thirdResult.Status);
        var foundDefaultRule_Third = rr.GetDefaultRule(context2);
        Assert.NotNull(foundDefaultRule_Third);
        Assert.Equal(foundDefaultRule_Third.Name, newDefaultRule_Third.Name);

        var DefaultRuleContext1 = rr.GetDefaultRule(context1);
        Assert.NotNull(DefaultRuleContext1);
        Assert.Equal(DefaultRuleContext1.Context, context1);

        var DefaultRuleContext2 = rr.GetDefaultRule(context2);
        Assert.NotNull(DefaultRuleContext2);
        Assert.Equal(DefaultRuleContext2.Context, context2);

        var DefaultRuleContext3 = rr.GetDefaultRule("NotFound");
        Assert.NotNull(DefaultRuleContext3);
        Assert.Equal(DefaultRuleContext3.Context, string.Empty);
    }

    [Fact]
    public void DefaultRuleBuilder_Test()
    {
        IRightRule rr = new BeSoft.RightRule();

        string Context = "ContextFromBuilder";

        var defaultRuleDefn = rr.BuildDefaultRule()
            .Name("DefaultName")
            .Context(Context)
            .Decision("WhatEver")
            .Build();

        var defRuleRegistered = rr.RegisterDefaultRule(defaultRuleDefn);
        var defaultRule = rr.GetDefaultRule(Context);
        Assert.NotNull(defaultRule);
    }
}
