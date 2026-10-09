using RightRule.System.Common.Operator.Models;

namespace RightRule.OperatorServices.OperatorLoader;

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

    internal static Dictionary<string, Operator> MakeCleanAliases(Operator Operator)
    {
        // Make sure the Operator specifies Aliases
        var aliases = Operator.Aliases;
        if (aliases is null || aliases.Count == 0)
        {
            return [];
        }
        var aliasesToRegister = CleanAliases(aliases);
        return aliasesToRegister.ToDictionary(alias => alias, alias => Operator);
    }

    internal static string[] CleanAliases(List<string> aliases)
    {
        // Convert to array for further processing
        var aliasesToRegister = aliases.ToArray();

        // Remove any null, empty, or whitespace identifiers
        if (aliasesToRegister.Any(string.IsNullOrWhiteSpace))
        {
            aliasesToRegister = aliasesToRegister
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .ToArray();
        }
        return aliasesToRegister.Distinct(StringComparer.OrdinalIgnoreCase).ToArray();
    }

    internal static bool OperatorAliasesUnique(List<Operator> Operators)
    {
        if (Operators == null || Operators.Count == 0)
            return true;

        // Flatten all identifiers, ignore null/whitespace, compare case-insensitively.
        var hasDuplicates = Operators
            .Where(op => op?.Aliases != null)
            .SelectMany(op => op.Aliases)
            .Where(id => !string.IsNullOrWhiteSpace(id))
            .GroupBy(id => id, StringComparer.OrdinalIgnoreCase)
            .Any(g => g.Count() > 1);

        return !hasDuplicates;
    }

    internal static bool OperatorAliasesUnique(Dictionary<string, Operator> RegisteredAliases, string[] OperatorAliases)
    {
        if (RegisteredAliases == null || RegisteredAliases.Count == 0)
            return true;

        return !OperatorAliases.Any(alias => RegisteredAliases.ContainsKey(alias));
    }

    internal static bool OperatorNameExists(List<Operator> Operators, Operator Operator)
    {
        return Operators.Any(existingOperator => ReferenceEquals(existingOperator.Name, Operator.Name));
    }
}
