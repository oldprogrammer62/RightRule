using RightRule.Rules;

namespace RightRule.HelperFunctions;

internal static class Helpers
{
    internal static List<ParsedRule> FilterRules(string Context, List<ParsedRule> Rules)
    {
        var filteredRules = Rules.Where(x => x.Context.Equals(Context, StringComparison.InvariantCultureIgnoreCase)).ToList();
        return filteredRules;
    }
}
