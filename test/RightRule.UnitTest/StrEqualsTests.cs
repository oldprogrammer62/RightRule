using RightRule.RuleModels;

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

        RuleDefinition rule1 = new RuleDefinition
        {
            Context = "TestContext",
            Name = "TestRule",
            Number = 1,
            Decision = "GOOD",
            Statement = "Data.value1 strequals \"HELLO World\"",
            OperatorConfig = StringComparison.OrdinalIgnoreCase
        };

        BeSoft.IRightRule rr = new BeSoft.RightRule();
        var dataset = rr.RegisterDataset("Data", data);
        var rule = rr.RegisterRule(rule1);

        var result = rr.EvaluateFirstRule("TestContext");

        Assert.NotNull(result);
        Assert.Equal(rule1.Decision, result.Decision);
    }

    [Fact]
    public void StrEquals_ShouldReturnTrue_WhenStringsAreEqual_DiffCaseDisallowed()
    {
        Dictionary<string, object> data = new Dictionary<string, object>
        {
            { "value1", "Hello World" }
        };

        RuleDefinition rule1 = new RuleDefinition
        {
            Context = "TestContext",
            Name = "TestRule",
            Number = 1,
            Decision = "GOOD",
            Statement = "Data.value1 strequals \"HELLO World\"",
            OperatorConfig = StringComparison.Ordinal
        };

        BeSoft.IRightRule rr = new BeSoft.RightRule();
        var dataset = rr.RegisterDataset("Data", data);
        var rule = rr.RegisterRule(rule1);

        var result = rr.EvaluateFirstRule("TestContext");

        Assert.NotNull(result);
        Assert.Equal(AppliedRule.DefaultDecision, result.Decision);
    }

    [Fact]
    public void StrEquals_ShouldReturnTrue_WhenStringsAreEqual_DiffCommand()
    {
        Dictionary<string, object> data = new Dictionary<string, object>
        {
            { "value1", "Hello World" }
        };

        RuleDefinition rule1 = new RuleDefinition
        {
            Context = "TestContext",
            Name = "TestRule",
            Number = 1,
            Decision = "GOOD",
            Statement = "Data.value1 =str \"HELLO World\"",
            OperatorConfig = StringComparison.OrdinalIgnoreCase
        };

        BeSoft.IRightRule rr = new BeSoft.RightRule();
        var dataset = rr.RegisterDataset("Data", data);
        var rule = rr.RegisterRule(rule1);

        var result = rr.EvaluateFirstRule("TestContext");

        Assert.NotNull(result);
        Assert.Equal(rule1.Decision, result.Decision);
    }
}
