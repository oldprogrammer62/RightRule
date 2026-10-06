using RightRule;

namespace RightRuleTests;

public class DataDeregistrationTests
{
    [Fact]
    public void DeregisterDataObject()
    {
        var d = "Hello";
        var d2 = "World";

        IRightRule rr = new RightRule.RightRule();
        rr.RegisterData(nameof(d), d);
        rr.RegisterData(nameof(d2), d2);
        var registeredData = rr.GetRegisteredData();
        Assert.NotNull(registeredData);
        Assert.Equal(2, registeredData.Count);

        rr.DeRegisterData("d");
        var deregisteredData = rr.GetRegisteredData();
        Assert.NotNull(deregisteredData);
        Assert.Single(deregisteredData);
    }

    [Fact]
    public void DeregisterDataSet()
    {
        Dictionary<string, object> d = new Dictionary<string, object>()
        {
            { "Name", "Bob" }
        };
        Dictionary<string, object> d2 = new Dictionary<string, object>()
        {
            { "Name", "Harry" }
        };
        IRightRule rr = new RightRule.RightRule();

        rr.RegisterData("d", d);
        rr.RegisterData("d2", d2);
        var data = rr.GetRegisteredData();
        Assert.NotNull(data);
        Assert.Equal(2, data.Count);

        rr.DeRegisterData("d");
        var deregisteredData = rr.GetRegisteredData();
        Assert.NotNull(deregisteredData);
        Assert.Single(deregisteredData);
    }
}

public class  ObjRef
{
    public string Name { get; set; } = string.Empty;
    public int Age { get; set; } = 30;

    public ObjRef()
    {
        Name = "Bob";
        Age = 30;
    }
}
