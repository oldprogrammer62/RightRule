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
        IRightRule rr = new RightRule.RightRule();
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
        IRightRule rr = new RightRule.RightRule();
        Assert.NotNull(rr);

        var json = Template.JsonString();
        Assert.NotNull(json);

        List<RuleRegistrationResult>? rules = rr.LoadRules(json);
        Assert.NotNull(rules);
        Assert.Single(rules);

        var loadedRules = rr.GetRegisteredRules();
        Assert.NotNull(loadedRules);
        Assert.Single(loadedRules);
        Assert.Equal("RuleName", loadedRules[0].Name);
    }

    [Fact]
    public void Json_LoadRules_OnTopOfExistingRules()
    {
        IRightRule rr = new RightRule.RightRule();
        Assert.NotNull(rr);
        Assert.NotNull(rr.RuleService);

        var rule = rr.BuildRule()
            .Number(2)
            .Name("RuleName")
            .Context("NewContext")
            .Statement("x = 100")
            .TrueDecision("SUCCESS")
            .FalseDecision("FAIL")
            .Build()
            .Register(rr.RuleService);

        Assert.NotNull(rule);
        Assert.Single(rr.GetRegisteredRules());
        Assert.Single(rr.GetRegisteredRules("NewContext"));

        var json = JsonString(); // Context = "RuleContext"
        var rules = rr.LoadRules(json);
        Assert.NotNull(rules);
        Assert.Single(rules);
        Assert.Equal(RightRuleStatusEnum.SUCCESS, rules[0].Status);

        Assert.Equal(2, rr.GetRegisteredRules().Count);
        Assert.Single(rr.GetRegisteredRules("NewContext"));
        Assert.Single(rr.GetRegisteredRules("RuleContext"));
    }

    [Fact]
    public void Json_LoadRules_Test4()
    {
        RightRuleOptions options = new RightRuleOptions()
        {
            Diagnostics = true
        };

        IRightRule rr = new RightRule.RightRule(options);
        Assert.NotNull(rr);
        Assert.NotNull(rr.RuleService);

        int number = 1;
        string name = "RuleName";
        string statement = "a = b";
        string context = "RuleContext";
        string trueDecision = "SUCCESS";
        object? trueDecisionData = null;
        string falseDecision = "FAIL";
        object? falseDecisionData = null;
        object? optionalData = null;

        var json = JsonString(number, name, statement, context, trueDecision, trueDecisionData, falseDecision, falseDecisionData, optionalData);
        Assert.NotNull(json);

        List<RuleRegistrationResult>? rules = rr.LoadRules(json);
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
        json.Append("'TrueDecision': 'SUCCESS'");
        json.Append(',');
        json.Append("'TrueDecisionData': null");
        json.Append(',');
        json.Append("'FalseDecision': 'SUCCESS'");
        json.Append(',');
        json.Append("'FalseDecisionData': null");
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
        string trueDecision = "SUCCESS",
        object? trueDecisionData = null,
        string falseDecision = "FAIL",
        object? falseDecisionData = null,
        object? optionalData = null)
    {
        var x = MakeRule(number, name, statement, context, trueDecision, trueDecisionData, falseDecision, falseDecisionData, optionalData);
        return JsonSerializer.Serialize(x);
    }

    private static dynamic MakeRule(
        int number = 1,
        string name = "RuleName",
        string statement = "a = b",
        string context = "RuleContext",
        string trueDecision = "SUCCESS",
        object? trueDecisionData = null,
        string falseDecision = "FAIL",
        object? falseDecisionData = null,
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
                TrueDecision = trueDecision,
                TrueDecisionData = trueDecisionData,
                FalseDecision = falseDecision,
                FalseDecisionData = falseDecisionData,
                OptionalData = optionalData
            }
        };
    }
}
