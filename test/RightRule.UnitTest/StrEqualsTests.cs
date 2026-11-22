using RightRule;
using RightRule.RuleServices.RuleModels;

namespace RightRuleTests;

public class StrEqualsTests
{
    [Fact]
    public void StrEquals_ShouldReturnTrue_WhenStringsAreEqual_DiffCaseAllowed()
    {
        Dictionary<string, object> data = new Dictionary<string, object>
        {
            { "value1", "Hello World" }
        };

        DefinedRule rule1 = new DefinedRule
        {
            Context = "TestContext",
            Name = "TestRule",
            Number = 1,
            TrueDecision = "GOOD",
            Statement = "Data.value1 strequals \"HELLO World\"",
            OptionalData = StringComparison.OrdinalIgnoreCase
        };

        var rr = new RightRule.RightRule().CreateSession();
        var dataset = rr.DataService.RegisterData("Data", data);
        var rule = rr.RuleService.RegisterRule(rule1);

        var result = rr.RuleService.ApplyRule("TestContext");

        Assert.NotNull(result);
        Assert.Equal(rule1.TrueDecision, result.Decision);
    }

    [Fact]
    public void StrEquals_ShouldReturnTrue_WhenStringsAreEqual_DiffCaseDisallowed()
    {
        Dictionary<string, object> data = new Dictionary<string, object>
        {
            { "value1", "Hello World" }
        };

        DefinedRule rule1 = new DefinedRule
        {
            Context = "TestContext",
            Name = "TestRule",
            Number = 1,
            TrueDecision = "GOOD",
            Statement = "Data.value1 strequals \"HELLO World\"",
            OptionalData = StringComparison.Ordinal
        };

        var rr = new RightRule.RightRule().CreateSession();
        var dataset = rr.DataService.RegisterData("Data", data);
        var rule = rr.RuleService.RegisterRule(rule1);

        var result = rr.RuleService.ApplyRule("TestContext");

        Assert.NotNull(result);
        Assert.Equal(DefaultRule.DECISIONWORD, result.Decision);
    }

    [Fact]
    public void StrEquals_ShouldReturnTrue_WhenStringsAreEqual_DiffCommand()
    {
        Dictionary<string, object> data = new Dictionary<string, object>
        {
            { "value1", "Hello World" }
        };

        DefinedRule rule1 = new DefinedRule
        {
            Context = "TestContext",
            Name = "TestRule",
            Number = 1,
            TrueDecision = "GOOD",
            Statement = "Data.value1 =str \"HELLO World\"",
            OptionalData = StringComparison.OrdinalIgnoreCase
        };

        var rr = new RightRule.RightRule().CreateSession();
        var dataset = rr.DataService.RegisterData("Data", data);
        var rule = rr.RuleService.RegisterRule(rule1);

        var result = rr.RuleService.ApplyRule("TestContext");

        Assert.NotNull(result);
        Assert.Equal(rule1.TrueDecision, result.Decision);
    }
}
