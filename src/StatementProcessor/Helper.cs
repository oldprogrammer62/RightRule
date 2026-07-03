using System.Globalization;

namespace RightRule.StatementProcessor;

internal class Helper
{
    internal static string ResolveStatement(string RuleStatement, Dictionary<string, object> Arguments)
    {
        if (Arguments == null || Arguments.Count == 0)
        {
            return RuleStatement;
        }

        char brace = '{';
        if (!RuleStatement.Contains(brace))
        {
            return RuleStatement;
        }

        int start = RuleStatement.IndexOf('{');
        int end = RuleStatement.IndexOf('}');

        // Extract the key name inside the braces
        string key = RuleStatement.Substring(start + 1, end - start - 1);
        string placeholder = RuleStatement.Substring(start, end - start + 1);

        // Get the replacement value or keep the placeholder if key is missing
        string replacement = Arguments.TryGetValue(key, out object? value) ? value.ToString() : placeholder;

        // Replace only the first occurrence
        string updatedInput = RuleStatement.Substring(0, start) + replacement + RuleStatement.Substring(end + 1);

        // Recursive call for the rest of the string
        return ResolveStatement(updatedInput, Arguments);
    }

    internal static object ResolveOperatorValue(Operand operand, Dictionary<string, object> Arguments)
    {
        char brace = '{';
        if (operand.Raw.Contains(brace))
        {
            return GetOperatorValue(operand.Raw, Arguments);
        }
        return operand.Raw;
    }

    private static object GetOperatorValue(string Op, Dictionary<string, object> Arguments)
    {
        var operatorName = GetOperatorName(Op);

        if (Arguments.TryGetValue(operatorName, out var value))
        {
            return value ?? string.Empty;
        }
        return string.Empty;
    }

    internal static string GetOperatorName(string Op)
    {
        char[] StartChar = { '{' };
        char[] EndChar = { '}' };

        return Op.TrimStart(StartChar).TrimEnd(EndChar);
    }

    internal static object ParsePlaceholder(string raw)
    {
        if (bool.TryParse(raw, out var value))
            return value;

        // Prefer int, fall back to double
        if (int.TryParse(raw, NumberStyles.Integer, CultureInfo.InvariantCulture, out var i))
            return i;
        if (double.TryParse(raw, NumberStyles.Float, CultureInfo.InvariantCulture, out var d))
            return d;

        // Assume string
        return raw;
    }

    internal static object ParseBoolean(string raw)
    {
        if (bool.TryParse(raw, out var value))
            return value;
        return raw;
    }

    internal static object ParseNumber(string raw)
    {
        // Prefer int, fall back to double
        if (int.TryParse(raw, NumberStyles.Integer, CultureInfo.InvariantCulture, out var i))
            return i;
        if (double.TryParse(raw, NumberStyles.Float, CultureInfo.InvariantCulture, out var d))
            return d;
        return raw; // fallback: treat as string if parsing fails
    }
}
