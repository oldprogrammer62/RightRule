using BeSoft;
using RightRule.Models.RuleModels;

namespace RightRuleTests;

public class ApplyFirstRule_False
{
    [Fact]
    public void First_True_Second_False_Test()
    {
        IRightRule rr = new BeSoft.RightRule();

        var rule1 = rr.BuildRule()
            .Number(1)
            .Name("Name1")
            .Context("Context")
            .Statement("2 = 2")
            .Decision("CORRECT")
            .FalseDecision("INCORRECT")
            .Build();
        var rrule1 = rr.RegisterRule(rule1);

        var rule2 = rr.BuildRule()
            .Number(2)
            .Name("Name2")
            .Context("Context")
            .Statement("1 = 2")
            .Decision("CORRECT")
            .FalseDecision("INCORRECT")
            .Build();
        var rrule2 = rr.RegisterRule(rule2);

        var result = rr.ApplyRules("Context");

        Assert.NotNull(result);
        Assert.Equal(1, result.RuleNumber);
    }

    [Fact]
    public void First_False_Second_True_Test()
    {
        IRightRule rr = new BeSoft.RightRule();

        var rule1 = rr.BuildRule()
            .Number(1)
            .Name("Name1")
            .Context("Context")
            .Statement("2 = 1")
            .Decision("CORRECT")
            .FalseDecision("INCORRECT")
            .Build();
        var rrule1 = rr.RegisterRule(rule1);

        var rule2 = rr.BuildRule()
            .Number(2)
            .Name("Name2")
            .Context("Context")
            .Statement("2 = 2")
            .Decision("CORRECT")
            .FalseDecision("INCORRECT")
            .Build();
        var rrule2 = rr.RegisterRule(rule2);

        var result = rr.ApplyRules("Context");

        Assert.NotNull(result);
        Assert.Equal(2, result.RuleNumber);
    }


    [Fact]
    public void First_False_Second_False_Test()
    {
        IRightRule rr = new BeSoft.RightRule();

        var rule1 = rr.BuildRule()
            .Number(1)
            .Name("Name1")
            .Context("Context")
            .Statement("2 = 1")
            .Decision("CORRECT")
            .FalseDecision("INCORRECT")
            .Build();
        var rrule1 = rr.RegisterRule(rule1);

        var rule2 = rr.BuildRule()
            .Number(2)
            .Name("Name2")
            .Context("Context")
            .Statement("2 = 3")
            .Decision("CORRECT")
            .FalseDecision("INCORRECT")
            .Build();
        var rrule2 = rr.RegisterRule(rule2);

        var result = rr.ApplyRules("Context");

        Assert.NotNull(result);
        Assert.Equal(DefaultRule.DECISIONWORD, result.Decision);
    }
}
