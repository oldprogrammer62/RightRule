using RightRule.RuleServices.RuleModels;

namespace RightRule.RuleServices.RuleSerializer;

internal class RuleSerializerService : IRuleSerializerService
{
    public RuleSerializerService()
    {
    }

    public string SerializeRules(List<ParsedRule> Rules)
    {
        return Newtonsoft.Json.JsonConvert.SerializeObject(Rules);
    }
}
