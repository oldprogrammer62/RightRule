using RightRule;
using RightRule.LanguageProcessor.Evaluator;
using RightRule.Models;
using RightRule.RuleServices.RuleRegistration;

namespace RightRuleTests;

public class RuleBuilder_Test
{
    [Fact]
    public void TestBuilder()
    {
        var rr = new RightRule.RightRule().CreateSession();

        RuleRegistrationResult? rule1 = rr.RuleService.BuildRule()
            .Number(1)
            .Name("Rule1")
            .Context("Context")
            .Statement("a = b")
            .Decision("GOOD")
            .DecisionData(null)
            .OptionalData(null)
            .Build()
            .Register(rr.RuleService);

        var rule2 = rr.RuleService.BuildRule()
            .Number(2)
            .Name("Name2")
            .Context("Context")
            .Statement("a = b")
            .Decision("GOOD")
            .Build();
        var x2 = rr.RuleService.RegisterRule(rule2);

        Assert.NotNull(rule1);
        Assert.IsType<RuleRegistrationResult>(rule1);

        Assert.NotNull(x2);
        Assert.IsType<RuleRegistrationResult>(x2);
    }

    [Fact]
    public void TestBuildAndRegister_First()
    {
        var rr = new RightRule.RightRule(new RightRuleOptions() { DataCheck = true }).CreateSession();

        rr.DataService.RegisterData("a", 100);
        rr.DataService.RegisterData("b", 100);

        string context = "Context";

        var rule1 = rr.RuleService.BuildRule()
            .Number(1)
            .Name("Rule1")
            .Context(context)
            .Statement("a <> b")
            .Decision("GOOD")
            .DecisionData(null)
            .OptionalData(null)
            .Build();
        var rule1reg = rr.RuleService.RegisterRule(rule1);

        var rule2 = rr.RuleService.BuildRule()
            .Number(2)
            .Name("Name2")
            .Context(context)
            .Statement("a = b")
            .Decision("GOOD")
            .Build();
        var rule2reg = rr.RuleService.RegisterRule(rule2);

        Assert.NotNull(rule1);
        Assert.IsType<RuleRegistrationResult>(rule1reg);

        Assert.NotNull(rule2);
        Assert.IsType<RuleRegistrationResult>(rule2reg);

        var result = rr.RuleService.ApplyRules(context, EvaluationTypeEnum.FIRST);
        Assert.Single(result);
        Assert.Equal(result[0].Decision, rule2reg?.RegisteredRule?.Decision);
    }

    [Fact]
    public void TestBuildAndRegister_Any()
    {
        var rr = new RightRule.RightRule(new RightRuleOptions() { DataCheck = true }).CreateSession();

        rr.DataService.RegisterData("a", 100);
        rr.DataService.RegisterData("b", 100);

        string context = "Context";

        var rule1 = rr.RuleService.BuildRule()
            .Number(1)
            .Name("Rule1")
            .Context(context)
            .Statement("a <> b")
            .Decision("GOOD")
            .DecisionData(null)
            .OptionalData(null)
            .Build();
        var rule1reg = rr.RuleService.RegisterRule(rule1);

        var rule2 = rr.RuleService.BuildRule()
            .Number(2)
            .Name("Name2")
            .Context(context)
            .Statement("a = b")
            .Decision("GOOD")
            .Build();
        var rule2reg = rr.RuleService.RegisterRule(rule2);

        Assert.NotNull(rule1);
        Assert.IsType<RuleRegistrationResult>(rule1reg);

        Assert.NotNull(rule2);
        Assert.IsType<RuleRegistrationResult>(rule2reg);

        var result = rr.RuleService.ApplyRules(context, EvaluationTypeEnum.ANY);
        Assert.Equal(2, result.Count);
        Assert.Equal(result[0].Decision, rule1reg?.RegisteredRule?.Decision);
        Assert.Equal(result[1].Decision, rule2reg?.RegisteredRule?.Decision);
    }

    [Fact]
    public void TestBuildAndRegister_All_Success()
    {
        var rr = new RightRule.RightRule(new RightRuleOptions() { DataCheck = true }).CreateSession();

        rr.DataService.RegisterData("a", 100);
        rr.DataService.RegisterData("b", 100);

        string context = "ContextXXX";

        var rule1 = rr.RuleService.BuildRule()
            .Number(1)
            .Name("Rule1")
            .Context(context)
            .Statement("a = b")
            .Decision("GOOD")
            .DecisionData(null)
            .OptionalData(null)
            .Build();
        var rule1reg = rr.RuleService.RegisterRule(rule1);

        var rule2 = rr.RuleService.BuildRule()
            .Number(2)
            .Name("Name2")
            .Context(context)
            .Statement("a = b")
            .Decision("GOOD")
            .Build();
        var rule2reg = rr.RuleService.RegisterRule(rule2);

        Assert.NotNull(rule1);
        Assert.IsType<RuleRegistrationResult>(rule1reg);

        Assert.NotNull(rule2);
        Assert.IsType<RuleRegistrationResult>(rule2reg);

        var result = rr.RuleService.ApplyRules(context, EvaluationTypeEnum.ALL);
        Assert.Equal(2, result.Count);
        Assert.Equal(result[0].Decision, rule1reg?.RegisteredRule?.Decision);
        Assert.Equal(result[1].Decision, rule2reg?.RegisteredRule?.Decision);
    }

    [Fact]
    public void TestBuildAndRegister_All_Fail()
    {
        var rr = new RightRule.RightRule(new RightRuleOptions() { DataCheck = true }).CreateSession();

        rr.DataService.RegisterData("a", 100);
        rr.DataService.RegisterData("b", 100);

        string context = "Context";

        var rule1 = rr.RuleService.BuildRule()
            .Number(1)
            .Name("Rule1")
            .Context(context)
            .Statement("a <> b")
            .Decision("GOOD")
            .DecisionData(null)
            .OptionalData(null)
            .Build();
        var rule1reg = rr.RuleService.RegisterRule(rule1);

        var rule2 = rr.RuleService.BuildRule()
            .Number(2)
            .Name("Name2")
            .Context(context)
            .Statement("a = b")
            .Decision("GOOD")
            .Build();
        var rule2reg = rr.RuleService.RegisterRule(rule2);

        Assert.NotNull(rule1);
        Assert.IsType<RuleRegistrationResult>(rule1reg);

        Assert.NotNull(rule2);
        Assert.IsType<RuleRegistrationResult>(rule2reg);

        var result = rr.RuleService.ApplyRules(context, EvaluationTypeEnum.ALL);
        Assert.Equal(2, result.Count);

        Assert.Equal(result[0].Decision, rule1.Decision);
        Assert.Equal(RightRuleStatusEnum.FAILURE, result[0].Status);

        Assert.Equal(result[1].Decision, rule2.Decision);
        Assert.Equal(RightRuleStatusEnum.SUCCESS, result[1].Status);
    }
}
