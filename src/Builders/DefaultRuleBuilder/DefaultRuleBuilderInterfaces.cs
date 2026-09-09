using RightRule.Models.RuleModels;

namespace RightRule.Builders.RightRule;

// Enforces step 1
public interface IDefaultNameStep
{
    IDefaultContextStep Name(string name);
}

// Enforces step 2
public interface IDefaultContextStep
{
    IDefaultDecisionStep Context(string context);
}

// Enforces step 3
public interface IDefaultDecisionStep
{
    IDefaultRuleBuilder Decision(string decision);
}

// Optional steps
public interface IDefaultRuleBuilder
{
    IDefaultRuleBuilder DecisionData(object? decisionData);

    DefaultRule Build();
}
