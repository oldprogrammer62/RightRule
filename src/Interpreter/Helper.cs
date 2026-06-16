using System.Globalization;

namespace RightRule.Interpreter;

internal class Helper
{
    internal static string ResolveOperatorValue(Operand operand, Dictionary<string, object> Arguments)
    {
        char brace = '{';
        if (operand.Raw.Contains(brace))
        {
            return GetOperatorValue(operand.Raw, Arguments);
        }
        return operand.Raw;
    }

    private static string GetOperatorValue(string Op, Dictionary<string, object> Arguments)
    {
        var operatorName = GetOperatorName(Op);

        if (Arguments.TryGetValue(operatorName, out var value))
        {
            return value.ToString() ?? string.Empty;
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
