
namespace Operator.dayofweek;

public class DayOfWeek : RightRule.System.Common.Operator
{
    public override List<string> Identifiers => ["dow", "dayofweek"];

    protected override Task<bool> DoOperatorProcessAsync(object left, object right)
    {
        return Task.FromResult(true);
    }
}
