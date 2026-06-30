using RightRule.System.Common;

namespace RightRule.Operators.DefaultOperators;

public sealed class NotEqualsOperator : IOperator
{
    public List<string> Identifiers => ["notequals", "neq", "!=", "<>"];

    public bool Process(object left, object right)
    {
        if (IsDateTime(left) && IsDateTime(right)) return !ProcessAsDateTime(left, right);
        if (IsDateOnly(left) && IsDateOnly(right)) return !ProcessAsDateOnly(left, right);

        return !Equals(left, right);
    }

    public void Dispose()
    {
        // Nothing to dispose
    }

    private bool ProcessAsDateOnly(object left, object right)
    {
        var dtLeft = ConvertToDateOnly(left);
        var dtRight = ConvertToDateOnly(right);
        return Equals(dtLeft, dtRight);
    }

    private bool ProcessAsDateTime(object left, object right)
    {
        var dtLeft = ConvertToDateTime(left);
        var dtRight = ConvertToDateTime(right);
        return Equals(dtLeft, dtRight);
    }

    private static DateOnly? ConvertToDateOnly(object input)
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

    private static DateTime? ConvertToDateTime(object input)
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
}
