using System.Text;
using System.Text.Json;

namespace RightRule.DataServices.DataLoader.Json;

public static class Template
{
    public static string JsonString(
        int number = 1,
        string name = "RuleName",
        string statement = "a = b",
        string context = "RuleContext",
        string trueDecision = "SUCCESS",
        object? trueDecisionData = null,
        string falseDecision = "FAIL",
        object? falseDecisionData = null,
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
                TrueDecision = trueDecision,
                TrueDecisionData = trueDecisionData,
                FalseDecision = falseDecision,
                FalseDecisionData = falseDecisionData,
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
        json.Append("'TrueDecision': 'SUCCESS'");
        json.Append(',');
        json.Append("'TrueDecisionData': '{}'");
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
