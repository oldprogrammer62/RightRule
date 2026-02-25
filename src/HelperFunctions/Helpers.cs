using RightRule.Rules;

namespace RightRule.HelperFunctions;

internal static class Helpers
{
    internal static ParsedRules FilterRules(string Context, List<ParsedRule> Rules)
    {
        var filteredRules = Rules.Where
            (
            x => x.Context.Equals(Context, StringComparison.InvariantCultureIgnoreCase)
            )
            .OrderBy( x => x.Number )
            .ToList();
        return new ParsedRules(filteredRules);
    }
}
