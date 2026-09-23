using RightRule.RuleServices.RuleModels;

namespace RightRule.RuleServices.RuleParsingServices
{
    internal interface IParseRuleService
    {
        ParsedRule? ParseRule(DefinedRule Rule);
        List<ParsedRule> ParseRules(IEnumerable<DefinedRule> Rules);
    }
}