namespace RightRule.RuleModels;

// Enforces step 1
public interface INumberStep
{
    INameStep Number(int number);
}

// Enforces step 2
public interface INameStep
{
    IContextStep Name(string name);
}

// Enforces step 3
public interface IContextStep
{
    IStatementStep Context(string context);
}

// Enforces step 4
public interface IStatementStep
{
    IDecisionStep Statement(string statement);
}

// Enforces step 5
public interface IDecisionStep
{
    IRuleBuilder Decision(string decision);
}

// Unlocks remaining options in any order
public interface IRuleBuilder
{
    // IRuleBuilder Statement(string statement);
    // IRuleBuilder Context(string context);
    // IRuleBuilder Decision(string decision);
    IRuleBuilder DecisionData(object? decisionData);
    IRuleBuilder OptionalData(object? optionalData);
    IRuleBuilder WithCheckData(bool checkData = RuleConfiguration.DEFAULT_DATACHECK);
    IRuleBuilder WithDiagnostics(bool diagnostics = RuleConfiguration.DEFAULT_DIAGNOSTICS);

    RuleDefinition Build();
}
