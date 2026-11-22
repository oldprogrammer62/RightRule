using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Testing;
using RightRule;
using RightRule.Factories.RuleLoaderFactory;
using RightRule.Models;
using RightRule.RuleServices.RuleLoader.Json;
using RightRule.RuleServices.RuleModels;
using RightRule.RuleServices.RuleRegistration;
using System.Text;
using System.Text.Json;

namespace RightRuleTests;

public class RuleLoader_Tests
{
    [Fact]
    public void Json_DeserializeRuleJson()
    {
        var rr = new RightRule.RightRule().CreateSession();
        Assert.NotNull(rr);

        ILogger<RuleLoaderFactory> logger = new FakeLogger<RuleLoaderFactory>();
        RuleLoaderFactory? loaderFactory = new RuleLoaderFactory(logger);
        Assert.NotNull(loaderFactory);

        var loader = loaderFactory.BuildRuleLoader("JSON");
        Assert.NotNull(loader);

        var json = Template.JsonString();
        Assert.NotNull(json);

        List<DefinedRule>? rules = loader.Deserialize(json);
        Assert.NotNull(rules);
        Assert.Single(rules);

        Assert.Equal("RuleName", rules[0].Name);
    }

    [Fact]
    public void Json_LoadRules()
    {
        var rr = new RightRule.RightRule().CreateSession();
        Assert.NotNull(rr);

        var json = Template.JsonString();
        Assert.NotNull(json);

        List<RuleRegistrationResult>? rules = rr.RuleService.LoadRules(json);
        Assert.NotNull(rules);
        Assert.Single(rules);

        var loadedRules = rr.RuleService.GetRegisteredRules();
        Assert.NotNull(loadedRules);
        Assert.Single(loadedRules);
        Assert.Equal("RuleName", loadedRules[0].Name);
    }

    [Fact]
    public void Json_LoadRules_OnTopOfExistingRules()
    {
        var rr = new RightRule.RightRule().CreateSession();
        Assert.NotNull(rr);
        Assert.NotNull(rr.RuleService);

        var rule = rr.RuleService.BuildRule()
            .Number(2)
            .Name("RuleName")
            .Context("NewContext")
            .Statement("x = 100")
            .Decision("SUCCESS")
            .Build()
            .Register(rr.RuleService);

        Assert.NotNull(rule);
        Assert.Single(rr.RuleService.GetRegisteredRules());
        Assert.Single(rr.RuleService.GetRegisteredRules("NewContext"));

        var json = JsonString(); // Context = "RuleContext"
        var rules = rr.RuleService.LoadRules(json);
        Assert.NotNull(rules);
        Assert.Single(rules);
        Assert.Equal(RightRuleStatusEnum.SUCCESS, rules[0].Status);

        Assert.Equal(2, rr.RuleService.GetRegisteredRules().Count);
        Assert.Single(rr.RuleService.GetRegisteredRules("NewContext"));
        Assert.Single(rr.RuleService.GetRegisteredRules("RuleContext"));
    }

    [Fact]
    public void Json_LoadRules_Test4()
    {
        RightRuleOptions options = new RightRuleOptions()
        {
            Diagnostics = true
        };

        var rr = new RightRule.RightRule(options).CreateSession();
        Assert.NotNull(rr);
        Assert.NotNull(rr.RuleService);

        int number = 1;
        string name = "RuleName";
        string statement = "a = b";
        string context = "RuleContext";
        string decision = "SUCCESS";
        object? decisionData = null;
        object? optionalData = null;

        var json = JsonString(number, name, statement, context, decision, decisionData, optionalData);
        Assert.NotNull(json);

        List<RuleRegistrationResult>? rules = rr.RuleService.LoadRules(json);
        Assert.NotNull(rules);
        Assert.Single(rules);
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

    public static string JsonString(
        int number = 1,
        string name = "RuleName",
        string statement = "a = b",
        string context = "RuleContext",
        string decision = "SUCCESS",
        object? decisionData = null,
        object? optionalData = null)
    {
        var x = MakeRule(number, name, statement, context, decision, decisionData, optionalData);
        return JsonSerializer.Serialize(x);
    }

    private static dynamic MakeRule(
        int number = 1,
        string name = "RuleName",
        string statement = "a = b",
        string context = "RuleContext",
        string decision = "SUCCESS",
        object? decisionData = null,
        object? optionalData = null)
    {
        return new[]
        {
            new
            {
                Number = number,
                Name = name,
                Statement = statement,
                Context = context,
                Decision = decision,
                DecisionData = decisionData,
                OptionalData = optionalData
            }
        };
    }
}
