using BeSoft;
using Newtonsoft.Json;
using RightRule.RuleModels;

namespace RightRule.RuleLoaders.Json;

internal class JsonLoader : IRuleLoader
{
    private IRightRule _rightRule;

    public JsonLoader(IRightRule rightRule)
    {
        _rightRule = rightRule;
    }

    public List<ParsedRule>? Load()
    {
        var rules = LoadAndRegister(Json);
        return rules;
    }

    public List<ParsedRule>? Load(string JsonData)
    {
        var rules = LoadAndRegister(JsonData);
        return rules;
    }

    public List<ParsedRule>? LoadAndRegister(string JsonData)
    {
        var ruleDefs = ConvertJson(JsonData);
        if (ruleDefs == null)
        {
            return null;
        }
        return _rightRule.RegisterRules(ruleDefs);
    }

    private List<RuleDefinition>? ConvertJson(string JsonData)
    {
        if (string.IsNullOrWhiteSpace(JsonData))
        {
            return null;
        }

        try
        {
            List<RuleDefinition> rules = JsonConvert.DeserializeObject<List<RuleDefinition>>(JsonData);
            return rules;
        }
        catch
        {
            return null;
        }
    }

    private string Json =
        "[\r\n  {\r\n    'Number': 1,\r\n    'Name': 'RuleOne',\r\n    'Statement': 'a = b',\r\n    'Context': 'Context',\r\n    'Decision': 'GOOD',\r\n    'DecisionData': null,\r\n    'OptionalData': null\r\n  }\r\n]\r\n";

}
