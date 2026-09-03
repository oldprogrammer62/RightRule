using RightRule.RuleModels;

namespace RightRule.RuleRegistration.LogicRules
{
    internal interface IRuleRegistrationService
    {
        ParsedRule? RegisterRule(DefinedRule rule, HashSet<string> OperatorNames, HashSet<string>? ObjectNames, HashSet<string>? DatasetNames);
        List<ParsedRule> RegisterRules(IEnumerable<DefinedRule> rules, HashSet<string> OperatorNames, HashSet<string>? ObjectNames, HashSet<string>? DatasetNames);
    }
}