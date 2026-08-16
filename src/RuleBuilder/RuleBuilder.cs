using RightRule.RuleModels;
using System.Xml.Linq;

namespace RightRule.RuleBuilder;

public sealed class RuleBuilder : INumberStep, INameStep, IContextStep, IStatementStep, IDecisionStep, IRuleBuilder
{
    private readonly int _number;
    private readonly string _name;
    private readonly string _statement;
    private readonly string _context;
    private readonly string _decision;
    private readonly object? _decisionData;
    private readonly object? _optionalData;
    private readonly bool _checkData;
    private readonly bool _enableDiagnostics;

    // Private constructor for state progression
    private RuleBuilder(int number, string name, string context, string statement, string decision,
                        object? decisionData, object? optionalData, bool checkData, bool enableDiagnostics)
    {
        _number = number;
        _name = name;
        _context = context;
        _statement = statement;
        _decision = decision;
        _decisionData = decisionData;
        _optionalData = optionalData;
        _checkData = checkData;
        _enableDiagnostics = enableDiagnostics;
    }

    internal static INumberStep Create()
    {
        return new RuleBuilder(0, null, null, null, null, null, null, true, false);
    }

    // Step 1: Enforces that Number must be provided first
    public INameStep Number(int number)
    {
        return new RuleBuilder(number, _name, _context, _decision, _statement, _decisionData, _optionalData, _checkData, _enableDiagnostics);
    }

    // Step 2: Enforces that Name must follow Number
    public IContextStep Name(string name)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Name is mandatory.", nameof(name));
        return new RuleBuilder(_number, name, _context, _statement, _decision, _decisionData, _optionalData, _checkData, _enableDiagnostics);
    }

    // Step 3: Enforces that Context must follow Name
    public IStatementStep Context(string context)
    {
        if (string.IsNullOrWhiteSpace(context)) throw new ArgumentException("Context is mandatory.", nameof(context));
        return new RuleBuilder(_number, _name, context, _statement, _decision, _decisionData, _optionalData, _checkData, _enableDiagnostics);
    }

    // Step 4: Enforces that Statement must follow Context
    public IDecisionStep Statement(string statement)
    {
        if (string.IsNullOrWhiteSpace(statement)) throw new ArgumentException("Statement is mandatory.", nameof(statement));
        return new RuleBuilder(_number, _name, _context, statement, _decision, _decisionData, _optionalData, _checkData, _enableDiagnostics);
    }

    // Step 5: Enforces that Decision must follow Statement
    public IRuleBuilder Decision(string decision)
    {
        if (string.IsNullOrWhiteSpace(decision)) throw new ArgumentException("Decision is mandatory.", nameof(decision));
        return new RuleBuilder(_number, _name, _context, _statement, decision, _decisionData, _optionalData, _checkData, _enableDiagnostics);
    }

    // Unordered Mandatory / Optional Methods
    public IRuleBuilder DecisionData(object? decisionData)
    {
        return new RuleBuilder(_number, _name, _context, _statement, _decision, decisionData, _optionalData, _checkData, _enableDiagnostics);
    }

    public IRuleBuilder OptionalData(object? optionalData)
    {
        return new RuleBuilder(_number, _name, _context, _statement, _decision, _decisionData, optionalData, _checkData, _enableDiagnostics);
    }

    public IRuleBuilder CheckData(bool checkData = true)
    {
        return new RuleBuilder(_number, _name, _context, _statement, _decision, _decisionData, _optionalData, checkData, _enableDiagnostics);
    }

    public IRuleBuilder Diagnostics(bool enableDiagnostics = false)
    {
        return new RuleBuilder(_number, _name, _context, _statement, _decision, _decisionData, _optionalData, _checkData, enableDiagnostics);
    }

    // Terminal step: validates that remaining mandatory variables are populated before finishing
    public RuleDefinition Build()
    {
        if (_number.Equals(0)) throw new InvalidOperationException("Number is a mandatory field.");
        if (string.IsNullOrWhiteSpace(_name)) throw new InvalidOperationException("Name is a mandatory field.");
        if (string.IsNullOrWhiteSpace(_context)) throw new InvalidOperationException("Context is a mandatory field.");
        if (string.IsNullOrWhiteSpace(_statement)) throw new InvalidOperationException("Statement is a mandatory field.");
        if (string.IsNullOrWhiteSpace(_decision)) throw new InvalidOperationException("Decision is a mandatory field.");

        return RuleDefinition.Build(_number, _name, _context, _statement, _decision, _decisionData, _optionalData, _checkData, _enableDiagnostics);
    }
}
