using RightRule.Rules;

namespace RightRule.HelperFunctions;

public static class Helpers
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

    internal static T DeserialiseJson<T>(string Json)
    {
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
