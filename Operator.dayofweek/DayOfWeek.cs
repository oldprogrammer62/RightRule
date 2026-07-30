
namespace Operator.dayofweek;

public class DayOfWeek : RightRule.System.Common.Operator
{
    public override List<string> Identifiers => ["dow", "dayofweek"];

    private readonly List<string> ShortDays = ["sun", "mon", "tue", "wed", "thu", "fri", "sat"];
    private readonly List<string> LongDays = ["sunday", "monday", "tuesday", "wednesday", "thursday", "friday", "saturday"];

    public DayOfWeek() : base()
    {
    }

    protected override Task<bool> DoOperatorProcessAsync(object left, object right)
    {
        if (left is null || right is null)
        {
            return Task.FromResult(false);
        }

        // left will contain a reference to a Date
        // right will contain a day.
        // This can be
        // Long day: Monday, Tuesday etc
        // Short day: Mon, Tue etc

        var day = GetDayNumberOfWeek(left);
        if (day == -1) return Task.FromResult(false);

        var shortDay = ShortDays[day];
        var longDay = LongDays[day];

        if (right is string)
        {
            if (right.ToString()!.ToLower().Equals(shortDay.ToString().ToLower()))
            {
                return Task.FromResult(true);
            }
            if (right.ToString()!.ToLower().Equals(longDay.ToString().ToLower()))
            {
                return Task.FromResult(true);
            }
        }
        return Task.FromResult(false);
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
