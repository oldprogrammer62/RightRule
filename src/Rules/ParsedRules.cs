namespace RightRule.Rules;

/// <summary>
/// A Class used to hold a list of ParsedRule objects.
/// It provides methods to add and remove rules from the list.
/// It is used during the evaluation process to evaluate the rules against the provided data.
/// The RightRules property allows the interpreter to access the list of parsed rules and evaluate them accordingly.
/// This class is internal
/// </summary>
public class ParsedRules
{
    private readonly List<ParsedRule> _rules;
    internal List<ParsedRule> Rules { get { return _rules; } }

    public int Count {  get { return Rules.Count; } }

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

    internal bool RemoveRules()
    {
        try
        {
            _rules.Clear();
            return true;
        }
        catch
        {
            return false;
        }
    }
}
