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
            Name = "Number of orders Rule",
            Statement = @"numberoforders equals 10",
            Context = "AllContext",
            Decision = "GOOD",
        };

        // Rule
        RuleDefinition rule2 = new()
        {
            Number = 2,
            Name = "Name Rule",
            Statement = @"surname equals ""Doe""",
            Context = "AllContext",
            Decision = "GOOD",
        };

        RightRule.RightRule rr = new RightRule.RightRule();
        var newRule1 = rr.RegisterRule(rule1);
        var newRule2 = rr.RegisterRule(rule2);
        rr.RegisterData("Test", _Data);

        var appliedRule = rr.ApplyRules("AllContext", RightRule.System.Common.RulesToApply.First);

        Assert.Equal(appliedRule.Decision, rule1.Decision);
    }

    [Fact]
    public void TestItWorks_FirstWithMultiRuleRegistration()
    {
        // Rule
        RuleDefinition rule1 = new()
        {
            Number = 1,
            Name = "Number of orders Rule",
            Statement = @"numberoforders equals 10",
            Context = "AllContext",
            Decision = "GOOD",
        };

        // Rule
        RuleDefinition rule2 = new()
        {
            Number = 2,
            Name = "Name Rule",
            Statement = @"surname equals ""Doe""",
            Context = "AllContext",
            Decision = "GOOD",
        };

        var rules = new List<RuleDefinition>() { rule1, rule2 };

        RightRule.RightRule rr = new RightRule.RightRule();
        var newRules = rr.RegisterRules(rules);
        rr.RegisterData("Test", _Data);

        var appliedRule = rr.ApplyRules("AllContext", RightRule.System.Common.RulesToApply.First);

        Assert.Equal(appliedRule.Decision, rule1.Decision);
    }

    [Fact]
    public void TestItWorks_All()
    {
        // Rule
        RuleDefinition rule1 = new()
        {
            Number = 1,
            Name = "Number of orders Rule",
            Statement = @"numberoforders equals 10",
            Context = "AllContext",
            Decision = "GOOD",
        };

        // Rule
        RuleDefinition rule2 = new()
        {
            Number = 2,
            Name = "Name Rule",
            Statement = @"surname equals ""Doe""",
            Context = "AllContext",
            Decision = "GOOD",
        };

        RightRule.RightRule rr = new RightRule.RightRule();
        var newRule1 = rr.RegisterRule(rule1);
        var newRule2 = rr.RegisterRule(rule2);
        rr.RegisterData("Test", _Data); 

        var appliedRule = rr.ApplyRules("AllContext", RightRule.System.Common.RulesToApply.All);

        Assert.Equal(appliedRule.Decision, rule2.Decision);
    }

    [Fact]
    public void TestItWorks_All_None_First()
    {
        // Rule
        RuleDefinition rule1 = new()
        {
            Number = 1,
            Name = "Number of orders Rule",
            Statement = @"numberoforders equals 10",
            Context = "AllContext",
            Decision = "GOOD",
        };

        // Rule
        RuleDefinition rule2 = new()
        {
            Number = 2,
            Name = "Name Rule",
            Statement = @"surname equals ""Doexx""",
            Context = "AllContext",
            Decision = "GOOD",
        };

        RightRule.RightRule rr = new RightRule.RightRule();
        var newRule1 = rr.RegisterRule(rule1);
        var newRule2 = rr.RegisterRule(rule2);
        rr.RegisterData("Test", _Data);

        var appliedRule = rr.ApplyRules("AllContext", RightRule.System.Common.RulesToApply.All);

        Assert.Equal(appliedRule.Decision, AppliedRule.DefaultRule().Decision);
    }

    [Fact]
    public void TestItWorks_All_None_Last()
    {
        // Rule
        RuleDefinition rule1 = new()
        {
            Number = 1,
            Name = "Number of orders Rule",
            Statement = @"numberoforders equals 10",
            Context = "AllContext",
            Decision = "GOOD",
        };

        // Rule
        RuleDefinition rule2 = new()
        {
            Number = 2,
            Name = "Name Rule",
            Statement = @"surname equals ""Doexx""",
            Context = "AllContext",
            Decision = "GOOD",
        };

        RightRule.RightRule rr = new RightRule.RightRule();
        var newRule1 = rr.RegisterRule(rule1);
        var newRule2 = rr.RegisterRule(rule2);
        rr.RegisterData("Test", _Data);

        var appliedRule = rr.ApplyRules("AllContext", RightRule.System.Common.RulesToApply.All);

        Assert.Equal(appliedRule.Decision, AppliedRule.DefaultRule().Decision);
    }
}
