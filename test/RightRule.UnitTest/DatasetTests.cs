using RightRule;
using RightRule.DataServices.Data;

namespace RightRuleTests;

public class DatasetTests
{
    Dictionary<string, object> Data1 = new Dictionary<string, object>();
    Dictionary<string, object> Data2 = new Dictionary<string, object>();

    public DatasetTests()
    {
        Data1 = new Dictionary<string, object>()
        {
            { "name", "John" }
        };
        Data2 = new Dictionary<string, object>()
        {
            { "name", "Sarah" }
        };
    }

    [Fact]
    public void GetRegisteredDataset1()
    {
        IRightRule rr = new RightRule.RightRule();
        rr.RegisterData("Data1", Data1);
        rr.RegisterData("Data2", Data2);
        Assert.Equal(2, rr.GetRegisteredData().Count);

        var ds = rr.GetRegisteredData("Data1");
        Assert.NotNull(ds);

        DataItem ds_name = ds.DataItems[0];
        var data_name = (string)Data1["name"];
        Assert.Equal(ds_name.Value, data_name);
    }

    [Fact]
    public void GetRegisteredDataset2()
    {
        IRightRule rr = new RightRule.RightRule();
        rr.RegisterData("Data1", Data1);
        rr.RegisterData("Data2", Data2);
        Assert.Equal(2, rr.GetRegisteredData().Count);

        var ds = rr.GetRegisteredData("Data2");
        Assert.NotNull(ds);

        DataItem ds_name = ds.DataItems[0];
        var data_name = (string)Data2["name"];
        Assert.Equal(ds_name.Value, data_name);
    }

    [Fact]
    public void GetRegisteredDatasets()
    {
        IRightRule rr = new RightRule.RightRule();
        rr.RegisterData("Data1", Data1);
        rr.RegisterData("Data2", Data2);
        Assert.Equal(2, rr.GetRegisteredData().Count);

        var ds = rr.GetRegisteredData();
        Assert.NotNull(ds);
        Assert.Equal(2, ds.Count);
    }

    [Fact]
    public void GetRegisteredDatasets_Indexed()
    {
        IRightRule rr = new RightRule.RightRule();
        rr.RegisterData("Data1", Data1);
        rr.RegisterData("Data2", Data2);
        Assert.Equal(2, rr.GetRegisteredData().Count);

        var ds = rr.GetRegisteredData();
        Assert.NotNull(ds);
        Assert.Equal(2, ds.Count);

        var ds1 = ds[0];
        Assert.NotNull(ds1);
    }

    [Fact]
    public void DeregisterDataset1()
    {
        IRightRule rr = new RightRule.RightRule();
        rr.RegisterData("Data1", Data1);
        rr.RegisterData("Data2", Data2);
        Assert.Equal(2, rr.GetRegisteredData().Count);

        var dsBefore = rr.GetRegisteredData("Data1").DatasetName;
        Assert.NotNull(dsBefore);
        Assert.Equal("Data1", dsBefore);

        var ds2Before = rr.GetRegisteredData("Data2").DatasetName;
        Assert.NotNull(ds2Before);
        Assert.Equal("Data2", ds2Before);

        var done = rr.DeRegisterData("Data1");
        Assert.True(done);
        Assert.Single(rr.GetRegisteredData());

        var ds1After = rr.GetRegisteredData("Data1");
        Assert.Null(ds1After);

        var ds2After = rr.GetRegisteredData("Data2");
        Assert.NotNull(ds2After);
    }

    [Fact]
    public void DeregisterDataset2()
    {
        IRightRule rr = new RightRule.RightRule();
        rr.RegisterData("Data1", Data1);
        rr.RegisterData("Data2", Data2);
        Assert.Equal(2, rr.GetRegisteredData().Count);

        var ds1Before = rr.GetRegisteredData("Data1");
        Assert.NotNull(ds1Before);
        Assert.Equal("Data1", ds1Before.DatasetName);

        var ds2Before = rr.GetRegisteredData("Data2");
        Assert.NotNull(ds2Before);
        Assert.Equal("Data2", ds2Before.DatasetName);

        var done = rr.DeRegisterData("Data2");
        Assert.True(done);
        Assert.Single(rr.GetRegisteredData());

        var ds1After = rr.GetRegisteredData("Data1");
        Assert.NotNull(ds1After);

        var ds2After = rr.GetRegisteredData("Data2");
        Assert.Null(ds2After);
    }

    [Fact]
    public void DeregisterDatasets()
    {
        IRightRule rr = new RightRule.RightRule();
        rr.RegisterData("Data1", Data1);
        rr.RegisterData("Data2", Data2);
        Assert.Equal(2, rr.GetRegisteredData().Count);

        var ds1Before = rr.GetRegisteredData("Data1");
        Assert.NotNull(ds1Before);
        Assert.Equal("Data1", ds1Before.DatasetName);

        var ds2Before = rr.GetRegisteredData("Data2");
        Assert.NotNull(ds2Before);
        Assert.Equal("Data2", ds2Before.DatasetName);

        var done = rr.DeRegisterData();
        Assert.True(done);
        Assert.Empty(rr.GetRegisteredData());

        var ds1After = rr.GetRegisteredData("Data1");
        Assert.Null(ds1After);

        var ds2After = rr.GetRegisteredData("Data2");
        Assert.Null(ds2After);
    }
}
