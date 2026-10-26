using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Testing;
using RightRule;
using RightRule.DataServices.DataLoaders.Json;
using RightRule.Factories.RuleLoaderFactory;
using RightRule.RuleServices.RuleModels;
using RightRule.RuleServices.RuleRegistration;
using System.Text;

namespace RightRuleTests;

public class LoaderJSON_Tests
{
    [Fact]
    public void Json_Load_Test()
    {
        IRightRule rr = new RightRule.RightRule();
        ILogger<RuleLoaderFactory> logger = new FakeLogger<RuleLoaderFactory>();

        RuleLoaderFactory? loaderFactory = new RuleLoaderFactory(logger);

        var loader = loaderFactory.BuildLoader("JSON");
        Assert.NotNull(loader);

        var json = Template.JsonString();
        List<DefinedRule>? rules = loader.LoadFromJson(json);

        Assert.NotNull(rules);
    }

    [Fact]
    public void Json_Load_Test2()
    {
        IRightRule rr = new RightRule.RightRule();

        var json = Template.JsonString();
        List<RuleRegistrationResult>? rules = rr.RegisterRulesJson(json);

        Assert.NotNull(rules);
    }

    [Fact]
    public void Json_Load_Test3()
    {
        IRightRule rr = new RightRule.RightRule();

        int x = 100;
        var xregistered = rr.RegisterData(nameof(x), x);

        var json = JsonString();
        var rules = rr.RegisterRulesJson(json);
        Assert.NotNull(rules);

        var appliedRule = rr.ApplyRule("RuleContext");
        Assert.NotNull(appliedRule);
    }

    private static string JsonString()
    {
        StringBuilder json = new StringBuilder();
        json.Append('[');
        json.Append('{');
        json.Append("'Number': 1");
        json.Append(',');
        json.Append("'Name': 'RuleName'");
        json.Append(',');
        json.Append("'Statement': 'x = 100'");
        json.Append(',');
        json.Append("'Context': 'RuleContext'");
        json.Append(',');
        json.Append("'Decision': 'SUCCESS'");
        json.Append(',');
        json.Append("'DecisionData': null");
        json.Append(',');
        json.Append("'OptionalData': null");
        json.Append(',');

        json.Append("RuleConfiguration:");
        json.Append('{');
        json.Append("'Diagnostics': true");
        json.Append('}');

        json.Append('}');
        json.Append(']');
        return json.ToString();
    }
}
