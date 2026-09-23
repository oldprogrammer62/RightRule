using RightRule.RuleServices.RuleModels;
using RightRule.RuleServices.RuleRegistration;
using RightRule.RuleServices.Rules;

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
                parsedRule.Status == RegistrationStatusEnum.SUCCESS
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
