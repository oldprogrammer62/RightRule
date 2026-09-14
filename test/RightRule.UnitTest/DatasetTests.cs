using BeSoft;
using RightRule.RegistrationServices.DataRegistration;

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
        Assert.Equal(2, rr.newGetRegisteredData().Count);

        var ds = rr.newGetRegisteredData("Data1");
        Assert.NotNull(ds);

        newDataItem ds_name = ds.Data.DataItems[0];
        var data_name = (string)Data1["name"];
        Assert.Equal(ds_name.Value, data_name);
    }

    [Fact]
    public void GetRegisteredDataset2()
    {
        IRightRule rr = new BeSoft.RightRule();
        rr.RegisterData("Data1", Data1);
        rr.RegisterData("Data2", Data2);
        Assert.Equal(2, rr.newGetRegisteredData().Count);

        var ds = rr.newGetRegisteredData("Data2");
        //Assert.NotNull(ds);

        newDataItem ds_name = ds.Data.DataItems[0];
        var data_name = (string)Data2["name"];
        Assert.Equal(ds_name.Value, data_name);
    }

    [Fact]
    public void GetRegisteredDatasets()
    {
        IRightRule rr = new BeSoft.RightRule();
        rr.RegisterData("Data1", Data1);
        rr.RegisterData("Data2", Data2);
        Assert.Equal(2, rr.newGetRegisteredData().Count);

        var ds = rr.newGetRegisteredData();
        Assert.NotNull(ds);
        Assert.Equal(2, ds.Count);
    }

    [Fact]
    public void GetRegisteredDatasets_Indexed()
    {
        IRightRule rr = new BeSoft.RightRule();
        rr.RegisterData("Data1", Data1);
        rr.RegisterData("Data2", Data2);
        Assert.Equal(2, rr.newGetRegisteredData().Count);

        var ds = rr.newGetRegisteredData();
        Assert.NotNull(ds);
        Assert.Equal(2, ds.Count);

        var ds1 = ds[0];
        Assert.NotNull(ds1);
    }

    [Fact]
    public void DeregisterDataset1()
    {
        IRightRule rr = new BeSoft.RightRule();
        rr.RegisterData("Data1", Data1);
        rr.RegisterData("Data2", Data2);
        Assert.Equal(2, rr.newGetRegisteredData().Count);

        var dsBefore = rr.newGetRegisteredData("Data1").DataName;
        Assert.NotNull(dsBefore);
        Assert.Equal("Data1", dsBefore);

        var ds2Before = rr.newGetRegisteredData("Data2").DataName;
        Assert.NotNull(ds2Before);
        Assert.Equal("Data2", ds2Before);

        var done = rr.DeRegisterData("Data1");
        Assert.True(done);
        Assert.Single(rr.newGetRegisteredData());

        var ds1After = rr.newGetRegisteredData("Data1");
        Assert.Null(ds1After);

        var ds2After = rr.newGetRegisteredData("Data2");
        Assert.NotNull(ds2After);
    }

    [Fact]
    public void DeregisterDataset2()
    {
        IRightRule rr = new BeSoft.RightRule();
        rr.RegisterData("Data1", Data1);
        rr.RegisterData("Data2", Data2);
        Assert.Equal(2, rr.newGetRegisteredData().Count);

        var ds1Before = rr.newGetRegisteredData("Data1");
        Assert.NotNull(ds1Before);
        Assert.Equal("Data1", ds1Before.DataName);

        var ds2Before = rr.newGetRegisteredData("Data2");
        Assert.NotNull(ds2Before);
        Assert.Equal("Data2", ds2Before.DataName);

        var done = rr.DeRegisterData("Data2");
        Assert.True(done);
        Assert.Single(rr.newGetRegisteredData());

        var ds1After = rr.newGetRegisteredData("Data1");
        Assert.NotNull(ds1After);

        var ds2After = rr.newGetRegisteredData("Data2");
        Assert.Null(ds2After);
    }

    [Fact]
    public void DeregisterDatasets()
    {
        IRightRule rr = new BeSoft.RightRule();
        rr.RegisterData("Data1", Data1);
        rr.RegisterData("Data2", Data2);
        Assert.Equal(2, rr.newGetRegisteredData().Count);

        var ds1Before = rr.newGetRegisteredData("Data1");
        Assert.NotNull(ds1Before);
        Assert.Equal("Data1", ds1Before.DataName);

        var ds2Before = rr.newGetRegisteredData("Data2");
        Assert.NotNull(ds2Before);
        Assert.Equal("Data2", ds2Before.DataName);

        var done = rr.DeRegisterData();
        Assert.True(done);
        Assert.Empty(rr.newGetRegisteredData());

        var ds1After = rr.newGetRegisteredData("Data1");
        Assert.Null(ds1After);

        var ds2After = rr.newGetRegisteredData("Data2");
        Assert.Null(ds2After);
    }
}
