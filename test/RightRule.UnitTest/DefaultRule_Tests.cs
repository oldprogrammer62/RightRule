using BeSoft;

namespace RightRuleTests;

public class DefaultRule_Tests
{
    [Fact]
    public void DefaultRule1_SingleMatch_NoResult()
    {
        IRightRule rr = new BeSoft.RightRule();

        var ruleDef1 = rr.BuildRuleDefinition()
            .Number(1)
            .Name("Rule1")
            .Context("Test")
            .Statement("11 >= 19")
            .Decision("true")
            .FalseDecision("False")
            .Build();
        var rule1 = rr.RegisterRule(ruleDef1);
        var result1 = rr.ApplyRules("Test");
        Assert.NotNull(result1);
    }

    [Fact]
    public void DefaultRule2()
    {
        IRightRule rr = new BeSoft.RightRule();

        var ruleDef1 = rr.BuildRuleDefinition()
            .Number(1)
            .Name("Rule1")
            .Context("Test")
            .Statement("11 >= 19")
            .Decision("true")
            .FalseDecision("False")
            .Build();
        var rule1 = rr.RegisterRule(ruleDef1);
        var result1 = rr.ApplyRules("Test", RightRule.System.Common.RulesToReturn.All);
        Assert.NotNull(result1);
    }
}
