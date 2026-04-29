using System.Diagnostics.CodeAnalysis;

namespace RightRule.Rules;

public sealed class AppliedRule : Rule
{
    public required string DatasetMatched { get; init; }

    internal static readonly string NoDecision = "NODECISION";
    internal static readonly string NoDatasetMatched = "NO_DATASET_MATCHED";

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
        return DefaultRule(string.Empty);
    }

    public static AppliedRule DefaultRule(string decisionData)
    {
        return new AppliedRule()
        {
            Arguments = null,
            Number = -1,
            Name = "DEFAULTRULE",
            Statement = string.Empty,
            Context = string.Empty,
            Decision = NoDecision,
            DecisionData = decisionData,
            DatasetMatched = NoDatasetMatched
        };
    }
}
