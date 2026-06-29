using RightRule.System.Common;

namespace RightRule.Operators.DefaultOperators;

public sealed class EqualsOperator : IOperator
{
    public List<string> Identifiers => ["equals", "eq", "=", "=="];
    public bool Process(object left, object right)
    {
        if (left is bool _ && right is bool _) return ProcessAsBool(left, right);

        if (left is string _ && right is string _) return ProcessAsString((string)left, (string)right);
        if (IsNumber(left) && IsNumber(right)) return ProcessAsNumber(left, right);

        if (IsDateTime(left) && IsDateTime(right)) return ProcessAsDateTime(left, right);
        if (IsDateOnly(left) && IsDateOnly(right)) return ProcessAsDateOnly(left, right);

        return ProcessAsString((string)left, (string)right);
    }

    public bool ProcessAsString(string left, string right)
    {
        return Equals(left, right);
    }

    public bool ProcessAsBool(object left, object right)
    {
        return Equals(left, right);
    }

    public bool ProcessAsNumber(object left, object right)
    {
        var decLeft = ConvertToNumber(left);
        var decRight = ConvertToNumber(right);
        return decLeft == decRight;
    }

    public bool ProcessAsDateOnly(object left, object right)
    {
        var dtLeft = ConvertToDateOnly(left);
        var dtRight = ConvertToDateOnly(right);
        return Equals(dtLeft, dtRight);
    }

    public bool ProcessAsDateTime(object left, object right)
    {
        var dtLeft = ConvertToDateTime(left);
        var dtRight = ConvertToDateTime(right);
        return Equals(dtLeft, dtRight);
    }

    public static DateOnly? ConvertToDateOnly(object input)
    {
        if (input == null)
        {
            return null;
        }

        // Direct match
        if (input is DateOnly dateOnly)
        {
            return dateOnly;
        }

        // Safe conversion for string representations
        if (input is string stringInput)
        {
            if (DateOnly.TryParse(stringInput, out DateOnly parsedResult))
            {
                return parsedResult;
            }
        }

        // Try ultimate fallback for other convertible types (like long timestamps)
        try
        {
            return (DateOnly)input;
        }
        catch
        {
            return null;
        }
    }

    public static DateTime? ConvertToDateTime(object input)
    {
        if (input == null)
        {
            return null;
        }

        // Direct match
        if (input is DateTime dateTime)
        {
            return dateTime;
        }

        // Safe extraction for DateOnly (presents as midnight)
        if (input is DateOnly dateOnly)
        {
            return dateOnly.ToDateTime(TimeOnly.MinValue);
        }

        // Safe conversion for DateTimeOffset
        if (input is DateTimeOffset dateTimeOffset)
        {
            return dateTimeOffset.DateTime;
        }

        // Safe conversion for string representations
        if (input is string stringInput)
        {
            if (DateTime.TryParse(stringInput, out DateTime parsedResult))
            {
                return parsedResult;
            }
        }

        // Try ultimate fallback for other convertible types (like long timestamps)
        try
        {
            return Convert.ToDateTime(input);
        }
        catch
        {
            return null;
        }
    }



    private static decimal ConvertToNumber(object value)
    {
        if (value is null) return default;

        if (value is sbyte sb) return sb;
        if (value is byte b) return b;
        if (value is short s) return s;
        if (value is ushort us) return us;
        if (value is int i) return i;
        if (value is uint ui) return ui;
        if (value is long l) return l;
        if (value is ulong ul) return ul;
        if (value is float f) return Convert.ToDecimal(f);
        if (value is double d) return Convert.ToDecimal(d);
        if (value is decimal dec) return dec;
        return default;
    }

    private static bool IsNumber(object value)
    {
        if (value is null) return false;

        return value is sbyte
                || value is byte
                || value is short
                || value is ushort
                || value is int
                || value is uint
                || value is long
                || value is ulong
                || value is float
                || value is double
                || value is decimal;
    }

    private static bool IsDateOnly(object value)
    {
        if (value is DateOnly) return true;

        // Safe parsing for text strings
        if (value is string stringInput)
        {
            return DateOnly.TryParse(stringInput, out _);
        }

        try
        {
            var result = Convert.ToDateTime(value);
            return true;
        }
        catch
        {
            return false;
        }
    }

    private static bool IsDateTime(object value)
    {
        if (value is DateTime) return true;

        // Safe parsing for text strings
        if (value is string stringInput)
        {
            return DateTime.TryParse(stringInput, out _);
        }

        try
        {
            var result = Convert.ToDateTime(value);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public void Dispose()
    {
        // Nothing to dispose
    }
}
