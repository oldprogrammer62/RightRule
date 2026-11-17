using RightRule;
using RightRule.Models;

namespace RightRuleTests;

public class UniqueRegistration_Tests
{
    [Fact]
    public void RegisterSameData_Dict()
    {
        var rr = new RightRule.RightRule().CreateSession();

        Dictionary<string, object> data = new Dictionary<string, object>()
        {
            { "Name", "Hello" }
        };

        var first = rr.DataService.RegisterData("Data1", data);
        Assert.Equal(RightRuleStatusEnum.SUCCESS, first.Status);

        var second = rr.DataService.RegisterData("Data1", data);
        Assert.Equal(RightRuleStatusEnum.SUCCESS, second.Status);

        Assert.Single(rr.DataService.GetRegisteredData());
    }

    [Fact]
    public void RegisterDiffData_Dict()
    {
        var rr = new RightRule.RightRule().CreateSession();

        Dictionary<string, object> data = new Dictionary<string, object>()
        {
            { "Name", "Hello" }
        };

        var first = rr.DataService.RegisterData("Data1", data);
        Assert.Equal(RightRuleStatusEnum.SUCCESS, first.Status);

        var second = rr.DataService.RegisterData("Data2", data);
        Assert.Equal(RightRuleStatusEnum.SUCCESS, second.Status);

        Assert.Equal(2, rr.DataService.GetRegisteredData().Count);
    }

    [Fact]
    public void RegisterSameData_Object()
    {
        var rr = new RightRule.RightRule().CreateSession();

        var data = 100;

        var first = rr.DataService.RegisterData(nameof(data), data);
        Assert.Equal(RightRuleStatusEnum.SUCCESS, first.Status);

        var second = rr.DataService.RegisterData(nameof(data), data);
        Assert.Equal(RightRuleStatusEnum.SUCCESS, second.Status);

        Assert.Single(rr.DataService.GetRegisteredData());
    }
}
