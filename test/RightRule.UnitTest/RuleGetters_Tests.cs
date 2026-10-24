using RightRule;
using System.Text;

namespace RightRuleTests;

public class RuleGetters_Tests
{
    readonly IRightRule rr;

    public RuleGetters_Tests()
    {
        rr = new RightRule.RightRule();
        rr.RegisterRulesJson(JsonString());
    }

    [Fact]
    public void GetRuleNumbers()
    {
        var numbers = rr.GetRuleNumbers("RuleContext");
        Assert.NotNull(numbers);
    }

    [Fact]
    public void GetRuleNames()
    {
        var names = rr.GetRuleNames("RuleContext");
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
        json.Append("'TrueDecision': 'SUCCESS'");
        json.Append(',');
        json.Append("'TrueDecisionData': null");
        json.Append(',');
        json.Append("'OptionalData': null");
        json.Append(',');
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
        json.Append("'TrueDecision': 'SUCCESS'");
        json.Append(',');
        json.Append("'TrueDecisionData': null");
        json.Append(',');
        json.Append("'OptionalData': null");
        json.Append(',');
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
        json.Append("'TrueDecision': 'SUCCESS'");
        json.Append(',');
        json.Append("'TrueDecisionData': null");
        json.Append(',');
        json.Append("'OptionalData': null");
        json.Append(',');
        json.Append('}');

        json.Append(']');
        return json.ToString();
    }
}
