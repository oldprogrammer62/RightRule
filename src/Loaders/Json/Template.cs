using System.Text;

namespace RightRule.Loaders.Json;

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
