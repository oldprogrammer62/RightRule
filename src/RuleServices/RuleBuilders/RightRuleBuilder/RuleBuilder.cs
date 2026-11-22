using RightRule.RuleServices.RuleModels;

namespace RightRule.RuleServices.RuleBuilders.RightRuleBuilder;

internal sealed class RuleBuilder : INumberStep, INameStep, IContextStep, IStatementStep, IDecisionStep, IRuleBuilder
{
    private readonly int _number;
    private readonly string _name;
    private readonly string _statement;
    private readonly string _context;
    private readonly string _decision;
    private readonly object? _decisionData;
    private readonly object? _optionalData;

    // Private constructor for state progression
    private RuleBuilder(
        int number, 
        string name, 
        string context, 
        string statement, 
        string decision, 
        object? decisionData,
        object? optionalData
        )
    {
        _number = number;
        _name = name;
        _context = context;
        _statement = statement;
        _decision = decision;
        _decisionData = decisionData;
        _optionalData = optionalData;
    }

    internal static INumberStep Create()
    {
        int number = 0;
        string name = string.Empty;
        string context = string.Empty;
        string statement = string.Empty;
        string decision = string.Empty;
        object? decisionData = null;
        object? optionalData = null;

        return new RuleBuilder(number, name, context, statement, decision, decisionData, optionalData);
    }

    // Step 1: Enforces that Number must be provided first
    public INameStep Number(int number)
    {
        return new RuleBuilder(number, _name, _context, _statement, _decision, _decisionData, _optionalData);
    }

    // Step 2: Enforces that Name must follow Number
    public IContextStep Name(string name)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Name is mandatory.", nameof(name));
        return new RuleBuilder(_number, name, _context, _statement, _decision, _decisionData, _optionalData);
    }

    // Step 3: Enforces that Context must follow Name
    public IStatementStep Context(string context)
    {
        if (string.IsNullOrWhiteSpace(context)) throw new ArgumentException("Context is mandatory.", nameof(context));
        return new RuleBuilder(_number, _name, context, _statement, _decision, _decisionData, _optionalData);
    }

    // Step 4: Enforces that Statement must follow Context
    public IDecisionStep Statement(string statement)
    {
        if (string.IsNullOrWhiteSpace(statement)) throw new ArgumentException("Statement is mandatory.", nameof(statement));
        return new RuleBuilder(_number, _name, _context, statement, _decision, _decisionData, _optionalData);
    }

    // Step 5: Enforces that Decision must follow Statement
    public IRuleBuilder Decision(string decision)
    {
        if (string.IsNullOrWhiteSpace(decision)) throw new ArgumentException("Decision is mandatory.", nameof(decision));
        return new RuleBuilder(_number, _name, _context, _statement, decision, _decisionData, _optionalData);
    }

    // Unordered Optional
    public IRuleBuilder DecisionData(object? decisionData)
    {
        return new RuleBuilder(_number, _name, _context, _statement, _decision, decisionData, _optionalData);
    }

    // Unordered Optional
    public IRuleBuilder OptionalData(object? optionalData)
    {
        return new RuleBuilder(_number, _name, _context, _statement, _decision, _decisionData, optionalData);
    }

    // Terminal step: validates that remaining mandatory variables are populated before finishing
    public DefinedRule Build()
    {
        if (_number.Equals(0)) throw new InvalidOperationException("Number is a mandatory field.");
        if (string.IsNullOrWhiteSpace(_name)) throw new InvalidOperationException("Name is a mandatory field.");
        if (string.IsNullOrWhiteSpace(_context)) throw new InvalidOperationException("Context is a mandatory field.");
        if (string.IsNullOrWhiteSpace(_statement)) throw new InvalidOperationException("Statement is a mandatory field.");
        if (string.IsNullOrWhiteSpace(_decision)) throw new InvalidOperationException("True Decision is a mandatory field.");

        return DefinedRule.Build(_number, _name, _context, _statement, _decision, _decisionData, _optionalData);
    }
}
