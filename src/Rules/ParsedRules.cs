namespace RightRule.Rules;

public class ParsedRules
{
    private readonly List<ParsedRule> _rules;
    internal List<ParsedRule> RightRules { get { return _rules; } }

    internal ParsedRules()
    {
        _rules = [];
    }

    internal ParsedRules(List<ParsedRule> rules)
    {
        _rules = rules;
    }

    internal void AddRule(ParsedRule rule)
    {
        _rules.Add(rule);
    }
}
