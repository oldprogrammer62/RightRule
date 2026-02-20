using System.Diagnostics.CodeAnalysis;

namespace RightRule.Rules;

public sealed class AppliedRule : Rule
{
    public static readonly string NoDecision = "NODECISION";

    internal AppliedRule()
    {
    }

    [SetsRequiredMembers]
    internal AppliedRule(ParsedRule parsedRule)
    {
        Number = parsedRule.Number;
        Name = parsedRule.Name;
        Statement = parsedRule.Statement;
        Context = parsedRule.Context;
        Decision = parsedRule.Decision;
        DecisionData = parsedRule.DecisionData;
    }

    public static AppliedRule DefaultRule()
    {
        return new AppliedRule()
        {
            Number = -1,
            Name = "DefaultRule",
            Statement = string.Empty,
            Context = string.Empty,
            Decision = "NODECISION",
            DecisionData = null
        };
    }
}
