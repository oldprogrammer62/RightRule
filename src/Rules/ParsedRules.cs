namespace RightRule.Rules;

public class ParsedRules
{
    private List<ParsedRule> _rules;
    public List<ParsedRule> RightRules { get { return _rules; } }

    public ParsedRules()
    {
        _rules = [];
    }

    public void AddRule(ParsedRule rule)
    {
        _rules.Add(rule);
    }
}
