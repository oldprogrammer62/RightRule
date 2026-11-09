
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Testing;
using RightRule;
using RightRule.DataServices.DataRegistration;
using RightRule.Factories.DataLoaderFactory;
using RightRule.Models;

namespace RightRuleTests;

public class DataLoader_Tests
{
    [Fact]
    public void Json_LoadData_Test()
    {
        IRightRule rr = new RightRule.RightRule();
        ILogger<DataLoaderFactory> logger = new FakeLogger<DataLoaderFactory>();

        DataLoaderFactory? loaderFactory = new DataLoaderFactory(logger);
        var loader = loaderFactory.BuildDataLoader("JSON");
        Assert.NotNull(loader);

        var json = JsonData();
        List<DataRegistrationResult>? data = rr.LoadData(json);
        Assert.NotNull(data);
    }

    [Fact]
    public void DeserializeThenLoadData()
    {
        using var rightRule = new RightRule.RightRule();

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

        var serializedData = rightRule.DeserializeData();

        // Clear all the data from the data service so we can test loading it back in
        rightRule.DataService.Clear();

        // Now try and load the data
        ILogger<DataLoaderFactory> logger = new FakeLogger<DataLoaderFactory>();

        DataLoaderFactory? loaderFactory = new DataLoaderFactory(logger);
        var loader = loaderFactory.BuildDataLoader("JSON");
        Assert.NotNull(loader);

        var json = serializedData;
        List<DataRegistrationResult>? data = rightRule.LoadData(json);
        Assert.NotNull(data);
    }

    private static string JsonData()
    {
        return "[{\"DatasetName\":\"Customer\",\"DataItems\":[{\"Key\":\"name\",\"Value\":\"Alice\"},{\"Key\":\"age\",\"Value\":30},{\"Key\":\"isActive\",\"Value\":true}],\"DataItemNames\":[\"name\",\"age\",\"isActive\"]},{\"DatasetName\":\"Order\",\"DataItems\":[{\"Key\":\"orderValue\",\"Value\":123.99},{\"Key\":\"orderDate\",\"Value\":\"01.10.2026\"},{\"Key\":\"isPaid\",\"Value\":true}],\"DataItemNames\":[\"orderValue\",\"orderDate\",\"isPaid\"]}]";
    }
}
