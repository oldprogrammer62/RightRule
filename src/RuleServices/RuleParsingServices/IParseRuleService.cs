using RightRule.RuleServices.RuleModels;

namespace RightRule.RuleServices.RuleParsingServices
{
    internal interface IParseRuleService
    {
        ParsedRule? ParseRule(DefinedRule Rule, bool Diagnostics);
        List<ParsedRule> ParseRules(IEnumerable<DefinedRule> Rules, bool Diagnostics);
    }
}