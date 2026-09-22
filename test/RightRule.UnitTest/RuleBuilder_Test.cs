using RightRule.LanguageProcessor.Evaluator;
using RightRule.RegistrationServices.RuleRegistration;
using RightRule.RegistrationServices.RuleRegistration.RuleBuilders;

namespace RightRuleTests;

public class RuleBuilder_Test
{
    [Fact]
    public void TestBuilder()
    {
        BeSoft.IRightRule rr = new BeSoft.RightRule();

        var rule1 = rr.BuildRule()
            .Number(1)
            .Name("Rule1")
            .Context("Context")
            .Statement("a = b")
            .Decision("GOOD")
            .FalseDecision("BAD")
            .DecisionData(null)
            .OptionalData(null)
            .WithDataCheck(false)
            .WithDiagnostics(false)
            .Build();
        var x1 = rr.RegisterRule(rule1);

        var rule2 = rr.BuildRule()
            .Number(2)
            .Name("Name2")
            .Context("Context")
            .Statement("a = b")
            .Decision("GOOD")
            .FalseDecision("BAD")
            .WithDataCheck(false)
            .WithDiagnostics(true)
            .Build();
        var x2 = rr.RegisterRule(rule2);

        Assert.NotNull(x1);
        Assert.IsType<RuleRegistrationResult>(x1);

        Assert.NotNull(x2);
        Assert.IsType<RuleRegistrationResult>(x2);
    }

    [Fact]
    public void TestBuildAndRegister_First()
    {
        BeSoft.IRightRule rr = new BeSoft.RightRule();

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
            .WithDataCheck(true)
            .WithDiagnostics(true)
            .Build();
        var rule1reg = rr.RegisterRule(rule1);

        var rule2 = rr.BuildRule()
            .Number(2)
            .Name("Name2")
            .Context(context)
            .Statement("a = b")
            .Decision("GOOD")
            .FalseDecision("BAD")
            .WithDataCheck(true)
            .WithDiagnostics(true)
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
        BeSoft.IRightRule rr = new BeSoft.RightRule();

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
            .WithDataCheck(true)
            .WithDiagnostics(true)
            .Build();
        var rule1reg = rr.RegisterRule(rule1);

        var rule2 = rr.BuildRule()
            .Number(2)
            .Name("Name2")
            .Context(context)
            .Statement("a = b")
            .Decision("GOOD")
            .FalseDecision("BAD")
            .WithDataCheck(true)
            .WithDiagnostics(true)
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
        BeSoft.IRightRule rr = new BeSoft.RightRule();

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
            .WithDataCheck(true)
            .WithDiagnostics(true)
            .Build();
        var rule1reg = rr.RegisterRule(rule1);

        var rule2 = rr.BuildRule()
            .Number(2)
            .Name("Name2")
            .Context(context)
            .Statement("a = b")
            .Decision("GOOD")
            .FalseDecision("BAD")
            .WithDataCheck(true)
            .WithDiagnostics(true)
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
        BeSoft.IRightRule rr = new BeSoft.RightRule();

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
            .WithDataCheck(true)
            .WithDiagnostics(true)
            .Build();
        var rule1reg = rr.RegisterRule(rule1);

        var rule2 = rr.BuildRule()
            .Number(2)
            .Name("Name2")
            .Context(context)
            .Statement("a = b")
            .Decision("GOOD")
            .FalseDecision("BAD")
            .WithDataCheck(true)
            .WithDiagnostics(true)
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
