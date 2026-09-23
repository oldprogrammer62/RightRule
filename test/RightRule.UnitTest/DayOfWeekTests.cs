using RightRule.RuleServices.RuleModels;

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

        DefinedRule rule1 = new DefinedRule()
        {
            Context = "context",
            Number = 1,
            Name = "rule1",
            Statement = "data.date dow \"wednesday\"",
            Decision = "SAME",
            FalseDecision = "NOT_SAME"
        };

        BeSoft.IRightRule rr = new BeSoft.RightRule();
        rr.RegisterData("data", data);
        var parsedRule = rr.RegisterRule(rule1);
        var appliedRule = rr.ApplyRule("context");

        Assert.NotNull(appliedRule);
    }
}
