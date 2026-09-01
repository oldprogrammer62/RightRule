using BeSoft;

namespace RightRuleTests;

public class SimpleTests
{
    [Fact]
    public void Test1()
    {
        IRightRule rr = new BeSoft.RightRule();
        var rule1 = rr.BuildRuleDefinition()
            .Number(1)
            .Name("TestRule1")
            .Context("TestContext")
            .Statement("1 = 1 and 2 <> 3")
            .Decision("SAME")
            .FalseDecision("NOTSAME")
            .Build();
        var rule1Result = rr.RegisterRule(rule1);

        var result = rr.EvaluateFirstMatch("TestContext");
        Assert.NotNull(result);
    }

    [Fact]
    public void Test_WithError_Rule()
    {
        IRightRule rr = new BeSoft.RightRule();
        var rule1 = rr.BuildRuleDefinition()
            .Number(1)
            .Name("TestRule1")
            .Context("TestContext")
            .Statement("1 = 1 and 2 <> 3")
            .Decision("GOOD")
            .FalseDecision("NOTGOOD")
            .Build();
        var rule1Result = rr.RegisterRule(rule1);

        var rule2 = rr.BuildRuleDefinition()
            .Number(2)
            .Name("TestRule2")
            .Context("TestContext")
            .Statement("1 <> 1 and 2 <> 3")
            .Decision("GOOD")
            .FalseDecision("NOTGOOD")
            .Build();
        var rule2Result = rr.RegisterRule(rule2);

        var rule3 = rr.BuildRuleDefinition()
            .Number(3)
            .Name("TestRule3")
            .Context("TestContext")
            .Statement("1 <>  and 2 <> 3")
            .Decision("GOOD")
            .FalseDecision("NOTGOOD")
            .Build();
        var rule3Result = rr.RegisterRule(rule3);

        var result = rr.EvaluateAll("TestContext");
        Assert.NotNull(result);
    }
}
