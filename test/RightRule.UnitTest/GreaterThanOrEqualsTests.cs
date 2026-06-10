using RightRule.Rules;

namespace RightRuleTests;

public class GreaterThanOrEqualsTests
{
    private readonly Dictionary<string, object> _Data;

    public GreaterThanOrEqualsTests()
    {
        // Data
        _Data = new Dictionary<string, object>(StringComparer.Ordinal)
        {
            { "value", 100 }
        };
    }

    [Fact]
    public void GreaterThan()
    {
        // Rule
        RuleDefinition rule = new()
        {
            Number = 1,
            Name = "Greater Than Value Rule",
            Statement = @"value greaterthanorequals 10",
            Context = "Context",
            Decision = "GREATERTHANOREQUALS",
        };

        RightRule.RightRule rr = new RightRule.RightRule();
        var newRule = rr.RegisterRule(rule);
        rr.RegisterData("Test", _Data);

        var appliedRule = rr.ApplyRules("Context", RightRule.System.Common.RulesToApply.First);

        Assert.Equal(rule.Decision, appliedRule.Decision);
    }

    [Fact]
    public void NotGreaterThan()
    {
        // Rule
        RuleDefinition rule = new()
        {
            Number = 1,
            Name = "Not Greater Than Value Rule",
            Statement = @"value greaterthanorequals 101",
            Context = "Context",
            Decision = "NOT_GREATERTHAN",
        };

        RightRule.RightRule rr = new RightRule.RightRule();
        var newRule = rr.RegisterRule(rule);
        rr.RegisterData("Test", _Data);

        var appliedRule = rr.ApplyRules("Context", RightRule.System.Common.RulesToApply.First);

        Assert.Equal(AppliedRule.DefaultRule().Decision, appliedRule.Decision);
    }
}
