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
        rr.RegisterData("d", nameof(d), d);
        rr.RegisterData("d2", nameof(d2), d2);
        // Assert.Equal(2, rr.GetRegisteredDataObjects().Count);
        Assert.Equal(2, rr.GetRegisteredData().Count);

        rr.DeRegisterData("d");
        // Assert.Equal(1, rr.GetRegisteredDataObjects().Count);
        Assert.Single(rr.GetRegisteredData());
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
        //Assert.Equal(2, rr.GetRegisteredDatasets().Count);
        Assert.Equal(2, rr.GetRegisteredData().Count);

        rr.DeRegisterData("d");
        //Assert.Equal(1, rr.GetRegisteredDatasets().Count);
        Assert.Single(rr.GetRegisteredData());
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
