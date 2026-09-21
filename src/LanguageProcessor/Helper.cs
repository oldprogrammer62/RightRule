using System.Globalization;

namespace RightRule.LanguageProcessor;

internal class Helper
{
    internal static object ParseBoolean(string Value)
    {
        if (bool.TryParse(Value, out var value))
            return value;
        return Value;
    }

    internal static object ParseNumber(string Value)
    {
        // Prefer int, fall back to double
        if (int.TryParse(Value, NumberStyles.Integer, CultureInfo.InvariantCulture, out var i))
            return i;
        if (double.TryParse(Value, NumberStyles.Float, CultureInfo.InvariantCulture, out var d))
            return d;
        return Value; // fallback: treat as string if parsing fails
    }
}
