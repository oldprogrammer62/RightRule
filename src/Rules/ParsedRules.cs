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

    internal void AddRules(List<ParsedRule> rules)
    {
        foreach (ParsedRule rule in rules)
        {
            _rules.Add(rule);
        }
    }

    internal bool RemoveRules(string Context)
    {
        try
        {
            _rules.RemoveAll(r => r.Context == Context);
            return true;
        }
        catch
        {
            return false;
        }
    }
}
