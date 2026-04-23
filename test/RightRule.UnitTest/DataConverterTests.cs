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
}
