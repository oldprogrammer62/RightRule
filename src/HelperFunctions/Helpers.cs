using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
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

    public static string GetValueFromSettings(string key)
    {
        string? val = ConfigurationManager.AppSettings[key];
        return val ?? throw new Exception($"Key '{key}' not found in app settings.");
    }

    public static bool IsDictionary(Type type, out Type? keyType, out Type? valueType)
    {
        if (type == null) throw new ArgumentNullException(nameof(type));
        if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IDictionary<,>))
        {
            var genericArgs = type.GetGenericArguments();
            keyType = genericArgs[0];
            valueType = genericArgs[1];
            return true;
        }
        var dictInterface = type.GetInterfaces()
                                .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IDictionary<,>))
                                .FirstOrDefault();
        if (dictInterface != null)
        {
            var genericArgs = dictInterface.GetGenericArguments();
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
        if (type == null) throw new ArgumentNullException(nameof(type));

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

        // If the type itself is IEnumerable<T>
        if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IEnumerable<>))
        {
            elementType = type.GetGenericArguments()[0];
            return true;
        }

        // Check implemented interfaces for IEnumerable<T>
        var ie = type.GetInterfaces()
                     .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                     .FirstOrDefault();

        if (ie != null)
        {
            elementType = ie.GetGenericArguments()[0];
            return true;
        }

        elementType = null;
        return false;
    }
}
