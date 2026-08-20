using BeSoft;
using System.Text;

namespace RightRuleTests;

public class RuleGetters_Tests
{
    readonly IRightRule rr;

    public RuleGetters_Tests()
    {
        rr = new BeSoft.RightRule();
        rr.RegisterRulesJson(JsonString());
    }

    [Fact]
    public void GetRuleNumbers()
    {
        var numbers = rr.GetRuleNumbers();
        Assert.NotNull(numbers);
    }

    [Fact]
    public void GetRuleNames()
    {
        var names = rr.GetRuleNames();
        Assert.NotNull(names);
    }

    [Fact]
    public void GetRuleContexts()
    {
        var contexts = rr.GetRuleContexts();
        Assert.NotNull(contexts);
    }

    private static string JsonString()
    {
        StringBuilder json = new StringBuilder();
        json.Append('[');
        json.Append('{');
        json.Append("'Number': 1");
        json.Append(',');
        json.Append("'Name': 'RuleName1'");
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

        json.Append(',');

        json.Append('{');
        json.Append("'Number': 2");
        json.Append(',');
        json.Append("'Name': 'RuleName2'");
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

        json.Append(',');

        json.Append('{');
        json.Append("'Number': 3");
        json.Append(',');
        json.Append("'Name': 'RuleName3'");
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
