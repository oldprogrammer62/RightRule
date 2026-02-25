using RightRule.Rules;

namespace RightRuleTests;

public class RulesToApplyTests
{
    private readonly Dictionary<string, object> _Data;

    public RulesToApplyTests()
    {
        // Data
        _Data = new Dictionary<string, object>(StringComparer.Ordinal)
        {
            { "name", "John" },
            { "surname", "Doe" },
            { "fullname", "John D. Doe" },
            { "age", 28 },
            { "numberoforders", 10 },
            { "goodpayer", true },
            { "joined", new DateTime(2020, 1, 1) } // yyyy mm dd
        };
    }

    [Fact]
    public void TestItWorks_First()
    {
        // Rule
        RuleDefinition rule1 = new()
        {
            Number = 1,
            Name = "Age Rule",
            Statement = @"age equals 28",
            Context = "AllContext",
            Decision = "OLDENOUGH",
        };

        // Rule
        RuleDefinition rule2 = new()
        {
            Number = 2,
            Name = "Name Rule",
            Statement = @"surname equals ""Doe""",
            Context = "AllContext",
            Decision = "NAMEFOUND",
        };

        RightRule.RightRule rr = new RightRule.RightRule();
        var newRule1 = rr.RegisterRule(rule1);
        var newRule2 = rr.RegisterRule(rule2);

        var appliedRule = rr.ApplyRules("AllContext", _Data, RightRule.System.Common.RulesToApply.First);

        Assert.Equal(appliedRule.Decision, rule1.Decision);
    }

    [Fact]
    public void TestItWorks_All()
    {
        // Rule
        RuleDefinition rule1 = new()
        {
            Number = 1,
            Name = "Age Rule",
            Statement = @"age equals 28",
            Context = "AllContext",
            Decision = "OLDENOUGH",
        };

        // Rule
        RuleDefinition rule2 = new()
        {
            Number = 2,
            Name = "Name Rule",
            Statement = @"surname equals ""Doe""",
            Context = "AllContext",
            Decision = "NAMEFOUND",
        };

        RightRule.RightRule rr = new RightRule.RightRule();
        var newRule1 = rr.RegisterRule(rule1);
        var newRule2 = rr.RegisterRule(rule2);

        var appliedRule = rr.ApplyRules("AllContext", _Data, RightRule.System.Common.RulesToApply.All);

        Assert.Equal(appliedRule.Decision, rule2.Decision);
    }

    [Fact]
    public void TestItWorks_All_None_First()
    {
        // Rule
        RuleDefinition rule1 = new()
        {
            Number = 1,
            Name = "Age Rule",
            Statement = @"age equals 30",
            Context = "AllContext",
            Decision = "OLDENOUGH",
        };

        // Rule
        RuleDefinition rule2 = new()
        {
            Number = 2,
            Name = "Name Rule",
            Statement = @"surname equals ""Doexx""",
            Context = "AllContext",
            Decision = "NAMEFOUND",
        };

        RightRule.RightRule rr = new RightRule.RightRule();
        var newRule1 = rr.RegisterRule(rule1);
        var newRule2 = rr.RegisterRule(rule2);

        var appliedRule = rr.ApplyRules("AllContext", _Data, RightRule.System.Common.RulesToApply.All);

        Assert.Equal(appliedRule.Decision, AppliedRule.DefaultRule().Decision);
    }

    [Fact]
    public void TestItWorks_All_None_Last()
    {
        // Rule
        RuleDefinition rule1 = new()
        {
            Number = 1,
            Name = "Age Rule",
            Statement = @"age equals 28",
            Context = "AllContext",
            Decision = "OLDENOUGH",
        };

        // Rule
        RuleDefinition rule2 = new()
        {
            Number = 2,
            Name = "Name Rule",
            Statement = @"surname equals ""Doexx""",
            Context = "AllContext",
            Decision = "NAMEFOUND",
        };

        RightRule.RightRule rr = new RightRule.RightRule();
        var newRule1 = rr.RegisterRule(rule1);
        var newRule2 = rr.RegisterRule(rule2);

        var appliedRule = rr.ApplyRules("AllContext", _Data, RightRule.System.Common.RulesToApply.All);

        Assert.Equal(appliedRule.Decision, AppliedRule.DefaultRule().Decision);
    }
}
