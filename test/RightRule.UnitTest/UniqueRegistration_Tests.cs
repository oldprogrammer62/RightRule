using BeSoft;

namespace RightRuleTests;

public class UniqueRegistration_Tests
{
    [Fact]
    public void RegisterSameData_Dict()
    {
        IRightRule rr = new BeSoft.RightRule();

        Dictionary<string, object> data = new Dictionary<string, object>()
        {
            { "Name", "Hello" }
        };

        var first = rr.RegisterData("Data1", data);
        Assert.True(first.Success);

        var second = rr.RegisterData("Data1", data);
        Assert.False(second.Success);

        Assert.Equal(1, rr.GetRegisteredDatasets().Count);
    }

    [Fact]
    public void RegisterDiffData_Dict()
    {
        IRightRule rr = new BeSoft.RightRule();

        Dictionary<string, object> data = new Dictionary<string, object>()
        {
            { "Name", "Hello" }
        };

        var first = rr.RegisterData("Data1", data);
        Assert.True(first.Success);

        var second = rr.RegisterData("Data2", data);
        Assert.True(second.Success);

        Assert.Equal(2, rr.GetRegisteredDatasets().Count);
    }

    [Fact]
    public void RegisterSameData_Object()
    {
        IRightRule rr = new BeSoft.RightRule();

        var data = 100;

        var first = rr.RegisterData("Data1", data);
        Assert.True(first.Success);

        var second = rr.RegisterData("Data1", data);
        Assert.False(second.Success);

        Assert.Equal(1, rr.GetRegisteredDataObjects().Count);
    }
}
