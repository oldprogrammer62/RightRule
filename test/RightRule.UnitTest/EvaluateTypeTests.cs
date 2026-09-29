using RightRule;
using RightRule.LanguageProcessor.Evaluator;

namespace RightRuleTests;

public class EvaluateTypeTests
{
    [Fact]
    public void TestEvaluate_ApplyRule()
    {
        IRightRule rr = new RightRule.RightRule();

        var ruleDef = rr.BuildRule()
            .Number(1)
            .Name("Name1")
            .Context("Context")
            .Statement("1 = 1")
            .Decision("Hello")
            .FalseDecision("Goodbye")
            .Build();

        var rule1 = rr.RegisterRule(ruleDef);

        var result = rr.ApplyRule("Context");
        result.ToString();
        Assert.NotNull(result);
    }

    [Fact]
    public void TestEvaluate_ApplyRules()
    {
        IRightRule rr = new RightRule.RightRule();

        var ruleDef1 = rr.BuildRule()
            .Number(1)
            .Name("Name1")
            .Context("Context")
            .Statement("1 = 0")
            .Decision("Hello")
            .FalseDecision("Goodbye")
            .Build();

        var ruleDef2 = rr.BuildRule()
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
        Assert.Single(result);
        Assert.Equal(result[0].Decision, ruleDef2.Decision);
    }

    [Fact]
    public void TestEvaluate_ApplyRules_First()
    {
        IRightRule rr = new RightRule.RightRule();

        var ruleDef1 = rr.BuildRule()
            .Number(1)
            .Name("Name1")
            .Context("Context")
            .Statement("1 = 0")
            .Decision("Hello")
            .FalseDecision("Goodbye")
            .Build();

        var ruleDef2 = rr.BuildRule()
            .Number(2)
            .Name("Name2")
            .Context("Context")
            .Statement("1 = 1")
            .Decision("Hello2")
            .FalseDecision("Goodbye2")
            .Build();

        var rule1 = rr.RegisterRule(ruleDef1);
        var rule2 = rr.RegisterRule(ruleDef2);

        var result = rr.ApplyRules("Context", EvaluationTypeEnum.FIRST);
        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Equal(result[0].Decision, ruleDef2.Decision);
    }

    [Fact]
    public void TestEvaluate_ApplyRules_Any()
    {
        IRightRule rr = new RightRule.RightRule();

        var ruleDef1 = rr.BuildRule()
            .Number(1)
            .Name("Name1")
            .Context("Context")
            .Statement("1 = 1")
            .Decision("Hello")
            .FalseDecision("Goodbye")
            .Build();

        var ruleDef2 = rr.BuildRule()
            .Number(2)
            .Name("Name2")
            .Context("Context")
            .Statement("1 = 0")
            .Decision("Hello2")
            .FalseDecision("Goodbye2")
            .Build();

        var rule1 = rr.RegisterRule(ruleDef1);
        var rule2 = rr.RegisterRule(ruleDef2);

        var result = rr.ApplyRules("Context", EvaluationTypeEnum.ANY);
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        Assert.Equal(result[0].Decision, ruleDef1.Decision);
        Assert.Equal(result[1].Decision, ruleDef2.FalseDecision);
    }

    [Fact]
    public void TestEvaluate_ApplyRules_All_Success()
    {
        IRightRule rr = new RightRule.RightRule();

        var ruleDef1 = rr.BuildRule()
            .Number(1)
            .Name("Name1")
            .Context("Context")
            .Statement("1 = 1")
            .Decision("Hello")
            .FalseDecision("Goodbye")
            .Build();

        var ruleDef2 = rr.BuildRule()
            .Number(2)
            .Name("Name2")
            .Context("Context")
            .Statement("9 = 9")
            .Decision("Hello2")
            .FalseDecision("Goodbye2")
            .Build();

        var rule1 = rr.RegisterRule(ruleDef1);
        var rule2 = rr.RegisterRule(ruleDef2);

        var result = rr.ApplyRules("Context", EvaluationTypeEnum.ALL);
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        Assert.Equal(result[0].Decision, ruleDef1.Decision);
        Assert.Equal(result[1].Decision, ruleDef2.Decision);
    }

    [Fact]
    public void TestEvaluate_ApplyRules_All_Fail()
    {
        IRightRule rr = new RightRule.RightRule();

        var ruleDef1 = rr.BuildRule()
            .Number(1)
            .Name("Name1")
            .Context("Context")
            .Statement("1 = 1")
            .Decision("Hello")
            .FalseDecision("Goodbye")
            .Build();

        var ruleDef2 = rr.BuildRule()
            .Number(2)
            .Name("Name2")
            .Context("Context")
            .Statement("9 = 999")
            .Decision("Hello2")
            .FalseDecision("Goodbye2")
            .Build();

        var rule1 = rr.RegisterRule(ruleDef1);
        var rule2 = rr.RegisterRule(ruleDef2);

        var result = rr.ApplyRules("Context", EvaluationTypeEnum.ALL);
        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Equal(result[0].Decision, rr.GetDefaultRule("Context").Decision);
    }
}
