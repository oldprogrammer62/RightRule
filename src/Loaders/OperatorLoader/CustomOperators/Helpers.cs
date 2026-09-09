namespace RightRule.Loaders.OperatorLoader.CustomOperators;

internal class Helpers
{
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
