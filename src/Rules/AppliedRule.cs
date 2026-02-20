using System.Diagnostics.CodeAnalysis;

namespace RightRule.Rules
{
    public class AppliedRule
    {
        public required int Number { get; init; }
        public required string Name { get; init; }
        public required string Statement { get; init; }
        public required string Context { get; init; }
        public required string Decision { get; init; }
        public required object? DecisionData { get; init; }

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

        public AppliedRule()
        {
        }

        [SetsRequiredMembers]
        public AppliedRule(ParsedRule parsedRule)
        {
            Number = parsedRule.Number;
            Name = parsedRule.Name;
            Statement = parsedRule.Statement;
            Context = parsedRule.Context;
            Decision = parsedRule.Decision;
            DecisionData = parsedRule.DecisionData;
        }
    }
}
