using RightRule.Models.RuleModels;

namespace RightRule.RegistrationServices.RuleRegistration
{
    internal interface IRuleRegistrationService
    {
        ParsedRule? RegisterRule(DefinedRule rule, HashSet<string> OperatorNames);
        List<ParsedRule> RegisterRules(IEnumerable<DefinedRule> rules, HashSet<string> OperatorNames);
    }
}