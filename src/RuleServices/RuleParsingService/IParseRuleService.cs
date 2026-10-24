using RightRule.RuleServices.RuleModels;

namespace RightRule.RuleServices.RuleParsingService;

internal interface IParseRuleService
{
    ParsedRule? ParseRule(DefinedRule Rule, bool Diagnostics);
}