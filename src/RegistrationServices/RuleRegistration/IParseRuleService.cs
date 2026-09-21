using RightRule.Models.RuleModels;

namespace RightRule.RegistrationServices.RuleRegistration
{
    internal interface IParseRuleService
    {
        ParsedRule? ParseRule(DefinedRule Rule);
        List<ParsedRule> ParseRules(IEnumerable<DefinedRule> Rules);
    }
}