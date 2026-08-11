using BeSoft;

namespace RightRuleTests;

public class DataDeregistrationTests
{
    [Fact]
    public void DeregisterDataObject()
    {
        var d = new ObjRef();
        var d2 = new ObjRef();

        IRightRule rr = new BeSoft.RightRule();
        rr.RegisterData("d", d);
        rr.RegisterData("d2", d2);
        Assert.Equal(2, rr.RegisteredDataObjects().Count);

        rr.DeRegisterData("d");
        Assert.Equal(1, rr.RegisteredDataObjects().Count);
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
        IRightRule rr = new BeSoft.RightRule();
        rr.RegisterData("d", d);
        rr.RegisterData("d2", d2);
        Assert.Equal(2, rr.RegisteredDatasets().Count);

        rr.DeRegisterData("d");
        Assert.Equal(1, rr.RegisteredDatasets().Count);
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
