namespace RightRule.Rules;

public class ParsedRules
{
    private List<ParsedRule> _rules;
    internal List<ParsedRule> RightRules { get { return _rules; } }

    internal ParsedRules()
    {
        _rules = [];
    }

    internal void AddRule(ParsedRule rule)
    {
        _rules.Add(rule);
    }
}
