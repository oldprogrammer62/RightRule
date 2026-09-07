using BeSoft;
using RightRule.RuleModels;

namespace RightRuleTests;

public class RuleRegistrationTests
{
    [Fact]
    public void RegisterRule_Diff()
    {
        IRightRule rr = new BeSoft.RightRule();

        DefinedRule rule1 = rr.BuildRule()
            .Number(1)
            .Name("Name1")
            .Context("Context1")
            .Statement("a = b")
            .Decision("Decision")
            .FalseDecision("FalseDecision")
            .WithDataCheck(false)
            .Build();

        DefinedRule rule2 = rr.BuildRule()
            .Number(2)
            .Name("Name2")
            .Context("Context2")
            .Statement("a = b")
            .Decision("Decision")
            .FalseDecision("FalseDecision")
            .WithDataCheck(false)
            .Build();

        var result1 = rr.RegisterRule(rule1);
        var result2 = rr.RegisterRule(rule2);

        Assert.NotNull(result1);
        Assert.NotNull(result2);

        Assert.True(result1.IsSuccess);
        Assert.True(result2.IsSuccess);
    }

    [Fact]
    public void RegisterRule_ByNumber()
    {
        IRightRule rr = new BeSoft.RightRule();

        DefinedRule rule1 = rr.BuildRule()
            .Number(1)
            .Name("Name1")
            .Context("Context1")
            .Statement("a = b")
            .Decision("Decision")
            .FalseDecision("FalseDecision")
            .WithDataCheck(false)
            .Build();

        DefinedRule rule2 = rr.BuildRule()
            .Number(1)
            .Name("Name2")
            .Context("Context2")
            .Statement("a = b")
            .Decision("Decision")
            .FalseDecision("FalseDecision")
            .WithDataCheck(false)
            .Build();

        var result1 = rr.RegisterRule(rule1);
        var result2 = rr.RegisterRule(rule2);

        Assert.NotNull(result1);
        Assert.NotNull(result2);

        Assert.True(result1.IsSuccess);
        Assert.False(result2.IsSuccess);
    }

    [Fact]
    public void RegisterRule_ByName()
    {
        IRightRule rr = new BeSoft.RightRule();

        DefinedRule rule1 = rr.BuildRule()
            .Number(1)
            .Name("Name1")
            .Context("Context1")
            .Statement("a = b")
            .Decision("Decision")
            .FalseDecision("FalseDecision")
            .WithDataCheck(false)
            .Build();

        DefinedRule rule2 = rr.BuildRule()
            .Number(2)
            .Name("Name1")
            .Context("Context2")
            .Statement("a = b")
            .Decision("Decision")
            .FalseDecision("FalseDecision")
            .WithDataCheck(false)
            .Build();

        var result1 = rr.RegisterRule(rule1);
        var result2 = rr.RegisterRule(rule2);

        Assert.NotNull(result1);
        Assert.NotNull(result2);
    }
}
