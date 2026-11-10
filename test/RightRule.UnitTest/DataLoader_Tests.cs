using RightRule;
using RightRule.DataServices.DataRegistration;
using RightRule.Models;

namespace RightRuleTests;

public class DataLoader_Tests
{
    [Fact]
    public void Json_LoadData_Test()
    {
        IRightRule rr = new RightRule.RightRule();

        var json = JsonData();
        List<DataRegistrationResult>? data = rr.LoadData(json);
        Assert.NotNull(data);
    }

    [Fact]
    public void SerializeThenLoadData_MultipleDatasets()
    {
        using var rightRule = new RightRule.RightRule();
        Assert.NotNull(rightRule);
        Assert.NotNull(rightRule.DataService);

        var registration = rightRule.RegisterData(
            "Customer",
            new Dictionary<string, object>
            {
                ["name"] = "Alice",
                ["age"] = 30,
                ["isActive"] = true
            });
        Assert.Equal(RightRuleStatusEnum.SUCCESS, registration.Status);

        var registration2 = rightRule.RegisterData(
            "Order",
            new Dictionary<string, object>
            {
                ["orderValue"] = 123.99,
                ["orderDate"] = "01.10.2026",
                ["isPaid"] = true
            });
        Assert.Equal(RightRuleStatusEnum.SUCCESS, registration2.Status);
        Assert.Equal(2, rightRule.DataService.GetDatasets().Count);

        var serializedData = rightRule.SerializeData();

        // Clear all the data from the data service so we can test loading it back in
        rightRule.DataService.Clear();
        Assert.Empty(rightRule.DataService.GetDatasets());

        List<DataRegistrationResult>? data = rightRule.LoadData(serializedData);
        Assert.NotNull(data);
        Assert.Equal(2, rightRule.DataService.GetDatasets().Count);
    }

    [Fact]
    public void SerializeThenLoadData_SingleDatasets()
    {
        using var rightRule = new RightRule.RightRule();
        Assert.NotNull(rightRule);
        Assert.NotNull(rightRule.DataService);

        var registration = rightRule.RegisterData(
            "Customer",
            new Dictionary<string, object>
            {
                ["name"] = "Alice",
                ["age"] = 30,
                ["isActive"] = true
            });
        Assert.Equal(RightRuleStatusEnum.SUCCESS, registration.Status);

        var registration2 = rightRule.RegisterData(
            "Order",
            new Dictionary<string, object>
            {
                ["orderValue"] = 123.99,
                ["orderDate"] = "01.10.2026",
                ["isPaid"] = true
            });
        Assert.Equal(RightRuleStatusEnum.SUCCESS, registration2.Status);
        Assert.Equal(2, rightRule.DataService.GetDatasets().Count);

        var serializedData = rightRule.SerializeData("Customer");

        // Clear all the data from the data service so we can test loading it back in
        rightRule.DataService.Clear();
        Assert.Empty(rightRule.DataService.GetDatasets());

        List<DataRegistrationResult>? data = rightRule.LoadData(serializedData);
        Assert.NotNull(data);
        Assert.Single(rightRule.DataService.GetDatasets());
    }

    private static string JsonData()
    {
        return "[{\"DatasetName\":\"Customer\",\"DataItems\":[{\"Key\":\"name\",\"Value\":\"Alice\"},{\"Key\":\"age\",\"Value\":30},{\"Key\":\"isActive\",\"Value\":true}],\"DataItemNames\":[\"name\",\"age\",\"isActive\"]},{\"DatasetName\":\"Order\",\"DataItems\":[{\"Key\":\"orderValue\",\"Value\":123.99},{\"Key\":\"orderDate\",\"Value\":\"01.10.2026\"},{\"Key\":\"isPaid\",\"Value\":true}],\"DataItemNames\":[\"orderValue\",\"orderDate\",\"isPaid\"]}]";
    }
}
