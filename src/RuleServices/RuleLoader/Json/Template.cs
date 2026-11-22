using System.Text;
using System.Text.Json;

namespace RightRule.RuleServices.RuleLoader.Json;

public static class Template
{
    public static string JsonString(
        int number = 1,
        string name = "RuleName",
        string statement = "a = b",
        string context = "RuleContext",
        string decision = "SUCCESS",
        object? decisionData = null,
        object? optionalData = null
        )
    {
        var rules = 
        new[]
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
        return JsonSerializer.Serialize(rules);
    }

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
        json.Append("'OptionalData': null");
        json.Append(',');
        json.Append('}');
        json.Append(']');
        return json.ToString();
    }
}
