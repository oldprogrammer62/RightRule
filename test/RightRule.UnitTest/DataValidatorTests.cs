using RightRule;
using RightRule.RuleServices.RuleRegistration;

namespace RightRuleTests;

public class DataValidatorTests
{
    [Fact]
    public void ValidatorTest_Success()
    {
        IRightRule rr = new RightRule.RightRule();

        var r1 = rr.RegisterData("a", 100);
        Assert.Equal(RegistrationStatusEnum.SUCCESS, r1.Status);

        var r2 = rr.RegisterData("b", 100);
        Assert.Equal(RegistrationStatusEnum.SUCCESS, r2.Status);

        Dictionary<string, object> data = new Dictionary<string, object>()
        {
            {"a", 100},
            {"b", 100}
        };
        var r3 = rr.RegisterData("Data", data);
        Assert.Equal(RegistrationStatusEnum.SUCCESS, r3.Status);

        var rule1 = rr.BuildRule()
            .Number(1)
            .Name("Name1")
            .Context("Context")
            .Statement("a = 100 and b.b = 100 and Data.a = 100 and Data.b = 100")
            .Decision("Decision1")
            .FalseDecision("FalseDecision1")
            .WithDataCheck(true)
            .Build();
        var rule1reg = rr.RegisterRule(rule1);
        Assert.Equal(RegistrationStatusEnum.SUCCESS, rule1reg.Status);
    }

    [Fact]
    public void ValidatorTest_Fail()
    {
        IRightRule rr = new RightRule.RightRule();

        var r1 = rr.RegisterData("a", 100);
        Assert.Equal(RegistrationStatusEnum.SUCCESS, r1.Status);

        var r2 = rr.RegisterData("b", 100);
        Assert.Equal(RegistrationStatusEnum.SUCCESS, r2.Status);

        Dictionary<string, object> data = new Dictionary<string, object>()
        {
            {"a", 100},
            {"b", 100}
        };
        var r3 = rr.RegisterData("Data", data);
        Assert.Equal(RegistrationStatusEnum.SUCCESS, r3.Status);

        var rule1 = rr.BuildRule()
            .Number(1)
            .Name("Name1")
            .Context("Context")
            .Statement("a = 100 and b.b = 100 and Data.a = 100 and Data.b = 100 and NotFound = 10")
            .Decision("Decision1")
            .FalseDecision("FalseDecision1")
            .WithDataCheck(true)
            .Build();
        var rule1reg = rr.RegisterRule(rule1);
        Assert.Equal(RegistrationStatusEnum.FAILURE, rule1reg.Status);
        Assert.Contains("NotFound", rule1reg.Message);
    }
}
