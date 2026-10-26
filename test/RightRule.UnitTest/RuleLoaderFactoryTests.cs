using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Testing;
using RightRule;
using RightRule.DataServices.DataLoaders.Json;
using RightRule.Factories.RuleLoaderFactory;

namespace RightRuleTests;

public class RuleLoaderFactoryTests
{
    [Theory]
    [InlineData("JSON")]
    [InlineData("json")]
    [InlineData("Json")]
    public void BuildLoader_WhenTypeIsJson_ReturnsJsonLoader(string loaderType)
    {
        using IRightRule rightRule = new RightRule.RightRule();
        ILogger<RuleLoaderFactory> logger = new FakeLogger<RuleLoaderFactory>();
        var factory = new RuleLoaderFactory(logger);

        var loader = factory.BuildLoader(loaderType);

        Assert.IsType<JsonLoader>(loader);
    }

    [Theory]
    [InlineData("XML")]
    [InlineData("")]
    public void BuildLoader_WhenTypeIsUnsupported_ReturnsNoLoader(string loaderType)
    {
        using IRightRule rightRule = new RightRule.RightRule();
        ILogger<RuleLoaderFactory> logger = new FakeLogger<RuleLoaderFactory>();
        var factory = new RuleLoaderFactory(logger);

        var loader = factory.BuildLoader(loaderType);
        Assert.Null(loader);
    }
}