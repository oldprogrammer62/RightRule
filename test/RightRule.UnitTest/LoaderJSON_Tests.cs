using RightRule.RuleRegistration.LogicRules;
using System.Text;

namespace RightRuleTests;

public class LoaderJSON_Tests
{
    [Fact]
    public void Json_Load_Test()
    {
        BeSoft.IRightRule rr = new BeSoft.RightRule();

        RightRule.Factories.RuleLoaderFactory.RuleLoaderFactory? loaderFactory = new RightRule.Factories.RuleLoaderFactory.RuleLoaderFactory(rr);

        var loader = loaderFactory.BuildLoader("JSON");
        Assert.NotNull(loader);

        var json = RightRule.RuleLoaders.Json.Template.JsonString();
        List<RightRule.RuleModels.DefinedRule>? rules = loader.Load(json);

        Assert.NotNull(rules);
    }

    [Fact]
    public void Json_Load_Test2()
    {
        BeSoft.IRightRule rr = new BeSoft.RightRule();

        var json = RightRule.RuleLoaders.Json.Template.JsonString();
        List<RuleRegistrationResult>? rules = rr.RegisterRulesJson(json);

        Assert.NotNull(rules);
    }

    [Fact]
    public void Json_Load_Test3()
    {
        BeSoft.IRightRule rr = new BeSoft.RightRule();

        int x = 100;
        var xregistered = rr.RegisterData("x", x);

        var json = JsonString();
        var rules = rr.RegisterRulesJson(json);
        Assert.NotNull(rules);

        var appliedRule = rr.ApplyRules("RuleContext");
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
        json.Append("'DataCheck': false");
        json.Append(',');
        json.Append("'Diagnostics': true");
        json.Append('}');

        json.Append('}');
        json.Append(']');
        return json.ToString();
    }
}
