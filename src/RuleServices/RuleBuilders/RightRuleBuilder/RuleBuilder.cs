using RightRule.ConfigurationService;
using RightRule.RuleServices.RuleModels;

namespace RightRule.RuleServices.RuleBuilders.RightRuleBuilder;

internal sealed class RuleBuilder : INumberStep, INameStep, IContextStep, IStatementStep, IDecisionStep, IFalseDecisionStep, IRuleBuilder
{
    private readonly int _number;
    private readonly string _name;
    private readonly string _statement;
    private readonly string _context;
    private readonly string _decision;
    private readonly object? _decisionData;
    private readonly string _falseDecision;
    private readonly object? _falseDecisionData;
    private readonly object? _optionalData;
    private readonly bool _diagnostics = RightRuleConfiguration.DEFAULT_DIAGNOSTICS;

    // Private constructor for state progression
    private RuleBuilder(
        int number, 
        string name, 
        string context, 
        string statement, 
        string decision, 
        object? decisionData,
        string falseDecision, 
        object? falseDecisionData,
        object? optionalData, 
        bool diagnostics
        )
    {
        _number = number;
        _name = name;
        _context = context;
        _statement = statement;
        _decision = decision;
        _decisionData = decisionData;
        _falseDecision = falseDecision;
        _falseDecisionData = falseDecisionData;
        _optionalData = optionalData;
        _diagnostics = diagnostics;
    }

    internal static INumberStep Create()
    {
        int number = 0;
        string name = string.Empty;
        string context = string.Empty;
        string statement = string.Empty;
        string decision = string.Empty;
        object? decisionData = null;
        string falseDecision = string.Empty;
        object? falseDecisionData = null;
        object? optionalData = null;
        var diagnostics = RightRuleConfiguration.DEFAULT_DIAGNOSTICS;

        return new RuleBuilder(number, name, context, statement, decision, decisionData, falseDecision, falseDecisionData, optionalData, diagnostics);
    }

    // Step 1: Enforces that Number must be provided first
    public INameStep Number(int number)
    {
        return new RuleBuilder(number, _name, _context, _statement, _decision, _decisionData, _falseDecision, _falseDecisionData, _optionalData, _diagnostics);
    }

    // Step 2: Enforces that Name must follow Number
    public IContextStep Name(string name)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Name is mandatory.", nameof(name));
        return new RuleBuilder(_number, name, _context, _statement, _decision, _decisionData, _falseDecision, _falseDecisionData, _optionalData, _diagnostics);
    }

    // Step 3: Enforces that Context must follow Name
    public IStatementStep Context(string context)
    {
        if (string.IsNullOrWhiteSpace(context)) throw new ArgumentException("Context is mandatory.", nameof(context));
        return new RuleBuilder(_number, _name, context, _statement, _decision, _decisionData, _falseDecision, _falseDecisionData, _optionalData, _diagnostics);
    }

    // Step 4: Enforces that Statement must follow Context
    public IDecisionStep Statement(string statement)
    {
        if (string.IsNullOrWhiteSpace(statement)) throw new ArgumentException("Statement is mandatory.", nameof(statement));
        return new RuleBuilder(_number, _name, _context, statement, _decision, _decisionData, _falseDecision, _falseDecisionData, _optionalData, _diagnostics);
    }

    // Step 5: Enforces that Decision must follow Statement
    public IFalseDecisionStep Decision(string decision)
    {
        if (string.IsNullOrWhiteSpace(decision)) throw new ArgumentException("Decision is mandatory.", nameof(decision));
        return new RuleBuilder(_number, _name, _context, _statement, decision, _decisionData, _falseDecision, _falseDecisionData, _optionalData, _diagnostics);
    }

    // Step 6: Enforces that False Decision must follow Decision
    public IRuleBuilder FalseDecision(string falseDecision)
    {
        if (string.IsNullOrWhiteSpace(falseDecision)) throw new ArgumentException("False Decision is mandatory.", nameof(falseDecision));
        return new RuleBuilder(_number, _name, _context, _statement, _decision, _decisionData, falseDecision, _falseDecisionData, _optionalData, _diagnostics);
    }

    // Unordered Mandatory / Optional Methods
    public IRuleBuilder FalseDecisionData(object? falseDecisionData)
    {
        return new RuleBuilder(_number, _name, _context, _statement, _decision, _decisionData, _falseDecision, falseDecisionData, _optionalData, _diagnostics);
    }

    // Unordered Mandatory / Optional Methods
    public IRuleBuilder DecisionData(object? decisionData)
    {
        return new RuleBuilder(_number, _name, _context, _statement, _decision, decisionData, _falseDecision, _falseDecisionData, _optionalData, _diagnostics);
    }

    public IRuleBuilder OptionalData(object? optionalData)
    {
        return new RuleBuilder(_number, _name, _context, _statement, _decision, _decisionData, _falseDecision, _falseDecisionData, optionalData,
          _diagnostics);
    }

    public IRuleBuilder WithDiagnostics(bool diagnostics = RightRuleConfiguration.DEFAULT_DIAGNOSTICS)
    {
        return new RuleBuilder(_number, _name, _context, _statement, _decision, _decisionData, _falseDecision, _falseDecisionData, _optionalData, diagnostics);
    }

    // Terminal step: validates that remaining mandatory variables are populated before finishing
    public DefinedRule Build()
    {
        if (_number.Equals(0)) throw new InvalidOperationException("Number is a mandatory field.");
        if (string.IsNullOrWhiteSpace(_name)) throw new InvalidOperationException("Name is a mandatory field.");
        if (string.IsNullOrWhiteSpace(_context)) throw new InvalidOperationException("Context is a mandatory field.");
        if (string.IsNullOrWhiteSpace(_statement)) throw new InvalidOperationException("Statement is a mandatory field.");
        if (string.IsNullOrWhiteSpace(_decision)) throw new InvalidOperationException("Decision is a mandatory field.");
        if (string.IsNullOrWhiteSpace(_falseDecision)) throw new InvalidOperationException("False Decision is a mandatory field.");

        RightRuleConfiguration ruleConfiguration = new RightRuleConfiguration(_diagnostics);

        return DefinedRule.Build(_number, _name, _context, _statement, _decision, _decisionData, _falseDecision, _falseDecisionData, _optionalData, ruleConfiguration);
    }
}
