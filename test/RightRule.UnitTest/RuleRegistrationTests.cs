using RightRule;
using RightRule.Models;
using RightRule.RuleServices.RuleModels;

namespace RightRuleTests;

public class RuleRegistrationTests
{
    [Fact]
    public void RegisterRule_Diff()
    {
        var rr = new RightRule.RightRule().CreateSession();

        DefinedRule rule1 = rr.RuleService.BuildRule()
            .Number(1)
            .Name("Name1")
            .Context("Context1")
            .Statement("a = b")
            .Decision("Decision")
            .Build();

        DefinedRule rule2 = rr.RuleService.BuildRule()
            .Number(2)
            .Name("Name2")
            .Context("Context2")
            .Statement("b = c")
            .Decision("Decision")
            .Build();

        var result1 = rr.RuleService.RegisterRule(rule1);
        var result2 = rr.RuleService.RegisterRule(rule2);

        Assert.NotNull(result1);
        Assert.NotNull(result2);

        Assert.Equal(RegistrationStatusEnum.SUCCESS, result1.Status);
        Assert.Equal(RegistrationStatusEnum.SUCCESS, result2.Status);
    }

    [Fact]
    public void RegisterRule_ByNumber()
    {
        var rr = new RightRule.RightRule().CreateSession();

        DefinedRule rule1 = rr.RuleService.BuildRule()
            .Number(1)
            .Name("Name1")
            .Context("Context1")
            .Statement("a = b")
            .Decision("Decision")
            .Build();

        DefinedRule rule2 = rr.RuleService.BuildRule()
            .Number(1)
            .Name("Name2")
            .Context("Context2")
            .Statement("a = b")
            .Decision("Decision")
            .Build();

        var result1 = rr.RuleService.RegisterRule(rule1);
        var result2 = rr.RuleService.RegisterRule(rule2);

        Assert.NotNull(result1);
        Assert.NotNull(result2);

        Assert.Equal(RegistrationStatusEnum.SUCCESS, result1.Status);
        Assert.Equal(RegistrationStatusEnum.SUCCESS, result2.Status);
    }

    [Fact]
    public void RegisterRule_ByName()
    {
        var rr = new RightRule.RightRule().CreateSession();

        DefinedRule rule1 = rr.RuleService.BuildRule()
            .Number(1)
            .Name("Name1")
            .Context("Context1")
            .Statement("a = b")
            .Decision("Decision")
            .Build();

        DefinedRule rule2 = rr.RuleService.BuildRule()
            .Number(2)
            .Name("Name1")
            .Context("Context2")
            .Statement("a = b")
            .Decision("Decision")
            .Build();

        var result1 = rr.RuleService.RegisterRule(rule1);
        var result2 = rr.RuleService.RegisterRule(rule2);

        Assert.NotNull(result1);
        Assert.NotNull(result2);
    }

    [Fact]
    public void RegisterRule_FromSession_ReturnsSuccess()
    {
        using var session = new RightRule.RightRule().CreateSession();

        var rule = session.RuleService.BuildRule()
            .Number(1)
            .Name("Rule1")
            .Context("Sales")
            .Statement("a = b")
            .Decision("Approved")
            .Build();

        var result = session.RuleService.RegisterRule(rule);

        Assert.Equal(RegistrationStatusEnum.SUCCESS, result.Status);
        Assert.Same(rule.Decision, result.RegisteredRule.Decision);
    }

    [Fact]
    public void RegisterRule_FromSession_ReturnsFailureForDuplicateRuleNumber()
    {
        using var session = new RightRule.RightRule().CreateSession();

        var firstRule = session.RuleService.BuildRule()
            .Number(1)
            .Name("First")
            .Context("Sales")
            .Statement("a = b")
            .Decision("Approved")
            .Build();

        var duplicateRule = session.RuleService.BuildRule()
            .Number(1)
            .Name("Second")
            .Context("Sales")
            .Statement("b = c")
            .Decision("Rejected")
            .Build();

        var firstResult = session.RuleService.RegisterRule(firstRule);
        var duplicateResult = session.RuleService.RegisterRule(duplicateRule);

        Assert.Equal(RegistrationStatusEnum.SUCCESS, firstResult.Status);
        Assert.Equal(RegistrationStatusEnum.FAILURE, duplicateResult.Status);
        Assert.Same(duplicateRule.Decision, duplicateResult.RegisteredRule.Decision);
    }

    [Fact]
    public void RegisterRule_FromSession_ReturnsErrorForInvalidRule()
    {
        using var session = new RightRule.RightRule().CreateSession();

        var invalidRule = DefinedRule.Build(
            1,
            "InvalidRule",
            "Sales",
            string.Empty,
            "Approved",
            null,
            null);

        var result = session.RuleService.RegisterRule(invalidRule);

        Assert.Equal(RegistrationStatusEnum.ERROR, result.Status);
        Assert.Null(result.RegisteredRule);
        Assert.Contains("Statement", result.Message);
    }

    [Fact]
    public void ApplyRule_FromSession_ReturnsConfiguredDefaultWhenNoRuleMatches()
    {
        using var session = new RightRule.RightRule().CreateSession();

        var defaultRule = new DefaultRule
        {
            Name = "SalesDefault",
            Context = "Sales",
            Decision = "NoMatch"
        };

        var registration = session.RuleService.RegisterDefaultRule(defaultRule);

        var nonMatchingRule = session.RuleService.BuildRule()
            .Number(1)
            .Name("NonMatchingRule")
            .Context("Sales")
            .Statement("11 < 10")
            .Decision("NotApplicable")
            .Build();

        var ruleRegistration = session.RuleService.RegisterRule(nonMatchingRule);
        var result = session.RuleService.ApplyRule("Sales");

        Assert.Equal(RegistrationStatusEnum.SUCCESS, registration.Status);
        Assert.Equal(RegistrationStatusEnum.SUCCESS, ruleRegistration.Status);
        Assert.Equal("SalesDefault", result.RuleName);
        Assert.Equal("NoMatch", result.Decision);
    }

    [Fact]
    public void ApplyRule_FromSession_ReturnsSuccessStatusWhenRuleMatches()
    {
        using var session = new RightRule.RightRule().CreateSession();

        var rule = session.RuleService.BuildRule()
            .Number(1)
            .Name("MatchingRule")
            .Context("Sales")
            .Statement("11 >= 10")
            .Decision("Approved")
            .Build();

        var registration = session.RuleService.RegisterRule(rule);
        var result = session.RuleService.ApplyRule("Sales");

        Assert.Equal(RegistrationStatusEnum.SUCCESS, registration.Status);
        Assert.Equal(RightRuleResultEnum.SUCCESS, result.Status);
        Assert.True(result.Result);
        Assert.Equal("MatchingRule", result.RuleName);
    }
}
