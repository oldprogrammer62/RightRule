using RightRule.Models.RuleModels;

namespace RightRule.RegistrationServices.RuleRegistration
{
    internal interface IRuleRegistrationService
    {
        ParsedRule? RegisterRule(DefinedRule rule, HashSet<string> OperatorNames, HashSet<string>? ObjectNames, HashSet<string>? DatasetNames);
        List<ParsedRule> RegisterRules(IEnumerable<DefinedRule> rules, HashSet<string> OperatorNames, HashSet<string>? ObjectNames, HashSet<string>? DatasetNames);
    }
}