using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Testing;
using RightRule;
using RightRule.Factories.RuleLoaderFactory;
using RightRule.RuleServices.RuleLoader.Json;

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

        var loader = factory.BuildRuleLoader(loaderType);

        Assert.IsType<JsonRuleLoader>(loader);
    }

    [Theory]
    [InlineData("XML")]
    [InlineData("")]
    public void BuildLoader_WhenTypeIsUnsupported_ReturnsNoLoader(string loaderType)
    {
        using IRightRule rightRule = new RightRule.RightRule();
        ILogger<RuleLoaderFactory> logger = new FakeLogger<RuleLoaderFactory>();
        var factory = new RuleLoaderFactory(logger);

        var loader = factory.BuildRuleLoader(loaderType);
        Assert.Null(loader);
    }
}