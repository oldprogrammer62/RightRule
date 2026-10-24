using RightRule;
using RightRule.RuleServices.RuleModels;
using RightRule.RuleServices.RuleModels.BaseModels;

namespace RightRuleTests;

public class ApplyFirstRule_False
{
    [Fact]
    public void First_True_Second_False_Test()
    {
        IRightRule rr = new RightRule.RightRule();

        var rule1 = rr.BuildRule()
            .Number(1)
            .Name("Name1")
            .Context("Context")
            .Statement("2 = 2")
            .TrueDecision("CORRECT")
            .FalseDecision("INCORRECT")
            .Build();
        var rrule1 = rr.RegisterRule(rule1);

        var rule2 = rr.BuildRule()
            .Number(2)
            .Name("Name2")
            .Context("Context")
            .Statement("1 = 2")
            .TrueDecision("CORRECT")
            .FalseDecision("INCORRECT")
            .Build();
        var rrule2 = rr.RegisterRule(rule2);

        RightRuleResult<IProcessedRule> result = rr.ApplyRule("Context");

        Assert.NotNull(result);
        Assert.Equal(1, result.RuleNumber);
    }

    [Fact]
    public void First_False_Second_True_Test()
    {
        IRightRule rr = new RightRule.RightRule();

        var rule1 = rr.BuildRule()
            .Number(1)
            .Name("Name1")
            .Context("Context")
            .Statement("2 = 1")
            .TrueDecision("CORRECT")
            .FalseDecision("INCORRECT")
            .Build();
        var rrule1 = rr.RegisterRule(rule1);

        var rule2 = rr.BuildRule()
            .Number(2)
            .Name("Name2")
            .Context("Context")
            .Statement("2 = 2")
            .TrueDecision("CORRECT")
            .FalseDecision("INCORRECT")
            .Build();
        var rrule2 = rr.RegisterRule(rule2);

        var result = rr.ApplyRule("Context");

        Assert.NotNull(result);
        Assert.Equal(2, result.RuleNumber);
    }

    [Fact]
    public void First_False_Second_False_Test()
    {
        IRightRule rr = new RightRule.RightRule();

        var rule1 = rr.BuildRule()
            .Number(1)
            .Name("Name1")
            .Context("Context")
            .Statement("2 = 1")
            .TrueDecision("CORRECT")
            .FalseDecision("INCORRECT")
            .Build();
        var rrule1 = rr.RegisterRule(rule1);

        var rule2 = rr.BuildRule()
            .Number(2)
            .Name("Name2")
            .Context("Context")
            .Statement("2 = 3")
            .TrueDecision("CORRECT")
            .FalseDecision("INCORRECT")
            .Build();
        var rrule2 = rr.RegisterRule(rule2);

        var result = rr.ApplyRule("Context");

        Assert.NotNull(result);
        Assert.Equal(DefaultRule.DECISIONWORD, result.Decision);
    }
}
