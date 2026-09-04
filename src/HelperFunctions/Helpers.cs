using RightRule.RuleModels;
using RightRule.RuleRegistration;
using RightRule.RuleRegistration.LogicRules;

namespace RightRule.HelperFunctions;

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

    internal static T DeserialiseJson<T>(string Json)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(Json, nameof(Json));

        var jsonSerializer = new Newtonsoft.Json.JsonSerializer();
        using (var jsonReader = new Newtonsoft.Json.JsonTextReader(new StringReader(Json)))
        {
            var deserializedObject = jsonSerializer.Deserialize<T>(jsonReader);
            if (deserializedObject == null)
                throw new Exception("Failed to deserialize JSON.");
            return deserializedObject;
        }
    }
}
