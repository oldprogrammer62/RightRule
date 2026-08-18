
using RightRule.System.Common;

namespace Operator.dayofweek;

public class DayOfWeek : RightRule.System.Common.Operator
{
    public override List<string> Identifiers => ["dow", "dayofweek"];

    private readonly List<string> ShortDays = ["sun", "mon", "tue", "wed", "thu", "fri", "sat"];
    private readonly List<string> LongDays = ["sunday", "monday", "tuesday", "wednesday", "thursday", "friday", "saturday"];

    public DayOfWeek() : base()
    {
    }

    protected override async Task<EvaluationResult> DoOperatorProcessAsync(object left, object right, object? OptionalData, bool Diagnostics)
    {
        if (left is null || right is null)
        {
            return EvaluationResult.Failure("One or both operands are null.");
        }

        // left will contain a reference to a Date
        // right will contain a day.
        // This can be
        // Long day: Monday, Tuesday etc
        // Short day: Mon, Tue etc

        var day = GetDayNumberOfWeek(left);
        if (day == -1) return EvaluationResult.Failure("Invalid day.");

        var shortDay = ShortDays[day];
        var longDay = LongDays[day];

        if (right is string)
        {
            if (right.ToString()!.ToLower().Equals(shortDay.ToString().ToLower()))
            {
                return EvaluationResult.Success(true);
            }
            if (right.ToString()!.ToLower().Equals(longDay.ToString().ToLower()))
            {
                return EvaluationResult.Success(true);
            }
        }
        return EvaluationResult.Failure("Operand (right) is not a valid date.");
    }

    /// <summary>
    /// Get Day Number of day from date
    /// 0 = Sunday
    /// 1 = Monday
    /// </summary>
    /// <param name="date"></param>
    /// <returns></returns>
    private int GetDayNumberOfWeek(object date)
    {
        if ( date is DateOnly)
        {
            DateOnly dDate = DateOnly.Parse(date.ToString()!);
            var day = dDate.Day;
            return day;
        }
        return -1;
    }
}
