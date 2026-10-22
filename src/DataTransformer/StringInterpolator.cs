using RightRule.DataServices;
using System.Text.RegularExpressions;

namespace RightRule.DataTransformer;

internal class StringInterpolator : IInterpolate
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
        var workingString = ProcessDottedIdentifiers(StringToTransform);
        return ProcessNoDotIdentifiers(workingString);
    }

    private string ProcessDottedIdentifiers(string StringToTransform)
    {
        var workingString = StringToTransform;
        var replaceables = FindDottedIdentifiers(StringToTransform);
        if (replaceables.Count == 0)
        {
            return StringToTransform;
        }
        foreach (var replaceable in replaceables)
        {
            var replacement = GetReplacement(replaceable.Substring(1));
            if (replacement.found)
            {
                workingString = workingString.Replace(replaceable, replacement.value);
            }
        }
        return workingString;
    }

    private string ProcessNoDotIdentifiers(string StringToTransform)
    {
        var workingString = StringToTransform;
        var replaceables = FindNoDotIdentifiers(StringToTransform);
        if (replaceables.Count == 0)
        {
            return StringToTransform;
        }
        foreach (var replaceable in replaceables)
        {
            var replacement = GetReplacement(replaceable.Substring(1));
            if (replacement.found)
            {
                workingString = workingString.Replace(replaceable, replacement.value);
            }
        }
        return workingString;
    }

    private (bool found, string? value) GetReplacement(string replaceable)
    {
        if (replaceable.Contains('.'))
        {
            return GetReplacementDotted(replaceable);
        }
        return GetReplacementNoDot(replaceable);
    }

    private (bool found, string? value) GetReplacementDotted(string replaceable)
    {
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

    private (bool found, string? value) GetReplacementNoDot(string replaceable)
    {
        // Dataset and Data Item name are same for simple variable registration
        var value = DataService.GetDataItemValue(replaceable, replaceable);
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

    public static List<string> FindNoDotIdentifiers(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return new List<string>();

        // \$                  --> Matches literal '$'
        // [a-zA-Z0-9_]+       --> Matches letters, numbers, or underscores (No dots allowed)
        // (?=\s|[^a-zA-Z]|$)  --> Lookahead: Ends with a space, non-alpha symbol, or end of string
        string pattern = @"\$[a-zA-Z0-9_]+(?=\s|[^a-zA-Z]|$)";

        MatchCollection matches = Regex.Matches(input, pattern);
        return matches.Cast<Match>()
                    .Select(m => m.Value.TrimEnd('.', ',', '!', '?', ';'))
                    .Select(s => s.Replace("\"", ""))
                    .ToList();
    }

    public static List<string> FindDottedIdentifiers(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return new List<string>();

        // \$                  --> Matches literal '$'
        // [a-zA-Z0-9_]+       --> Characters before the dot
        // \.                  --> Literal middle dot
        // [a-zA-Z0-9_]+       --> Characters after the dot
        // (?=\s|[^a-zA-Z]|$)  --> Lookahead: Ends with a space, non-alpha symbol, or end of string
        string pattern = @"\$[a-zA-Z0-9_]+\.[a-zA-Z0-9_]+(?=\s|[^a-zA-Z]|$)";

        MatchCollection matches = Regex.Matches(input, pattern);
        return matches.Cast<Match>()
                    .Select(m => m.Value.TrimEnd('.', ',', '!', '?', ';'))
                    .Select(s => s.Replace("\"", ""))
                    .ToList();
    }
}
