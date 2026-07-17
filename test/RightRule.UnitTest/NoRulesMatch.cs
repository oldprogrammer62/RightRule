using RightRule.Rules;

namespace RightRuleTests;

public class NoRulesMatch
{
    private readonly Dictionary<string, object> _Data;

    public NoRulesMatch()
    {
        // Data
        _Data = new Dictionary<string, object>(StringComparer.Ordinal)
        {
            { "intvalue", 100 }
        };
    }

    [Fact]
    public void SingleRule1_Matched()
    {
        // Rule
        RuleDefinition rule1 = new()
        {
            Number = 1,
            Name = "Equals Bool Value Rule",
            Statement = @"Test.intvalue equals 100",
            Context = "Context",
            Decision = "EQUALS100",
        };
        RuleDefinition rule2 = new()
        {
            Number = 2,
            Name = "Equals Bool Value Rule",
            Statement = @"Test.intvalue equals 101",
            Context = "Context",
            Decision = "EQUALS101",
        };

        RightRule.IRightRule rr = new RightRule.RightRule();
        var newRule = rr.RegisterRule(rule1);
        rr.RegisterData("Test", _Data);

        var appliedRule = rr.ApplyRule("Context");

        Assert.Equal(rule1.Decision, appliedRule.Decision);
    }

    [Fact]
    public void SingleRule2_Matched()
    {
        // Rule
        RuleDefinition rule1 = new()
        {
            Number = 1,
            Name = "Equals Bool Value Rule",
            Statement = @"Test.intvalue equals 101",
            Context = "Context",
            Decision = "EQUALS101",
        };
        RuleDefinition rule2 = new()
        {
            Number = 2,
            Name = "Equals Bool Value Rule",
            Statement = @"Test.intvalue equals 100",
            Context = "Context",
            Decision = "EQUALS100",
        };

        RightRule.IRightRule rr = new RightRule.RightRule();
        var newRule1 = rr.RegisterRule(rule1);
        var newRule2 = rr.RegisterRule(rule2);
        rr.RegisterData("Test", _Data);

        var appliedRule = rr.ApplyRule("Context");

        Assert.Equal(rule2.Decision, appliedRule.Decision);
    }

    [Fact]
    public void SingleRule1_NotMatched()
    {
        // Rule
        RuleDefinition rule1 = new()
        {
            Number = 1,
            Name = "Equals Bool Value Rule",
            Statement = @"Test.intvalue equals 99",
            Context = "Context",
            Decision = "EQUALS",
        };

        RightRule.IRightRule rr = new RightRule.RightRule();
        var newRule = rr.RegisterRule(rule1);
        rr.RegisterData("Test", _Data);

        var appliedRule = rr.ApplyRule("Context");

        Assert.Equal(AppliedRule.DefaultRule().Decision , appliedRule.Decision);
    }

    [Fact]
    public void SingleRule2_NotMatched()
    {
        // Rule
        RuleDefinition rule1 = new()
        {
            Number = 1,
            Name = "Equals Bool Value Rule",
            Statement = @"Test.intvalue equals 99",
            Context = "Context",
            Decision = "EQUALS",
        };
        RuleDefinition rule2 = new()
        {
            Number = 2,
            Name = "Equals Bool Value Rule",
            Statement = @"Test.intvalue equals 101",
            Context = "Context",
            Decision = "EQUALS",
        };

        RightRule.IRightRule rr = new RightRule.RightRule();
        var newRule1 = rr.RegisterRule(rule1);
        var newRule2 = rr.RegisterRule(rule2);
        rr.RegisterData("Test", _Data);

        var appliedRule = rr.ApplyRule("Context");

        Assert.Equal(AppliedRule.DefaultRule().Decision, appliedRule.Decision);
    }

    [Fact]
    public void AllRules_First_OneMatched()
    {
        // Rule
        RuleDefinition rule1 = new()
        {
            Number = 1,
            Name = "Equals Bool Value Rule",
            Statement = @"Test.intvalue equals 100",
            Context = "Context",
            Decision = "EQUALS100",
        };
        RuleDefinition rule2 = new()
        {
            Number = 2,
            Name = "Equals Bool Value Rule",
            Statement = @"Test.intvalue equals 101",
            Context = "Context",
            Decision = "EQUALS101",
        };

        RightRule.IRightRule rr = new RightRule.RightRule();
        var newRule1 = rr.RegisterRule(rule1);
        var newRule2 = rr.RegisterRule(rule2);
        rr.RegisterData("Test", _Data);

        var appliedRules = rr.ApplyRules("Context", RightRule.System.Common.RulesToReturn.First);

        Assert.Equal(rule1.Decision, appliedRules[0].Decision);
    }

    [Fact]
    public void AllRules_First_AllMatched()
    {
        // Rule
        RuleDefinition rule1 = new()
        {
            Number = 1,
            Name = "Equals Bool Value Rule",
            Statement = @"Test.intvalue equals 101",
            Context = "Context",
            Decision = "EQUALS101",
        };
        RuleDefinition rule2 = new()
        {
            Number = 2,
            Name = "Equals Bool Value Rule",
            Statement = @"Test.intvalue equals 100",
            Context = "Context",
            Decision = "EQUALS100",
        };

        RightRule.IRightRule rr = new RightRule.RightRule();
        var newRule1 = rr.RegisterRule(rule1);
        var newRule2 = rr.RegisterRule(rule2);
        rr.RegisterData("Test", _Data);

        var appliedRules = rr.ApplyRules("Context", RightRule.System.Common.RulesToReturn.First);

        Assert.Equal(rule2.Decision, appliedRules[0].Decision);
    }

    [Fact]
    public void AllRules_First_NoneMatched()
    {
        // Rule
        RuleDefinition rule1 = new()
        {
            Number = 1,
            Name = "Equals Bool Value Rule",
            Statement = @"Test.intvalue equals 101",
            Context = "Context",
            Decision = "EQUALS101",
        };
        RuleDefinition rule2 = new()
        {
            Number = 2,
            Name = "Equals Bool Value Rule",
            Statement = @"Test.intvalue equals 102",
            Context = "Context",
            Decision = "EQUALS102",
        };

        RightRule.IRightRule rr = new RightRule.RightRule();
        var newRule1 = rr.RegisterRule(rule1);
        var newRule2 = rr.RegisterRule(rule2);
        rr.RegisterData("Test", _Data);

        var appliedRules = rr.ApplyRules("Context", RightRule.System.Common.RulesToReturn.First);

        Assert.Equal(AppliedRule.DefaultRule().Decision, appliedRules[0].Decision);
    }

    [Fact]
    public void AllRules_All_OneMatched()
    {
        // Rule
        RuleDefinition rule1 = new()
        {
            Number = 1,
            Name = "Equals Bool Value Rule",
            Statement = @"Test.intvalue equals 100",
            Context = "Context",
            Decision = "EQUALS100",
        };
        RuleDefinition rule2 = new()
        {
            Number = 2,
            Name = "Equals Bool Value Rule",
            Statement = @"Test.intvalue equals 101",
            Context = "Context",
            Decision = "EQUALS101",
        };

        RightRule.IRightRule rr = new RightRule.RightRule();
        var newRule1 = rr.RegisterRule(rule1);
        var newRule2 = rr.RegisterRule(rule2);
        rr.RegisterData("Test", _Data);

        var appliedRules = rr.ApplyRules("Context", RightRule.System.Common.RulesToReturn.All);

        Assert.Equal(rule1.Decision, appliedRules[0].Decision);
    }

    [Fact]
    public void AllRules_All_AllMatched()
    {
        // Rule
        RuleDefinition rule1 = new()
        {
            Number = 1,
            Name = "Equals Bool Value Rule",
            Statement = @"Test.intvalue equals 100",
            Context = "Context",
            Decision = "EQUALS100_1",
        };
        RuleDefinition rule2 = new()
        {
            Number = 2,
            Name = "Equals Bool Value Rule",
            Statement = @"Test.intvalue equals 100",
            Context = "Context",
            Decision = "EQUALS100_2",
        };

        RightRule.IRightRule rr = new RightRule.RightRule();
        var newRule1 = rr.RegisterRule(rule1);
        var newRule2 = rr.RegisterRule(rule2);
        rr.RegisterData("Test", _Data);

        var appliedRules = rr.ApplyRules("Context", RightRule.System.Common.RulesToReturn.All);

        Assert.Equal(rule1.Decision, appliedRules[0].Decision);
        Assert.Equal(rule2.Decision, appliedRules[1].Decision);
    }

    [Fact]
    public void AllRules_All_NoneMatched()
    {
        // Rule
        RuleDefinition rule1 = new()
        {
            Number = 1,
            Name = "Equals Bool Value Rule",
            Statement = @"Test.intvalue equals 101",
            Context = "Context",
            Decision = "EQUALS101",
        };
        RuleDefinition rule2 = new()
        {
            Number = 2,
            Name = "Equals Bool Value Rule",
            Statement = @"Test.intvalue equals 102",
            Context = "Context",
            Decision = "EQUALS102",
        };

        RightRule.IRightRule rr = new RightRule.RightRule();
        var newRule1 = rr.RegisterRule(rule1);
        var newRule2 = rr.RegisterRule(rule2);
        rr.RegisterData("Test", _Data);

        var appliedRules = rr.ApplyRules("Context", RightRule.System.Common.RulesToReturn.All);

        Assert.Equal(AppliedRule.DefaultRule().Decision, appliedRules[0].Decision);
        Assert.Equal(AppliedRule.DefaultRule().Decision, appliedRules[1].Decision);
    }
}
