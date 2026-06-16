using RightRule.Rules;

namespace RightRuleTests;

public class LessThanTests
{
    private readonly Dictionary<string, object> _Data;

    public LessThanTests()
    {
        // Data
        _Data = new Dictionary<string, object>(StringComparer.Ordinal)
        {
            { "value", 10 }
        };
    }

    [Fact]
    public void LessThan()
    {
        // Rule
        RuleDefinition rule = new()
        {
            Number = 1,
            Name = "Less ThanValue Rule",
            Statement = @"value lessthan 100",
            Context = "Context",
            Decision = "LESSTHAN",
        };

        RightRule.RightRule rr = new RightRule.RightRule();
        var newRule = rr.RegisterRule(rule);
        rr.RegisterData("Test", _Data);

        var appliedRule = rr.ApplyRule("Context");

        Assert.Equal(rule.Decision, appliedRule.Decision);
    }

    [Fact]
    public void NotLessThan()
    {
        // Rule
        RuleDefinition rule = new()
        {
            Number = 1,
            Name = "Less ThanValue Rule",
            Statement = @"value lessthan 5",
            Context = "Context",
            Decision = "NOT_LESSTHAN",
        };

        RightRule.RightRule rr = new RightRule.RightRule();
        var newRule = rr.RegisterRule(rule);
        rr.RegisterData("Test", _Data);

        var appliedRule = rr.ApplyRule("Context");

        Assert.Equal(AppliedRule.DefaultRule().Decision, appliedRule.Decision);
    }
}
