using System.Text;

namespace RightRule.DataServices.DataLoaders.Json;

public static class Template
{
    public static string JsonString()
    {
        StringBuilder json = new StringBuilder();
        json.Append('[');
        json.Append('{');
        json.Append("'Number': 1");
        json.Append(',');
        json.Append("'Name': 'RuleName'");
        json.Append(',');
        json.Append("'Statement': 'a = b'");
        json.Append(',');
        json.Append("'Context': 'RuleContext'");
        json.Append(',');
        json.Append("'Decision': 'SUCCESS'");
        json.Append(',');
        json.Append("'DecisionData': '{}'");
        json.Append(',');
        json.Append("'FalseDecision': 'FAIL'");
        json.Append(',');
        json.Append("'FalseDecisionData': '{}'");
        json.Append(',');
        json.Append("'OptionalData': null");
        json.Append(',');
        json.Append('}');
        json.Append(']');
        return json.ToString();
    }
}
