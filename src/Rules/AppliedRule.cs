using System.Diagnostics.CodeAnalysis;

namespace RightRule.Rules;

public sealed class AppliedRule : Rule
{
    public required string DatasetMatched { get; init; }

    internal static readonly string NoDecision = "NODECISION";

    public AppliedRule() {}

    [SetsRequiredMembers]
    internal AppliedRule(ParsedRule parsedRule, string datasetMatched)
    {
        Arguments = parsedRule.Arguments;
        Number = parsedRule.Number;
        Name = parsedRule.Name;
        Statement = parsedRule.Statement;
        Context = parsedRule.Context;
        Decision = parsedRule.Decision;
        DecisionData = parsedRule.DecisionData;
        DatasetMatched = datasetMatched;
    }

    public static AppliedRule DefaultRule()
    {
        return new AppliedRule()
        {
            Arguments = null,
            Number = -1,
            Name = "DefaultRule",
            Statement = string.Empty,
            Context = string.Empty,
            Decision = NoDecision,
            DecisionData = null,
            DatasetMatched = NoDecision
        };
    }
}
