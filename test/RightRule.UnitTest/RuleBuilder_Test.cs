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
        IRightRule rr = new RightRule.RightRule();

        RuleRegistrationResult? rule1 = rr.BuildRule()
            .Number(1)
            .Name("Rule1")
            .Context("Context")
            .Statement("a = b")
            .Decision("GOOD")
            .FalseDecision("BAD")
            .DecisionData(null)
            .OptionalData(null)
            .Build()
            .Register(rr.RuleService);

        var rule2 = rr.BuildRule()
            .Number(2)
            .Name("Name2")
            .Context("Context")
            .Statement("a = b")
            .Decision("GOOD")
            .FalseDecision("BAD")
            .Build();
        var x2 = rr.RegisterRule(rule2);

        Assert.NotNull(rule1);
        Assert.IsType<RuleRegistrationResult>(rule1);

        Assert.NotNull(x2);
        Assert.IsType<RuleRegistrationResult>(x2);
    }

    [Fact]
    public void TestBuildAndRegister_First()
    {
        IRightRule rr = new RightRule.RightRule(new RightRuleOptions() { DataCheck = true });

        rr.RegisterData("a", 100);
        rr.RegisterData("b", 100);

        string context = "Context";

        var rule1 = rr.BuildRule()
            .Number(1)
            .Name("Rule1")
            .Context(context)
            .Statement("a <> b")
            .Decision("GOOD")
            .FalseDecision("BAD")
            .DecisionData(null)
            .OptionalData(null)
            .Build();
        var rule1reg = rr.RegisterRule(rule1);

        var rule2 = rr.BuildRule()
            .Number(2)
            .Name("Name2")
            .Context(context)
            .Statement("a = b")
            .Decision("GOOD")
            .FalseDecision("BAD")
            .Build();
        var rule2reg = rr.RegisterRule(rule2);

        Assert.NotNull(rule1);
        Assert.IsType<RuleRegistrationResult>(rule1reg);

        Assert.NotNull(rule2);
        Assert.IsType<RuleRegistrationResult>(rule2reg);

        var result = rr.ApplyRules(context, EvaluationTypeEnum.FIRST);
        Assert.Single(result);
        Assert.Equal(result[0].Decision, rule2reg?.RegisteredRule?.Decision);
    }

    [Fact]
    public void TestBuildAndRegister_Any()
    {
        IRightRule rr = new RightRule.RightRule(new RightRuleOptions() { DataCheck = true });

        rr.RegisterData("a", 100);
        rr.RegisterData("b", 100);

        string context = "Context";

        var rule1 = rr.BuildRule()
            .Number(1)
            .Name("Rule1")
            .Context(context)
            .Statement("a <> b")
            .Decision("GOOD")
            .FalseDecision("BAD")
            .DecisionData(null)
            .OptionalData(null)
            .Build();
        var rule1reg = rr.RegisterRule(rule1);

        var rule2 = rr.BuildRule()
            .Number(2)
            .Name("Name2")
            .Context(context)
            .Statement("a = b")
            .Decision("GOOD")
            .FalseDecision("BAD")
            .Build();
        var rule2reg = rr.RegisterRule(rule2);

        Assert.NotNull(rule1);
        Assert.IsType<RuleRegistrationResult>(rule1reg);

        Assert.NotNull(rule2);
        Assert.IsType<RuleRegistrationResult>(rule2reg);

        var result = rr.ApplyRules(context, EvaluationTypeEnum.ANY);
        Assert.Equal(2, result.Count);
        Assert.Equal(result[0].Decision, rule1reg?.RegisteredRule?.FalseDecision);
        Assert.Equal(result[1].Decision, rule2reg?.RegisteredRule?.Decision);
    }

    [Fact]
    public void TestBuildAndRegister_All_Success()
    {
        IRightRule rr = new RightRule.RightRule(new RightRuleOptions() { DataCheck = true });

        rr.RegisterData("a", 100);
        rr.RegisterData("b", 100);

        string context = "ContextXXX";

        var rule1 = rr.BuildRule()
            .Number(1)
            .Name("Rule1")
            .Context(context)
            .Statement("a = b")
            .Decision("GOOD")
            .FalseDecision("BAD")
            .DecisionData(null)
            .OptionalData(null)
            .Build();
        var rule1reg = rr.RegisterRule(rule1);

        var rule2 = rr.BuildRule()
            .Number(2)
            .Name("Name2")
            .Context(context)
            .Statement("a = b")
            .Decision("GOOD")
            .FalseDecision("BAD")
            .Build();
        var rule2reg = rr.RegisterRule(rule2);

        Assert.NotNull(rule1);
        Assert.IsType<RuleRegistrationResult>(rule1reg);

        Assert.NotNull(rule2);
        Assert.IsType<RuleRegistrationResult>(rule2reg);

        var result = rr.ApplyRules(context, EvaluationTypeEnum.ALL);
        Assert.Equal(2, result.Count);
        Assert.Equal(result[0].Decision, rule1reg?.RegisteredRule?.Decision);
        Assert.Equal(result[1].Decision, rule2reg?.RegisteredRule?.Decision);
    }

    [Fact]
    public void TestBuildAndRegister_All_Fail()
    {
        IRightRule rr = new RightRule.RightRule(new RightRuleOptions() { DataCheck = true });

        rr.RegisterData("a", 100);
        rr.RegisterData("b", 100);

        string context = "Context";

        var rule1 = rr.BuildRule()
            .Number(1)
            .Name("Rule1")
            .Context(context)
            .Statement("a <> b")
            .Decision("GOOD")
            .FalseDecision("BAD")
            .DecisionData(null)
            .OptionalData(null)
            .Build();
        var rule1reg = rr.RegisterRule(rule1);

        var rule2 = rr.BuildRule()
            .Number(2)
            .Name("Name2")
            .Context(context)
            .Statement("a = b")
            .Decision("GOOD")
            .FalseDecision("BAD")
            .Build();
        var rule2reg = rr.RegisterRule(rule2);

        Assert.NotNull(rule1);
        Assert.IsType<RuleRegistrationResult>(rule1reg);

        Assert.NotNull(rule2);
        Assert.IsType<RuleRegistrationResult>(rule2reg);

        var result = rr.ApplyRules(context, EvaluationTypeEnum.ALL);
        Assert.Single(result);
        Assert.Equal(result[0].Decision, rr.GetDefaultRule(context).Decision);
    }
}
