namespace RightRuleTests;

public class DataConverterTests
{
    [Fact]
    public void ListConverterTest()
    {
        var listConverter = new RightRule.DataConverters.ListConverter();
        var data = new List<string> { "apple", "banana", "cherry" };
        var dict = listConverter.ConvertToDictionary(data);
        Assert.Equal(3, dict.Count);
        Assert.Equal("apple", dict["0"]);
        Assert.Equal("banana", dict["1"]);
        Assert.Equal("cherry", dict["2"]);
    }

    [Fact]
    public void ListConverterViaDataConverterTest()
    {
        var data = new List<string> { "apple", "banana", "cherry" };
        var dict = RightRule.DataConverters.DataConverter.ConvertToDictionary<List<string>>(data);
        Assert.Equal(3, dict.Count);
        Assert.Equal("apple", dict["0"]);
        Assert.Equal("banana", dict["1"]);
        Assert.Equal("cherry", dict["2"]);
    }

    [Fact]
    public void ObjectConverterViaDataConverterTest()
    {
        var data = new TestObject { Fruit = "Apple", Colour = "green" };
        var dict = RightRule.DataConverters.DataConverter.ConvertToDictionary<TestObject>(data);
        Assert.Equal(2, dict.Count);
        Assert.Equal("Apple", dict["Fruit"]);
        Assert.Equal("green", dict["Colour"]);
    }
}

internal class TestObject
{
    public required string Fruit { get; set; }
    public required string Colour { get; set; }
}
