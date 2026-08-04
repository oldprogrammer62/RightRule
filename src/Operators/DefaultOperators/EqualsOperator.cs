using RightRule.System.Common;

namespace RightRule.Operators.DefaultOperators;

public class EqualsOperator : Operator
{
    public override List<string> Identifiers { get; } = ["equals", "eq", "=", "=="];

    public EqualsOperator() : base(false)
    {
    }

    protected override async Task<EvaluationResult> DoOperatorProcessAsync(object left, object right, object? OperatorConfig)
    {
        if (left == null && right == null)
        {
            return EvaluationResult.Failure("One or both operands are null.");
        }
        if (left == null || right == null)
        {
            return EvaluationResult.Failure("One or both operands are null.");
        }

        if (IsNumber(left) && IsNumber(right)) return EvaluationResult.Success(ProcessAsNumber(left, right));
        if (IsString(left) && IsString(right)) return EvaluationResult.Success(ProcessAsString(left, right));
        if (IsDateTime(left) && IsDateTime(right)) return EvaluationResult.Success(ProcessAsDateTime(left, right));
        if (IsDateOnly(left) && IsDateOnly(right)) return EvaluationResult.Success(ProcessAsDateOnly(left, right));

        return EvaluationResult.Success(Equals(left, right));
    }

    private static new bool Equals(dynamic left, dynamic right)
    {
        dynamic x = left == right;
        return x;
    }

    private static bool ProcessAsDateOnly(object left, object right)
    {
        var dtLeft = ConvertToDateOnly(left);
        var dtRight = ConvertToDateOnly(right);
        if (dtLeft != null && dtRight != null)
        {
            return Equals(dtLeft, dtRight);
        }
        return false;
    }

    private static bool ProcessAsDateTime(object left, object right)
    {
        var dtLeft = ConvertToDateTime(left);
        var dtRight = ConvertToDateTime(right);
        if (dtLeft != null && dtRight != null)
        {
            return Equals(dtLeft, dtRight);
        }
        return false;
    }

    private static DateOnly? ConvertToDateOnly(object input)
    {
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

    private static bool ProcessAsNumber(object left, object right)
    {
        var decLeft = ConvertToNumber(left);
        var decRight = ConvertToNumber(right);
        return Equals(decLeft, decRight);
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

    private static bool IsString(object value)
    {
        if (value is null) return false;

        return value is string;
    }

    private static bool ProcessAsString(object left, object right)
    {
        string sLeft = left?.ToString()?.ToUpper(global::System.Globalization.CultureInfo.CurrentCulture) ?? string.Empty;
        string sRight = right?.ToString()?.ToUpper(global::System.Globalization.CultureInfo.CurrentCulture) ?? string.Empty;
        return Equals(sLeft, sRight);
    }
}
