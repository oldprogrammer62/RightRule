using RightRule.System.Common;

namespace Operator.GreaterThanOrEquals;

public sealed class GreaterThanOrEquals : IOperator
{
    public List<string> Identifiers => ["greaterthanorequals", ">="];

    public bool Process(object left, object right, object? arguments)
    {
        if (!IsNumber(left))
        {
            return false;
        }

        if (!IsNumber(right))
        {
            return false;
        }
        var leftNumber = ConvertToNumber(left);
        var rightNumber = ConvertToNumber(right);

        return leftNumber >= rightNumber;
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
}
