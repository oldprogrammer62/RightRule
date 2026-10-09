using RightRule.System.Common.Operator.Models;

namespace dayofweek;

public class DayOfWeek : Operator
{
    public override List<string> Aliases => ["dow", "dayofweek"];

    private readonly List<string> ShortDays = ["sun", "mon", "tue", "wed", "thu", "fri", "sat"];
    private readonly List<string> LongDays = ["sunday", "monday", "tuesday", "wednesday", "thursday", "friday", "saturday"];

    public DayOfWeek() : base()
    {
    }

    protected override async Task<OperatorResult> ExecuteAsync(object left, object right, object? OptionalData)
    {
        if (left is null || right is null)
        {
            return OperatorResult.FailureResult(this.Name, "One or both operands are null.");
        }

        // left will contain a reference to a Date
        // right will contain a day.
        // This can be
        // Long day: Monday, Tuesday etc
        // Short day: Mon, Tue etc

        var day = GetDayNumberOfWeek(left);
        if (day == -1) return OperatorResult.FailureResult(this.Name, "Invalid day.");

        var shortDay = ShortDays[day];
        var longDay = LongDays[day];

        if (right is string)
        {
            if (right.ToString()!.ToLower().Equals(shortDay.ToString().ToLower()))
            {
                return OperatorResult.SuccessResult(this.Name, true);
            }
            if (right.ToString()!.ToLower().Equals(longDay.ToString().ToLower()))
            {
                return OperatorResult.SuccessResult(this.Name, true);
            }
        }
        return OperatorResult.FailureResult(this.Name, "Operand (right) is not a valid date.");
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
            var day = (int)dDate.DayOfWeek;
            return day;
        }
        return -1;
    }
}
