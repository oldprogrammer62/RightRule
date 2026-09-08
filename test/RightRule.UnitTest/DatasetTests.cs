using BeSoft;

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
        IRightRule rr = new BeSoft.RightRule();
        rr.RegisterData("Data1", Data1);
        rr.RegisterData("Data2", Data2);
        Assert.Equal(2, rr.GetRegisteredDatasets().Count);

        var ds = rr.GetRegisteredDataset("Data1");
        Assert.NotNull(ds);

        var ds_name = (string)ds.Value["name"];
        var data_name = (string)Data1["name"];
        Assert.Equal(ds_name, data_name);
    }

    [Fact]
    public void GetRegisteredDataset2()
    {
        IRightRule rr = new BeSoft.RightRule();
        rr.RegisterData("Data1", Data1);
        rr.RegisterData("Data2", Data2);
        Assert.Equal(2, rr.GetRegisteredDatasets().Count);

        var ds = rr.GetRegisteredDataset("Data2");
        Assert.NotNull(ds);

        var ds_name = (string)ds.Value["name"];
        var data_name = (string)Data2["name"];
        Assert.Equal(ds_name, data_name);
    }

    [Fact]
    public void GetRegisteredDatasets()
    {
        IRightRule rr = new BeSoft.RightRule();
        rr.RegisterData("Data1", Data1);
        rr.RegisterData("Data2", Data2);
        Assert.Equal(2, rr.GetRegisteredDatasets().Count);

        var ds = rr.GetRegisteredDatasets;
        Assert.NotNull(ds);
        Assert.Equal(2, ds().Count);
    }

    [Fact]
    public void GetRegisteredDatasets_Indexed()
    {
        IRightRule rr = new BeSoft.RightRule();
        rr.RegisterData("Data1", Data1);
        rr.RegisterData("Data2", Data2);
        Assert.Equal(2, rr.GetRegisteredDatasets().Datasets.Count);

        var ds = rr.GetRegisteredDatasets();
        Assert.NotNull(ds);
        Assert.Equal(2, ds.Datasets.Count);

        var ds1 = ds[0];
        Assert.NotNull(ds1);
    }

    [Fact]
    public void DeregisterDataset1()
    {
        IRightRule rr = new BeSoft.RightRule();
        rr.RegisterData("Data1", Data1);
        rr.RegisterData("Data2", Data2);
        Assert.Equal(2, rr.GetRegisteredDatasets().Count);

        var dsBefore = rr.GetRegisteredDataset("Data1");
        Assert.NotNull(dsBefore);
        Assert.Equal("Data1", dsBefore.Name);

        var ds2Before = rr.GetRegisteredDataset("Data2");
        Assert.NotNull(ds2Before);
        Assert.Equal("Data2", ds2Before.Name);

        var done = rr.DeRegisterData("Data1");
        Assert.True(done);
        Assert.Equal(1, rr.GetRegisteredDatasets().Count);

        var ds1After = rr.GetRegisteredDataset("Data1");
        Assert.Null(ds1After);

        var ds2After = rr.GetRegisteredDataset("Data2");
        Assert.NotNull(ds2After);
    }

    [Fact]
    public void DeregisterDataset2()
    {
        IRightRule rr = new BeSoft.RightRule();
        rr.RegisterData("Data1", Data1);
        rr.RegisterData("Data2", Data2);
        Assert.Equal(2, rr.GetRegisteredDatasets().Count);

        var ds1Before = rr.GetRegisteredDataset("Data1");
        Assert.NotNull(ds1Before);
        Assert.Equal("Data1", ds1Before.Name);

        var ds2Before = rr.GetRegisteredDataset("Data2");
        Assert.NotNull(ds2Before);
        Assert.Equal("Data2", ds2Before.Name);

        var done = rr.DeRegisterData("Data2");
        Assert.True(done);
        Assert.Equal(1, rr.GetRegisteredDatasets().Count);

        var ds1After = rr.GetRegisteredDataset("Data1");
        Assert.NotNull(ds1After);

        var ds2After = rr.GetRegisteredDataset("Data2");
        Assert.Null(ds2After);
    }

    [Fact]
    public void DeregisterDatasets()
    {
        IRightRule rr = new BeSoft.RightRule();
        rr.RegisterData("Data1", Data1);
        rr.RegisterData("Data2", Data2);
        Assert.Equal(2, rr.GetRegisteredDatasets().Count);

        var ds1Before = rr.GetRegisteredDataset("Data1");
        Assert.NotNull(ds1Before);
        Assert.Equal("Data1", ds1Before.Name);

        var ds2Before = rr.GetRegisteredDataset("Data2");
        Assert.NotNull(ds2Before);
        Assert.Equal("Data2", ds2Before.Name);

        var done = rr.DeRegisterData();
        Assert.True(done);
        Assert.Equal(0, rr.GetRegisteredDatasets().Count);

        var ds1After = rr.GetRegisteredDataset("Data1");
        Assert.Null(ds1After);

        var ds2After = rr.GetRegisteredDataset("Data2");
        Assert.Null(ds2After);
    }
}
