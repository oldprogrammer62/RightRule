using System.Data;

namespace RightRule.Rules;

internal class ParsedRuleService
{
    private readonly ParsedRules _parsedRules;
    public ParsedRules ParsedRules => _parsedRules;

    internal ParsedRuleService()
    {
        _parsedRules = new ParsedRules();
    }

    internal List<ParsedRule> GetRules(string Context)
    {
        return _parsedRules.Rules.Where(r => r.Context == Context).ToList();
    }

    internal List<ParsedRule> RegisterRules(IEnumerable<RuleDefinition> rules, HashSet<string> OperatorNames)
    {
        ArgumentNullException.ThrowIfNull(rules);

        var parsedRules = RuleRegistrationService.ParseRules(rules, OperatorNames);
        ParsedRules.AddRules(parsedRules);
        return parsedRules;
    }

    internal ParsedRule RegisterRule(RuleDefinition rule, HashSet<string> OperatorNames)
    {
        ArgumentNullException.ThrowIfNull(rule);

        var parsedRule = RuleRegistrationService.ParseRule(rule, OperatorNames);
        ParsedRules.AddRule(parsedRule);
        return parsedRule;
    }

    internal bool DeRegisterRules(string Context)
    {
        ArgumentNullException.ThrowIfNull(Context);
        if (string.IsNullOrWhiteSpace(Context))
        {
            throw new ArgumentException("Context cannot be null or empty.", nameof(Context));
        }
        return ParsedRules.RemoveRules(Context);
    }
}
