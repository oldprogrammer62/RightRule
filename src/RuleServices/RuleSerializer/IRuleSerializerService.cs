using RightRule.RuleServices.RuleModels;

namespace RightRule.RuleServices.RuleSerializer;

internal interface IRuleSerializerService
{
    string SerializeRules(List<ParsedRule> Rules);
}