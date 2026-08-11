using RightRule.RuleModels;

namespace RightRuleTests;

public class DayOfWeekTests
{
    [Fact]
    public void TestDayOfWeek()
    {
        // WEDNESDAY
        Dictionary<string, object> data = new Dictionary<string, object>()
        {
            { "date", new DateOnly(2026, 07, 29) }
        };

        RuleDefinition rule1 = new RuleDefinition()
        {
            Context = "context",
            Number = 1,
            Name = "rule1",
            Decision = "SAME",
            Statement = "data.date dow \"wednesday\""
        };

        BeSoft.IRightRule rr = new BeSoft.RightRule();
        rr.RegisterData("data", data);
        var parsedRule = rr.RegisterRule(rule1);
        var appliedRule = rr.EvaluateFirstRule("context");

        Assert.NotNull(appliedRule);
    }
}
