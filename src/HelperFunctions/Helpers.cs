using RightRule.Rules;

namespace RightRule.HelperFunctions;

public static class Helpers
{
    internal static RegisteredRules FilterRules(string Context, List<ParsedRule> Rules)
    {
        try
        {
            var filteredRules = Rules.Where
                (
                x => x.Context.Equals(Context, StringComparison.InvariantCultureIgnoreCase)
                )
                .OrderBy(x => x.Number)
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

    public static bool IsDictionary(Type type, out Type? keyType, out Type? valueType)
    {
        ArgumentNullException.ThrowIfNull(type, nameof(type));

        if (IsIDictionaryType(type))
        {
            var genericArgs = GetGenericArguments(type);
            keyType = genericArgs[0];
            valueType = genericArgs[1];
            return true;
        }
        var dictInterface = GetDictInterface(type);

        if (dictInterface != null)
        {
            var genericArgs = GetGenericIDictionaryArgs(dictInterface);
            keyType = genericArgs[0];
            valueType = genericArgs[1];
            return true;
        }
        keyType = null;
        valueType = null;
        return false;
    }

    /// <summary>
    /// Determines whether <paramref name="type"/> is an enumerable of T (IEnumerable&lt;T&gt; or T[]).
    /// Excludes <see cref="string"/>.
    /// </summary>
    /// <param name="type">Type to inspect.</param>
    /// <param name="elementType">The element type T when true; otherwise null.</param>
    /// <returns>True if <paramref name="type"/> represents an IEnumerable&lt;T&gt; or array; otherwise false.</returns>
    public static bool IsTEnumerable(Type type, out Type? elementType)
    {
        ArgumentNullException.ThrowIfNull(type, nameof(type));

        // Exclude string which is IEnumerable<char> but usually not intended as a collection type here.
        if (type == typeof(string))
        {
            elementType = null;
            return false;
        }

        // Arrays -> element type is the array element
        if (type.IsArray)
        {
            elementType = type.GetElementType();
            return elementType != null;
        }

        if (IsIEnumerableType(type))
        {
            elementType = GetGenericArguments(type, 0);
            return true;
        }
        var enumerableInterface = GetEnumerableInterface(type);

        if (enumerableInterface != null)
        {
            elementType = GetGenericArguments(enumerableInterface, 0);
            return true;
        }

        elementType = null;
        return false;
    }

    private static Type? GetEnumerableInterface(Type type)
    {
        ArgumentNullException.ThrowIfNull(type, nameof(type));

        try { 
            return type.GetInterfaces()
                     .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                     .FirstOrDefault();
        }
        catch (Exception ex)
        {
            throw new Exception($"Error retrieving IEnumerable interface from type '{type.FullName}': {ex.Message}", ex);
        }
    }

    private static Type GetGenericArguments(Type type, int Index)
    {
        try
        {
            return GetGenericArguments(type)[Index];
        }
        catch (Exception ex)
        {
            throw new Exception($"Error retrieving generic argument at index {Index} from type '{type.FullName}': {ex.Message}", ex);
        }
    }

    private static Type[] GetGenericArguments(Type type)
    {
        try
        {
            return type.GetGenericArguments();
        }
        catch (Exception ex)
        {
            throw new Exception($"Error retrieving generic arguments from type '{type.FullName}': {ex.Message}", ex);
        }
    }

    private static bool IsIDictionaryType(Type type)
    {
        try 
        { 
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IDictionary<,>);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error checking if type '{type.FullName}' is IDictionary: {ex.Message}", ex);
        }
    }

    private static bool IsIEnumerableType(Type type)
    {
        try
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IEnumerable<>);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error checking if type '{type.FullName}' is IEnumerable: {ex.Message}", ex);
        }
    }

    private static Type? GetDictInterface(Type type)
    {
        try
        {
            return type.GetInterfaces()
                    .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IDictionary<,>))
                    .FirstOrDefault();
        }
        catch (Exception ex)
        {
            throw new Exception($"Error retrieving IDictionary interface from type '{type.FullName}': {ex.Message}", ex);
        }
    }

    private static Type[] GetGenericIDictionaryArgs(Type dictInterface)
    {
        try
        {
            return dictInterface.GetGenericArguments();
        }
        catch (Exception ex)
        {
            throw new Exception($"Error retrieving generic arguments from IDictionary interface '{dictInterface.FullName}': {ex.Message}", ex);
        }
    }
}
