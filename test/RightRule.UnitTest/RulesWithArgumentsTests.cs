using RightRule.Rules;

namespace RightRuleTests;

public class RulesWithArgumentsTests
{
    private readonly Dictionary<string, object> _Data;

    public RulesWithArgumentsTests()
    {
        // Data
        _Data = new Dictionary<string, object>(StringComparer.Ordinal)
        {
            { "value", 10 },
            { "productcode", "PROD1" },
            { "newcust", true }
        };
    }

    [Fact]
    public void GreaterThanWithArgumentsOneCondition()
    {
        Dictionary<string, object> _Arguments = new(StringComparer.Ordinal)
        {
            { "hv", 5 }
        };

        // Rule
        RuleDefinition rule = new()
        {
            Number = 1,
            Name = "Greater Than Value Rule",
            Statement = @"value greaterthanorequals {hv}",
            Context = "Context",
            Decision = "GREATERTHANOREQUALS",
            Arguments = _Arguments
        };

        RightRule.RightRule rr = new RightRule.RightRule();
        var newRule = rr.RegisterRule(rule);
        rr.RegisterData("Test", _Data);

        var appliedRule = rr.ApplyRules("Context", RightRule.System.Common.RulesToApply.First);

        Assert.Equal(rule.Decision, appliedRule.Decision);
    }

    [Fact]
    public void GreaterThanWithArgumentsTwoCondition()
    {
        Dictionary<string, object> _Arguments = new(StringComparer.Ordinal)
        {
            { "hv", 5 }
        };

        // Rule
        RuleDefinition rule = new()
        {
            Number = 1,
            Name = "Greater Than Value Rule",
            Statement = @"value greaterthanorequals {hv} and productcode = ""PROD1""",
            Context = "Context",
            Decision = "GREATERTHANOREQUALS",
            Arguments = _Arguments
        };

        RightRule.RightRule rr = new RightRule.RightRule();
        var newRule = rr.RegisterRule(rule);
        rr.RegisterData("Test", _Data);

        var appliedRule = rr.ApplyRules("Context", RightRule.System.Common.RulesToApply.First);

        Assert.Equal(rule.Decision, appliedRule.Decision);
    }

    [Fact]
    public void GreaterThanWithArgumentsTwoVariableCondition()
    {
        Dictionary<string, object> _Arguments = new(StringComparer.Ordinal)
        {
            { "hv", 5 },
            { "pc", "PROD1" }
        };

        // Rule
        RuleDefinition rule = new()
        {
            Number = 1,
            Name = "Greater Than Value Rule",
            Statement = @"value greaterthanorequals {hv} and productcode = {pc}",
            Context = "Context",
            Decision = "GREATERTHANOREQUALS",
            Arguments = _Arguments
        };

        RightRule.RightRule rr = new RightRule.RightRule();
        var newRule = rr.RegisterRule(rule);
        rr.RegisterData("Test", _Data);

        var appliedRule = rr.ApplyRules("Context", RightRule.System.Common.RulesToApply.First);

        Assert.Equal(rule.Decision, appliedRule.Decision);
    }
}
