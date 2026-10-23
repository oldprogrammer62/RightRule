using RightRule.RuleServices.RuleModels;
using RightRule.RuleServices.RuleRegistration;
using System.Runtime.CompilerServices;

namespace RightRule.RuleServices.Rules;

/// <summary>
/// A Class used to hold a list of ParsedRule objects.
/// It provides methods to add and remove rules from the list.
/// It is used during the evaluation process to evaluate the rules against the provided data.
/// The RightRules property allows the interpreter to access the list of parsed rules and evaluate them accordingly.
/// This class is internal
/// </summary>
internal sealed class RegisteredRules : IRegisteredRules
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

    public List<RuleRegistrationResult> AddRules(List<ParsedRule> rules)
    {
        List<RuleRegistrationResult> results = new List<RuleRegistrationResult>();
        foreach (ParsedRule rule in rules)
        {
            results.Add(AddRule(rule));
        }
        return results;
    }

    public RuleRegistrationResult AddRule(ParsedRule Rule)
    {
        if (Rule == null)
        {
            return RuleRegistrationResult.Failed("Rule is null");
        }
        var existingRule = GetRule(Rule);
        switch (existingRule)
        {
            case RuleFoundEnum.NOTFOUND:
                _rules.Add(Rule);
                return RuleRegistrationResult.Success(Rule, $"Rule: {Rule.Number}-{Rule.Name} Registered");

            case RuleFoundEnum.FOUNDBYNUMBER:
                return RuleRegistrationResult.Failed(Rule, $"Rule Number: {Rule.Number} already registered");

            case RuleFoundEnum.FOUNDBYNAME:
                return RuleRegistrationResult.Failed(Rule, $"Rule Name: {Rule.Name} already registered");

            default:
                return RuleRegistrationResult.Failed(Rule, $"Rule: {Rule.Number}-{Rule.Name} not registered");
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

    public bool RemoveRules(string Context)
    {
        try
        {
            _rules.RemoveAll(r => 
            string.Equals(r.Context, Context, StringComparison.OrdinalIgnoreCase));
            return true;
        }
        catch
        {
            return false;
        }
    }

    public bool RemoveRule(string Context, string RuleName)
    {
        try
        {
            _rules.RemoveAll(r => 
            r.Context.Equals(Context, StringComparison.OrdinalIgnoreCase) 
            && 
            r.Name.Equals(RuleName, StringComparison.OrdinalIgnoreCase));
            return true;
        }
        catch
        {
            return false;
        }
    }

    public bool RemoveRule(string Context, int RuleNumber)
    {
        try
        {
            _rules.RemoveAll(r => 
            r.Context.Equals(Context, StringComparison.OrdinalIgnoreCase)
            &&
            r.Number == RuleNumber);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public List<(int Number, string Name)>? GetRuleNumbers(string Context)
    {
        if (string.IsNullOrWhiteSpace(Context))
        {
            return null;
        }
        var query = _rules.Where(r => 
            string.Equals(r.Context, Context, StringComparison.OrdinalIgnoreCase));

        return query
            .DistinctBy(rule => rule.Number)
            .Select(rule => (rule.Number, rule.Name))
            .ToList();
    }

    public List<string>? GetRuleNames(string Context)
    {
        if (string.IsNullOrWhiteSpace(Context))
        {
            return null;
        }
        var query = _rules.Where(r =>
            string.Equals(r.Context, Context, StringComparison.OrdinalIgnoreCase));

        return query
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

    private RuleFoundEnum GetRule(ParsedRule Rule)
    {
        var number = Rule.Number;
        var name = Rule.Name;
        var context = Rule.Context;

        if (GetRule(context, number))
        {
            return RuleFoundEnum.FOUNDBYNUMBER;
        }
        if (GetRule(context, name))
        {
            return RuleFoundEnum.FOUNDBYNAME;
        }
        return RuleFoundEnum.NOTFOUND;
    }

    private bool GetRule(string Context, int RuleNumber)
    {
        return _rules.Any(d => 
        d.Context.Equals(Context, StringComparison.OrdinalIgnoreCase) 
        && 
        d.Number.Equals(RuleNumber));
    }

    private bool GetRule(string Context, string RuleName)
    {
        return _rules.Any(d => 
        d.Context.Equals(Context, StringComparison.OrdinalIgnoreCase) 
        && 
        d.Name.Equals(RuleName, StringComparison.OrdinalIgnoreCase));
    }
}
