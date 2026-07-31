
namespace RightRule.RuleModels;

/// <summary>
/// A Class used to hold a list of ParsedRule objects.
/// It provides methods to add and remove rules from the list.
/// It is used during the evaluation process to evaluate the rules against the provided data.
/// The RightRules property allows the interpreter to access the list of parsed rules and evaluate them accordingly.
/// This class is internal
/// </summary>
public class RegisteredRules
{
    private readonly List<ParsedRule> _rules;
    public List<ParsedRule> Rules { get { return _rules; } }

    public int Count => _rules.Count;

    public ParsedRule? this[int index] => index >= 0 && index < _rules.Count ? _rules[index] : null;

    internal RegisteredRules()
    {
        _rules = [];
    }

    internal RegisteredRules(List<ParsedRule> rules)
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
            _rules.RemoveAll(r => string.Equals(r.Context, Context, StringComparison.OrdinalIgnoreCase));
            return true;
        }
        catch
        {
            return false;
        }
    }

    internal bool RemoveRule(string Name)
    {
        try
        {
            _rules.RemoveAll(r => string.Equals(r.Name, Name, StringComparison.OrdinalIgnoreCase));
            return true;
        }
        catch
        {
            return false;
        }
    }

    internal bool RemoveRule(int Number)
    {
        try
        {
            _rules.RemoveAll(r => r.Number == Number);
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
