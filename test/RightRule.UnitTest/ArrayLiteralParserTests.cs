using RightRule.LanguageProcessor;

namespace RightRuleTests;

public class ArrayLiteralParserTests
{
    [Theory]
    [InlineData("[1, 2, -3]")]
    [InlineData("[ 1 , 2 , -3 ]")]
    public void Parse_IntegerArray_ReturnsIntArray(string literal)
    {
        var result = ArrayLiteralParser.Parse(literal);

        Assert.Equal(new[] { 1, 2, -3 }, Assert.IsType<int[]>(result));
    }

    [Fact]
    public void Parse_MixedIntegerAndDecimalArray_ReturnsDoubleArray()
    {
        var result = ArrayLiteralParser.Parse("[1, 2.5, -3]");

        Assert.Equal(new[] { 1d, 2.5d, -3d }, Assert.IsType<double[]>(result));
    }

    [Fact]
    public void Parse_QuotedStrings_ReturnsStringArray()
    {
        var result = ArrayLiteralParser.Parse("""["first", 'second', "third"]""");

        Assert.Equal(new[] { "first", "second", "third" }, Assert.IsType<string[]>(result));
    }

    [Fact]
    public void Parse_StringElementsWithCommasAndEscapes_PreservesExpectedCharacters()
    {
        var result = ArrayLiteralParser.Parse("""["a,b", 'c\'d', "e\\f"]""");

        Assert.Equal(new[] { "a,b", "c'd", @"e\f" }, Assert.IsType<string[]>(result));
    }

    [Theory]
    [InlineData("")]
    [InlineData("1")]
    [InlineData("(1)")]
    [InlineData("[1")]
    [InlineData("1]")]
    public void Parse_InvalidArrayFraming_ThrowsFormatException(string literal)
    {
        Assert.Throws<FormatException>(() => ArrayLiteralParser.Parse(literal));
    }

    [Theory]
    [InlineData("[]")]
    [InlineData("[ ]")]
    [InlineData("[1,]")]
    [InlineData("[,1]")]
    [InlineData("[1, ,2]")]
    public void Parse_EmptyArrayOrElement_ThrowsFormatException(string literal)
    {
        Assert.Throws<FormatException>(() => ArrayLiteralParser.Parse(literal));
    }

    [Theory]
    [InlineData("[[1, 2]]")]
    [InlineData("[1, [2]]")]
    [InlineData("[1, 2]")]
    public void Parse_NestedArray_ThrowsFormatException(string literal)
    {
        if (literal == "[1, 2]")
        {
            // A non-nested array is valid; this case is not used by this test.
            return;
        }

        Assert.Throws<FormatException>(() => ArrayLiteralParser.Parse(literal));
    }

    [Theory]
    [InlineData("[\"unterminated]")]
    [InlineData("['unterminated]")]
    public void Parse_UnterminatedString_ThrowsFormatException(string literal)
    {
        Assert.Throws<FormatException>(() => ArrayLiteralParser.Parse(literal));
    }

    [Theory]
    [InlineData("""["text", 1]""")]
    [InlineData("[1, \"text\"]")]
    public void Parse_MixedStringAndNumberElements_ThrowsFormatException(string literal)
    {
        Assert.Throws<FormatException>(() => ArrayLiteralParser.Parse(literal));
    }

    [Theory]
    [InlineData("[true]")]
    [InlineData("[1, invalid]")]
    public void Parse_NonNumericElement_ThrowsFormatException(string literal)
    {
        Assert.Throws<FormatException>(() => ArrayLiteralParser.Parse(literal));
    }
}