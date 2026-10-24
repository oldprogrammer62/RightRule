using RightRule;
using RightRule.Models;

namespace RightRuleTests;

public class DataCheckOption_Tests
{
    [Fact]
    public void DataCheck_False()
    {
        RightRuleOptions options = new RightRuleOptions { LoadDefaultOperators = true, DataCheck = false };

        IRightRule rr = new RightRule.RightRule(options);

        var rule1 = rr.BuildRule()
            .Number(1)
            .Name("Rule1")
            .Context("Context")
            .Statement("100 = 100")
            .TrueDecision("Same")
            .FalseDecision("Different")
            .Build()
            .Register(rr.RuleService);

        Assert.Equal(RightRuleStatusEnum.SUCCESS, rule1.Status);
    }

    [Fact]
    public void DataCheck_True()
    {
        RightRuleOptions options = new RightRuleOptions 
        { 
            LoadDefaultOperators = true, 
            DataCheck = true 
        };

        IRightRule rr = new RightRule.RightRule(options);

        var rule1 = rr.BuildRule()
            .Number(1)
            .Name("Rule1")
            .Context("Context")
            .Statement("a = 100")
            .TrueDecision("Same")
            .FalseDecision("Different")
            .Build()
            .Register(rr.RuleService);

        Assert.Equal(RightRuleStatusEnum.ERROR, rule1.Status);
    }
}
