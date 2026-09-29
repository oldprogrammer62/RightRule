using RightRule;
using RightRule.RuleServices.RuleRegistration;

namespace RightRuleTests;

public class UniqueRegistration_Tests
{
    [Fact]
    public void RegisterSameData_Dict()
    {
        IRightRule rr = new RightRule.RightRule();

        Dictionary<string, object> data = new Dictionary<string, object>()
        {
            { "Name", "Hello" }
        };

        var first = rr.RegisterData("Data1", data);
        Assert.Equal(RegistrationStatusEnum.SUCCESS, first.Status);

        var second = rr.RegisterData("Data1", data);
        Assert.Equal(RegistrationStatusEnum.SUCCESS, second.Status);

        Assert.Single(rr.GetRegisteredData());
    }

    [Fact]
    public void RegisterDiffData_Dict()
    {
        IRightRule rr = new RightRule.RightRule();

        Dictionary<string, object> data = new Dictionary<string, object>()
        {
            { "Name", "Hello" }
        };

        var first = rr.RegisterData("Data1", data);
        Assert.Equal(RegistrationStatusEnum.SUCCESS, first.Status);

        var second = rr.RegisterData("Data2", data);
        Assert.Equal(RegistrationStatusEnum.SUCCESS, second.Status);

        Assert.Equal(2, rr.GetRegisteredData().Count);
    }

    [Fact]
    public void RegisterSameData_Object()
    {
        IRightRule rr = new RightRule.RightRule();

        var data = 100;

        var first = rr.RegisterData("Data1", nameof(data), data);
        Assert.Equal(RegistrationStatusEnum.SUCCESS, first.Status);

        var second = rr.RegisterData("Data1", nameof(data), data);
        Assert.Equal(RegistrationStatusEnum.SUCCESS, second.Status);

        Assert.Single(rr.GetRegisteredData());
    }
}
