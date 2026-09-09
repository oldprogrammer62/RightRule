using RightRule.Models;
using RightRule.Models.RuleModels;

namespace RightRule.Builders.RightRuleBuilder;

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
    IFalseDecisionStep Decision(string decision);
}

// Enforces step 6
public interface IFalseDecisionStep
{
    IRuleBuilder FalseDecision(string decision);
}

// Unlocks remaining options in any order
public interface IRuleBuilder
{
    IRuleBuilder DecisionData(object? decisionData);
    IRuleBuilder FalseDecisionData(object? decisionData);
    IRuleBuilder OptionalData(object? optionalData);
    IRuleBuilder WithDataCheck(bool checkData = RightRuleConfiguration.DEFAULT_DATACHECK);
    IRuleBuilder WithDiagnostics(bool diagnostics = RightRuleConfiguration.DEFAULT_DIAGNOSTICS);

    DefinedRule Build();
}
