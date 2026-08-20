using RightRule.RuleModels;

namespace RightRule.RuleRegistration
{
    internal interface IRuleRegistrationService
    {
        ParsedRule? RegisterRule(RuleDefinition rule, HashSet<string> OperatorNames, HashSet<string>? ObjectNames, HashSet<string>? DatasetNames);
        List<ParsedRule> RegisterRules(IEnumerable<RuleDefinition> rules, HashSet<string> OperatorNames, HashSet<string>? ObjectNames, HashSet<string>? DatasetNames);
    }
}