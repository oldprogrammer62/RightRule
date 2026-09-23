using RightRule.System.Common;

namespace RightRule.Operators.DefaultOperators;

public class EqualsOperator : Operator
{
    public override List<string> Identifiers { get; } = ["equals", "eq", "=", "=="];

    public EqualsOperator() : base(false)
    {
    }

    protected override async Task<OperatorResult> DoOperatorProcessAsync(object Left, object Right, object? OptionalData, bool Diagnostics)
    {
        if (Left == null && Right == null)
        {
            return OperatorResult.FailureResult(this.Name, "One or both operands are null.");
        }
        if (Left == null || Right == null)
        {
            return OperatorResult.FailureResult(this.Name, "One or both operands are null.");
        }

        if (IsNumber(Left) && IsNumber(Right)) return OperatorResult.SuccessResult(this.Name, ProcessAsNumber(Left, Right));
        if (IsString(Left) && IsString(Right)) return OperatorResult.SuccessResult(this.Name, ProcessAsString(Left, Right));
        if (IsChar(Left) && IsChar(Right)) return OperatorResult.SuccessResult(this.Name, ProcessAsChar(Left, Right));
        if (IsBoolean(Left) && IsBoolean(Right)) return OperatorResult.SuccessResult(this.Name, ProcessAsBoolean(Left, Right));
        if (IsDateTime(Left) && IsDateTime(Right)) return OperatorResult.SuccessResult(this.Name, ProcessAsDateTime(Left, Right));
        if (IsDateOnly(Left) && IsDateOnly(Right)) return OperatorResult.SuccessResult(this.Name, ProcessAsDateOnly(Left, Right));
        if (IsTimeOnly(Left) && IsTimeOnly(Right)) return OperatorResult.SuccessResult(this.Name, ProcessAsTimeOnly(Left, Right));
        return OperatorResult.SuccessResult(this.Name, Equals(Left, Right));
    }

    private static new bool Equals(dynamic Left, dynamic Right)
    {
        try
        {
            dynamic x = Left == Right;
            return x;
        }
        catch
        {
            return false;
        }
    }

    #region NUMBER
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

    private static bool ProcessAsNumber(object left, object right)
    {
        var decLeft = ConvertToNumber(left);
        var decRight = ConvertToNumber(right);
        return decLeft.Equals(decRight);
    }

    private static double ConvertToNumber(object value)
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
        if (value is float f) return f;
        if (value is Single si) return si;
        if (value is double d) return d;
        if (value is decimal dec) return Convert.ToDouble(dec);
        return default;
    }
    #endregion NUMBER

    #region STRING
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
    #endregion STRING

    #region CHAR
    private static bool IsChar(object value)
    {
        if (value is null) return false;
        if (value is string && value.ToString()!.Length == 1)
        {
            return true;
        }
        return value is char;
    }

    private static bool ProcessAsChar(object left, object right)
    {
        string sLeft = left?.ToString()?.ToUpper(global::System.Globalization.CultureInfo.CurrentCulture) ?? string.Empty;
        string sRight = right?.ToString()?.ToUpper(global::System.Globalization.CultureInfo.CurrentCulture) ?? string.Empty;
        return Equals(sLeft, sRight);
    }
    #endregion CHAR

    #region BOOLEAN
    private static bool IsBoolean(object value)
    {
        if (value is bool) return true;

        // Safe parsing for text strings
        if (value is string stringInput)
        {
            string sTrue = "true";
            string sFalse = "false";
            if (stringInput.Equals(sTrue, StringComparison.Ordinal)) return true;
            if (stringInput.Equals(sFalse, StringComparison.Ordinal)) return true;
            return false;
        }
        return false;
    }

    private static bool ProcessAsBoolean(object left, object right)
    {
        var bLeft = ConvertToBoolean(left);
        var bRight = ConvertToBoolean(right);
        return Equals(bLeft, bRight);
    }

    private static bool ConvertToBoolean(object value)
    {
        if (value is bool b) return b;
        // Safe parsing for text strings
        if (value is string stringInput)
        {
            if (bool.TryParse(stringInput, out bool parsedResult))
            {
                return parsedResult;
            }
        }
        try
        {
            return Convert.ToBoolean(value);
        }
        catch
        {
            return false;
        }
    }
    #endregion BOOLEAN

    #region DATETIME
    private static bool IsDateTime(object value)
    {
        if (value is DateTime) return true;

        // Safe parsing for text strings
        if (value is string stringInput)
        {
            return DateTime.TryParse(stringInput, out _);
        }
        return false;
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
    #endregion DATETIME

    #region DATEONLY
    private static bool IsDateOnly(object value)
    {
        if (value is DateOnly) return true;

        // Safe parsing for text strings
        if (value is string stringInput)
        {
            return DateOnly.TryParse(stringInput, out _);
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
    #endregion DATEONLY

    #region TIMEONLY
    private static bool IsTimeOnly(object value)
    {
        if (value is TimeOnly) return true;

        // Safe parsing for text strings
        if (value is string stringInput)
        {
            return TimeOnly.TryParse(stringInput, out _);
        }
        return false;
    }

    private static TimeOnly? ConvertToTimeOnly(object input)
    {
        // Direct match
        if (input is TimeOnly timeOnly)
        {
            return timeOnly;
        }

        // Safe conversion for string representations
        if (input is string stringInput)
        {
            if (TimeOnly.TryParse(stringInput, out TimeOnly parsedResult))
            {
                return parsedResult;
            }
        }

        // Try ultimate fallback for other convertible types (like long timestamps)
        try
        {
            return (TimeOnly)input;
        }
        catch
        {
            return null;
        }
    }

    private static bool ProcessAsTimeOnly(object left, object right)
    {
        var tLeft = ConvertToTimeOnly(left);
        var tRight = ConvertToTimeOnly(right);
        if (tLeft != null && tRight != null)
        {
            return Equals(tLeft, tRight);
        }
        return false;
    }
    #endregion TIMEONLY
}
