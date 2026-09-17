using RightRule.Models.RuleModels;

namespace RightRule.RegistrationServices.RuleRegistration.RuleBuilders.DefaultRuleBuilder;

internal sealed class DefaultRuleBuilder : IDefaultNameStep, IDefaultContextStep, IDefaultDecisionStep, IDefaultRuleBuilder
{
    private readonly string _name;
    private readonly string _context;
    private readonly string _decision;
    private readonly object? _decisionData;

    // Private constructor for state progression
    private DefaultRuleBuilder(string name, string context, string decision, object? decisionData)
    {
        _name = name;
        _context = context;
        _decision = decision;
        _decisionData = decisionData;
    }

    internal static IDefaultNameStep Create()
    {
        string name = string.Empty;
        string context = string.Empty;
        string decision = string.Empty;
        object? decisionData = null;

        return new DefaultRuleBuilder(name, context, decision, decisionData);
    }

    // Step 1: Enforces that Context must follow Name
    public IDefaultContextStep Name(string name)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Name is mandatory.", nameof(name));
        return new DefaultRuleBuilder(name, _context, _decision, _decisionData);
    }

    // Step 2: Enforces that Decision must follow Context
    public IDefaultDecisionStep Context(string context)
    {
        if (string.IsNullOrWhiteSpace(context)) throw new ArgumentException("Context is mandatory.", nameof(context));
        return new DefaultRuleBuilder(_name, context, _decision, _decisionData);
    }
  
    // Step 3: Enforces that Decision must follow Context
    public IDefaultRuleBuilder Decision(string decision)
    {
        if (string.IsNullOrWhiteSpace(decision)) throw new ArgumentException("Decision is mandatory.", nameof(decision));
        return new DefaultRuleBuilder(_name, _context, decision, _decisionData);
    }

    // Optional properties
    public IDefaultRuleBuilder DecisionData(object? decisionData)
    {
        return new DefaultRuleBuilder(_name, _context, _decision, decisionData);
    }

    // Terminal step: validates that remaining mandatory variables are populated before finishing
    public DefaultRule Build()
    {
        if (string.IsNullOrWhiteSpace(_name)) throw new InvalidOperationException("Name is a mandatory field.");
        if (string.IsNullOrWhiteSpace(_context)) throw new InvalidOperationException("Context is a mandatory field.");
        if (string.IsNullOrWhiteSpace(_decision)) throw new InvalidOperationException("Decision is a mandatory field.");

        return DefaultRule.Build(_name, _context, _decision, _decisionData);
    }
}
