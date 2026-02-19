using RightRule.Rules;

namespace RightRule.HelperFunctions;

internal static class Helpers
{
    internal static List<Rule> FilterRules(string Context, List<Rule> Rules)
    {
        var filteredRules = Rules.Where(x => x.Context.Equals(Context, StringComparison.InvariantCultureIgnoreCase)).ToList();
        return filteredRules;
    }
}
