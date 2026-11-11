using RightRule;
using RightRule.DataServices.DataRegistration;
using RightRule.Models;
using System.Text;

namespace RightRuleTests;

public class DataLoader_Tests
{
    [Fact]
    public void Json_LoadSingleDataset()
    {
        IRightRule rr = new RightRule.RightRule();
        Assert.NotNull(rr);
        Assert.NotNull(rr.DataService);

        var json = SingleDatasetJsonData();
        Assert.NotNull(json);

        List<DataRegistrationResult>? data = rr.LoadData(json);
        Assert.NotNull(data);
        Assert.Single(rr.DataService.GetDatasets());

        var serializedRules = rr.SerializeData();
        Assert.NotNull(serializedRules);
        Assert.Contains("\"DatasetName\":\"Customer\"", serializedRules);
    }

    [Fact]
    public void Json_LoadMultipleDataset()
    {
        IRightRule rr = new RightRule.RightRule();
        Assert.NotNull(rr);
        Assert.NotNull(rr.DataService);

        var json = MultipleDatasetJsonData();
        Assert.NotNull(json);

        List<DataRegistrationResult>? data = rr.LoadData(json);
        Assert.NotNull(data);
        Assert.Equal(2, rr.DataService.GetDatasets().Count);

        var serializedRules = rr.SerializeData();
        Assert.NotNull(serializedRules);
        Assert.Contains("\"DatasetName\":\"Customer1\"", serializedRules);
        Assert.Contains("\"DatasetName\":\"Customer2\"", serializedRules);
    }

    [Fact]
    public void SerializeThenLoadData_MultipleDatasets()
    {
        using var rr = new RightRule.RightRule();
        Assert.NotNull(rr);
        Assert.NotNull(rr.DataService);

        var registration = rr.RegisterData(
            "Customer",
            new Dictionary<string, object>
            {
                ["name"] = "Alice",
                ["age"] = 30,
                ["isActive"] = true
            });
        Assert.Equal(RightRuleStatusEnum.SUCCESS, registration.Status);

        var registration2 = rr.RegisterData(
            "Order",
            new Dictionary<string, object>
            {
                ["orderValue"] = 123.99,
                ["orderDate"] = "01.10.2026",
                ["isPaid"] = true
            });
        Assert.Equal(RightRuleStatusEnum.SUCCESS, registration2.Status);

        Assert.Equal(2, rr.DataService.GetDatasets().Count);

        var serializedData = rr.SerializeData();
        Assert.NotNull(serializedData);
        Assert.Contains("\"DatasetName\":\"Customer\"", serializedData);
        Assert.Contains("\"DatasetName\":\"Order\"", serializedData);

        // Clear all the data from the data service so we can test loading it back in
        rr.DataService.Clear();
        Assert.Empty(rr.DataService.GetDatasets());

        List<DataRegistrationResult>? data = rr.LoadData(serializedData);
        Assert.NotNull(data);
        Assert.Equal(2, rr.DataService.GetDatasets().Count);
        Assert.Equal("Customer", rr.DataService.GetDatasets()[0].DatasetName);
        Assert.Equal("Order", rr.DataService.GetDatasets()[1].DatasetName);
    }

    [Fact]
    public void SerializeThenLoadData_SingleDatasets()
    {
        using var rr = new RightRule.RightRule();
        Assert.NotNull(rr);
        Assert.NotNull(rr.DataService);

        var registration = rr.RegisterData(
            "Customer",
            new Dictionary<string, object>
            {
                ["name"] = "Alice",
                ["age"] = 30,
                ["isActive"] = true
            });
        Assert.Equal(RightRuleStatusEnum.SUCCESS, registration.Status);

        var registration2 = rr.RegisterData(
            "Order",
            new Dictionary<string, object>
            {
                ["orderValue"] = 123.99,
                ["orderDate"] = "01.10.2026",
                ["isPaid"] = true
            });
        Assert.Equal(RightRuleStatusEnum.SUCCESS, registration2.Status);

        Assert.Equal(2, rr.DataService.GetDatasets().Count);

        var serializedData = rr.SerializeData("Customer");
        Assert.NotNull(serializedData);
        Assert.Contains("\"DatasetName\":\"Customer\"", serializedData);

        // Clear all the data from the data service so we can test loading it back in
        rr.DataService.Clear();
        Assert.Empty(rr.DataService.GetDatasets());

        List<DataRegistrationResult>? data = rr.LoadData(serializedData);
        Assert.NotNull(data);
        Assert.Single(rr.DataService.GetDatasets());
        Assert.Equal("Customer", rr.DataService.GetDatasets()[0].DatasetName);
    }

    private static string SingleDatasetJsonData()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append('[');
        sb.Append("{\"DatasetName\":\"Customer\",\"DataItems\":[{\"Key\":\"name\",\"Value\":\"Alice\"},{\"Key\":\"age\",\"Value\":30},{\"Key\":\"isActive\",\"Value\":true}],\"DataItemNames\":[\"name\",\"age\",\"isActive\"]}");
        sb.Append(']');
        return sb.ToString();
    }

    private static string MultipleDatasetJsonData()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append('[');
        sb.Append("{\"DatasetName\":\"Customer1\",\"DataItems\":[{\"Key\":\"name\",\"Value\":\"Alice\"},{\"Key\":\"age\",\"Value\":30},{\"Key\":\"isActive\",\"Value\":true}],\"DataItemNames\":[\"name\",\"age\",\"isActive\"]}");
        sb.Append(',');
        sb.Append("{\"DatasetName\":\"Customer2\",\"DataItems\":[{\"Key\":\"name\",\"Value\":\"Alice\"},{\"Key\":\"age\",\"Value\":30},{\"Key\":\"isActive\",\"Value\":true}],\"DataItemNames\":[\"name\",\"age\",\"isActive\"]}");
        sb.Append(']');
        return sb.ToString();
    }
}
