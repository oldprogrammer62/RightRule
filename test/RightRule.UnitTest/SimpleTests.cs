using RightRule;
using RightRule.Models;

namespace RightRuleTests;

public class SimpleTests
{
    [Fact]
    public void WithDataCheck()
    {
        IRightRule rr = new RightRule.RightRule(new RightRuleOptions() { DataCheck = true });
        var ds = rr.RegisterData("a", 100);

        string context = "Context";
        var rule = rr.BuildRule()
            .Number(1)
            .Name("Name")
            .Context(context)
            .Statement("a.a = 100")
            .Decision("GOOD")
            .FalseDecision("BAD")
            .Build();

        rr.RegisterRule(rule);

        var result = rr.ApplyRule(context);
        Assert.NotNull(result);
    }

    [Fact]
    public void SimpleTest()
    {
        IRightRule rr = new RightRule.RightRule(new RightRuleOptions() { DataCheck = true });
        Dictionary<string, object> data = new Dictionary<string, object>()
        {
            {"a", 100 }
        };
        var ds = rr.RegisterData("data", data);

        string context = "Context";
        var rule = rr.BuildRule()
            .Number(1)
            .Name("Name")
            .Context(context)
            .Statement("data.a = 100")
            .Decision("GOOD")
            .FalseDecision("BAD")
            .Build();

        rr.RegisterRule(rule);

        var result = rr.ApplyRule(context);
        Assert.NotNull(result);
    }

    [Fact]
    public void Test1()
    {
        IRightRule rr = new RightRule.RightRule(new RightRuleOptions() { DataCheck = true });
        var rule1 = rr.BuildRule()
            .Number(1)
            .Name("TestRule1")
            .Context("TestContext")
            .Statement("1 = 1 and 2 <> 3")
            .Decision("SAME")
            .FalseDecision("NOTSAME")
            .Build();
        var rule1Result = rr.RegisterRule(rule1);

        var result = rr.ApplyRule("TestContext");
        Assert.NotNull(result);
    }

    [Fact]
    public void TestWithDefault()
    {
        IRightRule rr = new RightRule.RightRule();
        var rule1 = rr.BuildRule()
            .Number(1)
            .Name("TestRule1")
            .Context("TestContext")
            .Statement("1 = 1 and 2 <> 2")
            .Decision("SAME")
            .FalseDecision("NOTSAME")
            .Build();
        var rule1Result = rr.RegisterRule(rule1);

        var result = rr.ApplyRule("TestContext");
        Assert.NotNull(result);
    }

    [Fact]
    public void Test_WithError_Rule()
    {
        IRightRule rr = new RightRule.RightRule();
        var rule1 = rr.BuildRule()
            .Number(1)
            .Name("TestRule1")
            .Context("TestContext")
            .Statement("1 = 1 and 2 <> 3")
            .Decision("GOOD")
            .FalseDecision("NOTGOOD")
            .Build();
        var rule1Result = rr.RegisterRule(rule1);

        var rule2 = rr.BuildRule()
            .Number(2)
            .Name("TestRule2")
            .Context("TestContext")
            .Statement("1 <> 1 and 2 <> 3")
            .Decision("GOOD")
            .FalseDecision("NOTGOOD")
            .Build();
        var rule2Result = rr.RegisterRule(rule2);

        var rule3 = rr.BuildRule()
            .Number(3)
            .Name("TestRule3")
            .Context("TestContext")
            .Statement("1 <>  and 2 <> 3")
            .Decision("GOOD")
            .FalseDecision("NOTGOOD")
            .Build();
        var rule3Result = rr.RegisterRule(rule3);

        var result = rr.ApplyRule("TestContext");
        Assert.NotNull(result);
    }
}
