using RightRule;
using RightRule.Models;

namespace RightRuleTests;

public class DataCheckOption_Tests
{
    [Fact]
    public void DataCheck_False()
    {
        RightRuleOptions options = new RightRuleOptions { LoadDefaultOperators = true, DataCheck = false };

        var rr = new RightRule.RightRule(options).CreateSession();

        var rule1 = rr.RuleService.BuildRule()
            .Number(1)
            .Name("Rule1")
            .Context("Context")
            .Statement("100 = 100")
            .Decision("Same")
            .Build()
            .Register(rr.RuleService);

        Assert.Equal(RegistrationStatusEnum.SUCCESS, rule1.Status);
    }

    [Fact]
    public void DataCheck_True_NoData_Error()
    {
        RightRuleOptions options = new RightRuleOptions 
        { 
            LoadDefaultOperators = true, 
            DataCheck = true 
        };

        var rr = new RightRule.RightRule(options).CreateSession();

        var rule1 = rr.RuleService.BuildRule()
            .Number(1)
            .Name("Rule1")
            .Context("Context")
            .Statement("a = 100")
            .Decision("Same")
            .Build()
            .Register(rr.RuleService);

        Assert.Equal(RegistrationStatusEnum.ERROR, rule1.Status);
    }
}
