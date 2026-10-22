using RightRule;
using RightRule.DataServices.DataLoaders.Json;

namespace RightRuleTests;

public class RuleLoaderFactoryTests
{
    [Fact]
    public void BuildLoader_WhenTypeIsJson_ReturnsJsonLoader()
    {
        using IRightRule rightRule = new RightRule.RightRule();
        var factory = new RightRule.Factories.RuleLoaderFactory.RuleLoaderFactory(rightRule);

        var loader = factory.BuildLoader("JSON");

        Assert.IsType<JsonLoader>(loader);
    }

    [Theory]
    [InlineData("XML")]
    [InlineData("json")]
    [InlineData("")]
    public void BuildLoader_WhenTypeIsUnsupported_ThrowsNotImplementedException(string loaderType)
    {
        using IRightRule rightRule = new RightRule.RightRule();
        var factory = new RightRule.Factories.RuleLoaderFactory.RuleLoaderFactory(rightRule);

        var exception = Assert.Throws<NotImplementedException>(
            () => factory.BuildLoader(loaderType));

        Assert.Equal($"Not Implemented: {loaderType}", exception.Message);
    }
}