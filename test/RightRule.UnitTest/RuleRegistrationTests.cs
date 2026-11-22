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

        Assert.Equal(RightRuleStatusEnum.SUCCESS, result1.Status);
        Assert.Equal(RightRuleStatusEnum.SUCCESS, result2.Status);
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

        Assert.Equal(RightRuleStatusEnum.SUCCESS, result1.Status);
        Assert.Equal(RightRuleStatusEnum.SUCCESS, result2.Status);
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
}
