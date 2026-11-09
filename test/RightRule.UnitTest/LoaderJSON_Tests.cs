using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Testing;
using RightRule;
using RightRule.Factories.RuleLoaderFactory;
using RightRule.RuleServices.RuleLoader.Json;
using RightRule.RuleServices.RuleModels;
using RightRule.RuleServices.RuleRegistration;
using System.Text;
using System.Text.Json;
using System.Xml.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace RightRuleTests;

public class LoaderJSON_Tests
{
    [Fact]
    public void Json_Object_Test()
    {

    }

    [Fact]
    public void Json_Load_Test()
    {
        IRightRule rr = new RightRule.RightRule();
        ILogger<RuleLoaderFactory> logger = new FakeLogger<RuleLoaderFactory>();

        RuleLoaderFactory? loaderFactory = new RuleLoaderFactory(logger);

        var loader = loaderFactory.BuildRuleLoader("JSON");
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
        List<RuleRegistrationResult>? rules = rr.LoadRules(json);

        Assert.NotNull(rules);
    }

    [Fact]
    public void Json_Load_Test3()
    {
        IRightRule rr = new RightRule.RightRule();

        int x = 100;
        var xregistered = rr.RegisterData(nameof(x), x);

        var json = JsonString();
        var rules = rr.LoadRules(json);
        Assert.NotNull(rules);

        var appliedRule = rr.ApplyRule("RuleContext");
        Assert.NotNull(appliedRule);
    }

    [Fact]
    public void Json_Load_Test4()
    {
        IRightRule rr = new RightRule.RightRule();

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

        List<RuleRegistrationResult>? rules = rr.LoadRules(json);

        Assert.NotNull(rules);
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
