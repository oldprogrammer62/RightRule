using BeSoft;
using RightRule.RuleModels;

namespace RightRuleTests;

public class ApplyFirstRule_False
{
    [Fact]
    public void First_True_Second_False_Test()
    {
        IRightRule rr = new BeSoft.RightRule();

        var rule1 = rr.BuildRuleDefinition()
            .Number(1)
            .Name("Name1")
            .Context("Context")
            .Statement("2 = 2")
            .Decision("CORRECT")
            .Build();
        var rrule1 = rr.RegisterRule(rule1);

        var rule2 = rr.BuildRuleDefinition()
            .Number(2)
            .Name("Name2")
            .Context("Context")
            .Statement("1 = 2")
            .Decision("INCORRECT")
            .Build();
        var rrule2 = rr.RegisterRule(rule2);

        var result = rr.EvaluateFirstMatch("Context");

        Assert.NotNull(result);
        Assert.Equal(1, result.Number);
    }

    [Fact]
    public void First_False_Second_True_Test()
    {
        IRightRule rr = new BeSoft.RightRule();

        var rule1 = rr.BuildRuleDefinition()
            .Number(1)
            .Name("Name1")
            .Context("Context")
            .Statement("2 = 1")
            .Decision("INCORRECT")
            .Build();
        var rrule1 = rr.RegisterRule(rule1);

        var rule2 = rr.BuildRuleDefinition()
            .Number(2)
            .Name("Name2")
            .Context("Context")
            .Statement("2 = 2")
            .Decision("CORRECT")
            .Build();
        var rrule2 = rr.RegisterRule(rule2);

        var result = rr.EvaluateFirstMatch("Context");

        Assert.NotNull(result);
        Assert.Equal(2, result.Number);
    }


    [Fact]
    public void First_False_Second_False_Test()
    {
        IRightRule rr = new BeSoft.RightRule();

        var rule1 = rr.BuildRuleDefinition()
            .Number(1)
            .Name("Name1")
            .Context("Context")
            .Statement("2 = 1")
            .Decision("INCORRECT")
            .Build();
        var rrule1 = rr.RegisterRule(rule1);

        var rule2 = rr.BuildRuleDefinition()
            .Number(2)
            .Name("Name2")
            .Context("Context")
            .Statement("2 = 3")
            .Decision("INCORRECT")
            .Build();
        var rrule2 = rr.RegisterRule(rule2);

        var result = rr.EvaluateFirstMatch("Context");

        Assert.NotNull(result);
        Assert.Equal(AppliedRule.DefaultResult().Decision, result.Decision);
    }
}
