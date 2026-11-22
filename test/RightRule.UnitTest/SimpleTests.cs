using RightRule;
using RightRule.Models;

namespace RightRuleTests;

public class SimpleTests
{
    [Fact]
    public void WithDataCheck()
    {
        var rr = new RightRule.RightRule(new RightRuleOptions() { DataCheck = true }).CreateSession();
        var ds = rr.DataService.RegisterData("a", 100);

        string context = "Context";
        var rule = rr.RuleService.BuildRule()
            .Number(1)
            .Name("Name")
            .Context(context)
            .Statement("a.a = 100")
            .TrueDecision("GOOD")
            .Build();

        rr.RuleService.RegisterRule(rule);

        var result = rr.RuleService.ApplyRule(context);
        Assert.NotNull(result);
    }

    [Fact]
    public void SimpleTest()
    {
        var rr = new RightRule.RightRule(new RightRuleOptions() { DataCheck = true }).CreateSession();
        Dictionary<string, object> data = new Dictionary<string, object>()
        {
            {"a", 100 }
        };
        var ds = rr.DataService.RegisterData("data", data);

        string context = "Context";
        var rule = rr.RuleService.BuildRule()
            .Number(1)
            .Name("Name")
            .Context(context)
            .Statement("data.a = 100")
            .TrueDecision("GOOD")
            .Build();

        rr.RuleService.RegisterRule(rule);

        var result = rr.RuleService.ApplyRule(context);
        Assert.NotNull(result);
    }

    [Fact]
    public void Test1()
    {
        var rr = new RightRule.RightRule(new RightRuleOptions() { DataCheck = true }).CreateSession();
        var rule1 = rr.RuleService.BuildRule()
            .Number(1)
            .Name("TestRule1")
            .Context("TestContext")
            .Statement("1 = 1 and 2 <> 3")
            .TrueDecision("SAME")
            .Build();
        var rule1Result = rr.RuleService.RegisterRule(rule1);

        var result = rr.RuleService.ApplyRule("TestContext");
        Assert.NotNull(result);
    }

    [Fact]
    public void TestWithDefault()
    {
        var rr = new RightRule.RightRule().CreateSession();
        var rule1 = rr.RuleService.BuildRule()
            .Number(1)
            .Name("TestRule1")
            .Context("TestContext")
            .Statement("1 = 1 and 2 <> 2")
            .TrueDecision("SAME")
            .Build();
        var rule1Result = rr.RuleService.RegisterRule(rule1);

        var result = rr.RuleService.ApplyRule("TestContext");
        Assert.NotNull(result);
    }

    [Fact]
    public void Test_WithError_Rule()
    {
        var rr = new RightRule.RightRule().CreateSession();
        var rule1 = rr.RuleService.BuildRule()
            .Number(1)
            .Name("TestRule1")
            .Context("TestContext")
            .Statement("1 = 1 and 2 <> 3")
            .TrueDecision("GOOD")
            .Build();
        var rule1Result = rr.RuleService.RegisterRule(rule1);

        var rule2 = rr.RuleService.BuildRule()
            .Number(2)
            .Name("TestRule2")
            .Context("TestContext")
            .Statement("1 <> 1 and 2 <> 3")
            .TrueDecision("GOOD")
            .Build();
        var rule2Result = rr.RuleService.RegisterRule(rule2);

        var rule3 = rr.RuleService.BuildRule()
            .Number(3)
            .Name("TestRule3")
            .Context("TestContext")
            .Statement("1 <>  and 2 <> 3")
            .TrueDecision("GOOD")
            .Build();
        var rule3Result = rr.RuleService.RegisterRule(rule3);

        var result = rr.RuleService.ApplyRule("TestContext");
        Assert.NotNull(result);
    }
}
