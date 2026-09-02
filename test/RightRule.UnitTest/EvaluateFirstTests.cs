using BeSoft;
using RightRule.System.Common;

namespace RightRuleTests;

public class EvaluateFirstTests
{
    [Fact]
    public void TestEvaluateFirst()
    {
        IRightRule rr = new BeSoft.RightRule();

        var ruleDef = rr.BuildRuleDefinition()
            .Number(1)
            .Name("Name1")
            .Context("Context")
            .Statement("1 = 1")
            .Decision("Hello")
            .FalseDecision("Goodbye")
            .Build();

        var rule1 = rr.RegisterRule(ruleDef);

        var result = rr.ApplyRules("Context");
        Assert.NotNull(result);
    }

    [Fact]
    public void TestEvaluateFirst_2()
    {
        IRightRule rr = new BeSoft.RightRule();

        var ruleDef1 = rr.BuildRuleDefinition()
            .Number(1)
            .Name("Name1")
            .Context("Context")
            .Statement("1 = 0")
            .Decision("Hello")
            .FalseDecision("Goodbye")
            .Build();

        var ruleDef2 = rr.BuildRuleDefinition()
            .Number(2)
            .Name("Name2")
            .Context("Context")
            .Statement("1 = 1")
            .Decision("Hello2")
            .FalseDecision("Goodbye2")
            .Build();

        var rule1 = rr.RegisterRule(ruleDef1);
        var rule2 = rr.RegisterRule(ruleDef2);

        var result = rr.ApplyRules("Context");
        Assert.NotNull(result);
        Assert.Equal(result.Decision, ruleDef2.Decision);
    }

    [Fact]
    public void TestEvaluateAll()
    {
        IRightRule rr = new BeSoft.RightRule();

        var ruleDef1 = rr.BuildRuleDefinition()
            .Number(1)
            .Name("Name1")
            .Context("Context")
            .Statement("1 = 1")
            .Decision("Hello")
            .FalseDecision("Goodbye")
            .Build();

        var ruleDef2 = rr.BuildRuleDefinition()
            .Number(2)
            .Name("Name2")
            .Context("Context")
            .Statement("1 = 0")
            .Decision("Hello2")
            .FalseDecision("Goodbye2")
            .Build();

        var rule1 = rr.RegisterRule(ruleDef1);
        var rule2 = rr.RegisterRule(ruleDef2);

        var result = rr.ApplyRules("Context", RulesToReturn.All);
        Assert.NotNull(result);
        Assert.Equal(result[0].Decision, ruleDef1.Decision);
        Assert.Equal(result[1].Decision, ruleDef2.FalseDecision);
    }
}
