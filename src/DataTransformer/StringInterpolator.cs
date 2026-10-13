using RightRule.DataServices;
using System.Text.RegularExpressions;

namespace RightRule.DataTransformer;

public class StringInterpolator : IInterpolate
{
    private const char variablePointer = '$';

    private readonly IDataService DataService;

    public StringInterpolator(IDataService dataService)
    {
        DataService = dataService;
    }

    public object? Interpolate<T>(T DataToTransform)
    {
        if (DataToTransform is string data)
        {
            if (!ContainsReplaceables(data))
            {
                return data;
            }
            return TransformString(data);
        }
        return DataToTransform;
    }

    private string TransformString(string StringToTransform)
    {
        var workingString = StringToTransform;
        var replaceables = FindReplaceableWords(StringToTransform);
        if (replaceables.Count == 0)
        {
            return StringToTransform;
        }
        foreach (var replaceable in replaceables)
        {
            var replacement = GetReplacement(replaceable);
            if (replacement.found)
            {
                workingString = workingString.Replace(replaceable, replacement.value);
            }
        }
        return workingString;
    }

    private (bool found, string? value) GetReplacement(string replaceable)
    {
        // Remove pointer $
        replaceable = replaceable.Substring(1);

        // Needs to be dateset.field
        var elements = replaceable.Split('.');
        if (elements.Length != 2)
        {
            return (false, replaceable);
        }

        var value = DataService.GetDataItemValue(elements[0], elements[1]);
        if (value == null)
        {
            return (false, replaceable);
        }
        return (true, value.ToString());
    }

    private bool ContainsReplaceables(string StringToTransform)
    {
        return StringToTransform.Contains(variablePointer);
    }

    public static List<string> FindReplaceableWords(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return new List<string>();

        // Pattern breakdown:
        // (?<=\s|^)  --> Must start at the beginning of the string or after a space
        // \$         --> Matches the literal '$' sign
        // \S+?       --> Lazily matches any non-whitespace characters before the dot
        // \.         --> Matches the literal middle dot
        // \S+        --> Matches any non-whitespace characters after the dot
        string pattern = @"(?<=\s|^)\$\S+?\.[\w\S]+";

        return Regex.Matches(input, pattern)
                    .Cast<Match>()
                    .Select(m => m.Value.TrimEnd('.', ',', '!', '?')) // Cleans trailing punctuation from sentences
                    .ToList();
    }
}
