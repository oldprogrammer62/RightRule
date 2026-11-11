using RightRule.Models;

namespace RightRuleTests;

public class DataSerializationTests
{
    [Fact]
    public void SerializeData_ReturnsAllRegisteredData()
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

        var serializedData = rightRule.SerializeData();

        Assert.Contains("\"DatasetName\":\"Customer\"", serializedData);
        Assert.Contains("\"Key\":\"name\"", serializedData);
        Assert.Contains("\"Value\":\"Alice\"", serializedData);

        Assert.Contains("\"DatasetName\":\"Order\"", serializedData);
        Assert.Contains("\"Key\":\"orderValue\"", serializedData);
        Assert.Contains("\"Value\":123.99", serializedData);
    }

    [Fact]
    public void SerializeData_ReturnsSpecifiedData()
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

        var serializedData = rightRule.SerializeData("Order");

        Assert.DoesNotContain("\"DatasetName\":\"Customer\"", serializedData);
        Assert.DoesNotContain("\"Key\":\"name\"", serializedData);
        Assert.DoesNotContain("\"Value\":\"Alice\"", serializedData);

        Assert.Contains("\"DatasetName\":\"Order\"", serializedData);
        Assert.Contains("\"Key\":\"orderValue\"", serializedData);
        Assert.Contains("\"Value\":123.99", serializedData);
    }
}
