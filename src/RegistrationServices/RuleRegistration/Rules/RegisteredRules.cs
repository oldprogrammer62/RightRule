using RightRule.Models.RuleModels;

namespace RightRule.RegistrationServices.RuleRegistration.Rules;

/// <summary>
/// A Class used to hold a list of ParsedRule objects.
/// It provides methods to add and remove rules from the list.
/// It is used during the evaluation process to evaluate the rules against the provided data.
/// The RightRules property allows the interpreter to access the list of parsed rules and evaluate them accordingly.
/// This class is internal
/// </summary>
internal class RegisteredRules : IRegisteredRules
{
    private readonly List<ParsedRule> _rules;
    public List<ParsedRule> Rules { get { return _rules; } }

    public int Count => _rules.Count;

    public ParsedRule? this[int index] => index >= 0 && index < _rules.Count ? _rules[index] : null;

    public RegisteredRules()
    {
        _rules = [];
    }

    internal RegisteredRules(List<ParsedRule> rules)
    {
        _rules = rules;
    }

    public RuleRegistrationResult AddRule(ParsedRule rule)
    {
        if (rule == null)
        {
            return RuleRegistrationResult.Failed("Rule is null");
        }
        var existingRule = GetRule(rule.Number, rule.Name);
        switch (existingRule)
        {
            case RuleFound.NOTFOUND:
                _rules.Add(rule);
                return RuleRegistrationResult.Success($"Rule: {rule.Number}-{rule.Name} Registered", rule);
            case RuleFound.FOUNDBYNUMBER:
                return RuleRegistrationResult.Failed($"Rule Number: {rule.Number} already registered", rule);
            case RuleFound.FOUNDBYNAME:
                return RuleRegistrationResult.Failed($"Rule Name: {rule.Name} already registered", rule);
            default:
                return RuleRegistrationResult.Failed($"Rule: {rule.Number}-{rule.Name} not registered", rule);
        }
    }

    public List<RuleRegistrationResult> AddRules(List<ParsedRule> rules)
    {
        List<RuleRegistrationResult> results = new List<RuleRegistrationResult>();
        foreach (ParsedRule rule in rules)
        {
            results.Add(AddRule(rule));
        }
        return results;
    }

    public bool RemoveRules(string Context)
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

    public bool RemoveRule(string Name)
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

    public bool RemoveRule(int Number)
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

    public bool RemoveRules()
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

    public List<(int Number, string Name)> GetRuleNumbers()
    {
        return _rules
            .DistinctBy(rule => rule.Number)
            .Select(rule => (rule.Number, rule.Name))
            .ToList();
    }

    public List<string> GetRuleNames()
    {
        return _rules
            .Select(rule => rule.Name)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .Order()
            .ToList();
    }

    public List<string> GetRuleContexts()
    {
        return _rules
            .Select(rule => rule.Context)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .Order()
            .ToList();
    }

    private RuleFound GetRule(int number, string name)
    {
        if (GetRule(number))
        {
            return RuleFound.FOUNDBYNUMBER;
        }
        if (GetRule(name))
        {
            return RuleFound.FOUNDBYNAME;
        }
        return RuleFound.NOTFOUND;
    }

    private bool GetRule(int number)
    {
        return _rules.Any(d => d.Number.Equals(number));
    }

    private bool GetRule(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return false;
        }
        return _rules.Any(d => d.Name.Equals(name, StringComparison.Ordinal));
    }
}
