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
        var data = new TestObject { Name = "John", Age = 30 };
        var dict = RightRule.DataConverters.DataConverter.ConvertToDictionary<TestObject>(data);
        Assert.Equal(2, dict.Count);
        Assert.Equal("John", dict["Name"]);
        Assert.Equal(30, dict["Age"]);
    }
}

internal class TestObject
{
    public string Name { get; set; }
    public int Age { get; set; }
}
