using RightRule.Models.RuleModels;
using RightRule.RegistrationServices.RuleRegistration;

namespace RightRule.RuleApplication;

public static class Helpers
{
    internal static IRegisteredRules FilterRules(string Context, List<ParsedRule> Rules)
    {
        try
        {
            var filteredRules = Rules.Where
                (
                parsedRule => 
                parsedRule.Context.Equals(Context, StringComparison.InvariantCultureIgnoreCase)
                &&
                parsedRule.Status == RegistrationStatus.SUCCESS
                )
                .OrderBy(pr => pr.Number)
                .ToList();
            return new RegisteredRules(filteredRules);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error filtering rules for context '{Context}': {ex.Message}", ex);
        }
    }
}
