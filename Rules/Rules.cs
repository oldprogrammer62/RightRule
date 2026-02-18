namespace RightRule.Rules;

public class Rules
{
    private List<Rule> _rules;
    public List<Rule> Ruless { get { return _rules; } }

    public Rules()
    {
        _rules = [];
    }

    public void AddRule(Rule rule)
    {
        _rules.Add(rule);
    }
}
