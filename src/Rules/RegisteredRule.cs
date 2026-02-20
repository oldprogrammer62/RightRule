using System.Diagnostics.CodeAnalysis;

namespace RightRule.Rules;

public sealed class RegisteredRule : Rule
{
    internal RegisteredRule()
    {
    }

    [SetsRequiredMembers]
    internal RegisteredRule(ParsedRule rule)
    {
        Number = rule.Number;
        Name = rule.Name;
        Statement = rule.Statement;
        Context = rule.Context;
        Decision = rule.Decision;
        DecisionData = rule.DecisionData;
    }
}
