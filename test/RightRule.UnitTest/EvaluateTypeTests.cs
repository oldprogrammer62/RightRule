using RightRule;
using RightRule.LanguageProcessor.Evaluator;
using RightRule.Models;

namespace RightRuleTests;

public class EvaluateTypeTests
{
    [Fact]
    public void TestEvaluate_ApplyRule()
    {
        var rr = new RightRule.RightRule().CreateSession();

        var ruleDef = rr.RuleService.BuildRule()
            .Number(1)
            .Name("Name1")
            .Context("Context")
            .Statement("1 = 1")
            .Decision("Hello")
            .Build();

        var rule1 = rr.RuleService.RegisterRule(ruleDef);

        var result = rr.RuleService.ApplyRule("Context");
        result.ToString();
        Assert.NotNull(result);
    }

    [Fact]
    public void TestEvaluate_ApplyRules()
    {
        var rr = new RightRule.RightRule().CreateSession();

        var ruleDef1 = rr.RuleService.BuildRule()
            .Number(1)
            .Name("Name1")
            .Context("Context")
            .Statement("1 = 0")
            .Decision("Hello")
            .Build();

        var ruleDef2 = rr.RuleService.BuildRule()
            .Number(2)
            .Name("Name2")
            .Context("Context")
            .Statement("1 = 1")
            .Decision("Hello2")
            .Build();

        var rule1 = rr.RuleService.RegisterRule(ruleDef1);
        var rule2 = rr.RuleService.RegisterRule(ruleDef2);

        var result = rr.RuleService.ApplyRules("Context");
        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Equal(result[0].Decision, ruleDef2.Decision);
    }

    [Fact]
    public void TestEvaluate_ApplyRules_First()
    {
        var rr = new RightRule.RightRule().CreateSession();

        var ruleDef1 = rr.RuleService.BuildRule()
            .Number(1)
            .Name("Name1")
            .Context("Context")
            .Statement("1 = 0")
            .Decision("Hello")
            .Build();

        var ruleDef2 = rr.RuleService.BuildRule()
            .Number(2)
            .Name("Name2")
            .Context("Context")
            .Statement("1 = 1")
            .Decision("Hello2")
            .Build();

        var rule1 = rr.RuleService.RegisterRule(ruleDef1);
        var rule2 = rr.RuleService.RegisterRule(ruleDef2);

        var result = rr.RuleService.ApplyRules("Context", EvaluationTypeEnum.FIRST);
        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Equal(result[0].Decision, ruleDef2.Decision);
    }

    [Fact]
    public void TestEvaluate_ApplyRules_Any()
    {
        var rr = new RightRule.RightRule().CreateSession();

        var ruleDef1 = rr.RuleService.BuildRule()
            .Number(1)
            .Name("Name1")
            .Context("Context")
            .Statement("1 = 1")
            .Decision("Hello")
            .Build();

        var ruleDef2 = rr.RuleService.BuildRule()
            .Number(2)
            .Name("Name2")
            .Context("Context")
            .Statement("1 = 0")
            .Decision("Hello2")
            .Build();

        var rule1 = rr.RuleService.RegisterRule(ruleDef1);
        var rule2 = rr.RuleService.RegisterRule(ruleDef2);

        var result = rr.RuleService.ApplyRules("Context", EvaluationTypeEnum.ANY);
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        Assert.Equal(result[0].Decision, ruleDef1.Decision);
    }

    [Fact]
    public void TestEvaluate_ApplyRules_All_Success()
    {
        var rr = new RightRule.RightRule().CreateSession();

        var ruleDef1 = rr.RuleService.BuildRule()
            .Number(1)
            .Name("Name1")
            .Context("Context")
            .Statement("1 = 1")
            .Decision("Hello")
            .Build();

        var ruleDef2 = rr.RuleService.BuildRule()
            .Number(2)
            .Name("Name2")
            .Context("Context")
            .Statement("9 = 9")
            .Decision("Hello2")
            .Build();

        var rule1 = rr.RuleService.RegisterRule(ruleDef1);
        var rule2 = rr.RuleService.RegisterRule(ruleDef2);

        var result = rr.RuleService.ApplyRules("Context", EvaluationTypeEnum.ALL);
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        Assert.Equal(result[0].Decision, ruleDef1.Decision);
        Assert.Equal(result[1].Decision, ruleDef2.Decision);
    }

    [Fact]
    public void TestEvaluate_ApplyRules_All_Fail()
    {
        var rr = new RightRule.RightRule().CreateSession();

        var ruleDef1 = rr.RuleService.BuildRule()
            .Number(1)
            .Name("Name1")
            .Context("Context")
            .Statement("1 = 1")
            .Decision("Hello")
            .Build();

        var ruleDef2 = rr.RuleService.BuildRule()
            .Number(2)
            .Name("Name2")
            .Context("Context")
            .Statement("9 = 999")
            .Decision("Hello2")
            .Build();

        var rule1 = rr.RuleService.RegisterRule(ruleDef1);
        var rule2 = rr.RuleService.RegisterRule(ruleDef2);

        var result = rr.RuleService.ApplyRules("Context", EvaluationTypeEnum.ALL);
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);

        Assert.Equal(RightRuleStatusEnum.SUCCESS, result[0].Status);
        Assert.Equal(RightRuleStatusEnum.FAILURE, result[1].Status);
    }
}
