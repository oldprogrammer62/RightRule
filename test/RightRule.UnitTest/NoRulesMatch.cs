using RightRule.RuleModels;

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
        DefinedRule rule1 = new()
        {
            Number = 1,
            Name = "Equals Bool Value Rule",
            Statement = @"Test.intvalue equals 100",
            Context = "Context",
            Decision = "EQUALS100",
        };
        DefinedRule rule2 = new()
        {
            Number = 2,
            Name = "Equals Bool Value Rule",
            Statement = @"Test.intvalue equals 101",
            Context = "Context",
            Decision = "EQUALS101",
        };

        BeSoft.IRightRule rr = new BeSoft.RightRule();
        var newRule = rr.RegisterRule(rule1);
        rr.RegisterData("Test", _Data);

        var appliedRule = rr.EvaluateFirstMatch("Context");

        Assert.Equal(rule1.Decision, appliedRule.Decision);
    }

    [Fact]
    public void SingleRule2_Matched()
    {
        // Rule
        DefinedRule rule1 = new()
        {
            Number = 1,
            Name = "Equals Bool Value Rule1",
            Statement = @"Test.intvalue equals 101",
            Context = "Context",
            Decision = "EQUALS101",
        };
        DefinedRule rule2 = new()
        {
            Number = 2,
            Name = "Equals Bool Value Rule2",
            Statement = @"Test.intvalue equals 100",
            Context = "Context",
            Decision = "EQUALS100",
        };

        BeSoft.IRightRule rr = new BeSoft.RightRule();
        var newRule1 = rr.RegisterRule(rule1);
        var newRule2 = rr.RegisterRule(rule2);
        rr.RegisterData("Test", _Data);

        var appliedRule = rr.EvaluateFirstMatch("Context");

        Assert.Equal(rule2.Decision, appliedRule.Decision);
    }

    [Fact]
    public void SingleRule1_NotMatched()
    {
        // Rule
        DefinedRule rule1 = new()
        {
            Number = 1,
            Name = "Equals Bool Value Rule",
            Statement = @"Test.intvalue equals 99",
            Context = "Context",
            Decision = "EQUALS",
        };

        BeSoft.IRightRule rr = new BeSoft.RightRule();
        var newRule = rr.RegisterRule(rule1);
        rr.RegisterData("Test", _Data);

        var appliedRule = rr.EvaluateFirstMatch("Context");

        Assert.Equal(AppliedRule.DefaultDecision , appliedRule.Decision);
    }

    [Fact]
    public void SingleRule2_NotMatched()
    {
        // Rule
        DefinedRule rule1 = new()
        {
            Number = 1,
            Name = "Equals Bool Value Rule",
            Statement = @"Test.intvalue equals 99",
            Context = "Context",
            Decision = "EQUALS",
        };
        DefinedRule rule2 = new()
        {
            Number = 2,
            Name = "Equals Bool Value Rule",
            Statement = @"Test.intvalue equals 101",
            Context = "Context",
            Decision = "EQUALS",
        };

        BeSoft.IRightRule rr = new BeSoft.RightRule();
        var newRule1 = rr.RegisterRule(rule1);
        var newRule2 = rr.RegisterRule(rule2);
        rr.RegisterData("Test", _Data);

        var appliedRule = rr.EvaluateFirstMatch("Context");

        Assert.Equal(AppliedRule.DefaultDecision, appliedRule.Decision);
    }

    [Fact]
    public void AllRules_First_OneMatched()
    {
        // Rule
        DefinedRule rule1 = new()
        {
            Number = 1,
            Name = "Equals Bool Value Rule",
            Statement = @"Test.intvalue equals 100",
            Context = "Context",
            Decision = "EQUALS100",
        };
        DefinedRule rule2 = new()
        {
            Number = 2,
            Name = "Equals Bool Value Rule",
            Statement = @"Test.intvalue equals 101",
            Context = "Context",
            Decision = "EQUALS101",
        };

        BeSoft.IRightRule rr = new BeSoft.RightRule();
        var newRule1 = rr.RegisterRule(rule1);
        var newRule2 = rr.RegisterRule(rule2);
        rr.RegisterData("Test", _Data);

        var appliedRules = rr.EvaluateAll("Context", RightRule.System.Common.RulesToReturn.First);

        Assert.Equal(rule1.Decision, appliedRules[0].Decision);
    }

    [Fact]
    public void AllRules_First_AllMatched()
    {
        // Rule
        DefinedRule rule1 = new()
        {
            Number = 1,
            Name = "Equals Bool Value Rule1",
            Statement = @"Test.intvalue equals 101",
            Context = "Context",
            Decision = "EQUALS101",
        };
        DefinedRule rule2 = new()
        {
            Number = 2,
            Name = "Equals Bool Value Rule2",
            Statement = @"Test.intvalue equals 100",
            Context = "Context",
            Decision = "EQUALS100",
        };

        BeSoft.IRightRule rr = new BeSoft.RightRule();
        var newRule1 = rr.RegisterRule(rule1);
        var newRule2 = rr.RegisterRule(rule2);
        rr.RegisterData("Test", _Data);

        var appliedRules = rr.EvaluateAll("Context", RightRule.System.Common.RulesToReturn.First);

        Assert.Equal(rule2.Decision, appliedRules[0].Decision);
    }

    [Fact]
    public void AllRules_First_NoneMatched()
    {
        // Rule
        DefinedRule rule1 = new()
        {
            Number = 1,
            Name = "Equals Bool Value Rule",
            Statement = @"Test.intvalue equals 101",
            Context = "Context",
            Decision = "EQUALS101",
        };
        DefinedRule rule2 = new()
        {
            Number = 2,
            Name = "Equals Bool Value Rule",
            Statement = @"Test.intvalue equals 102",
            Context = "Context",
            Decision = "EQUALS102",
        };

        BeSoft.IRightRule rr = new BeSoft.RightRule();
        var newRule1 = rr.RegisterRule(rule1);
        var newRule2 = rr.RegisterRule(rule2);
        rr.RegisterData("Test", _Data);

        var appliedRules = rr.EvaluateAll("Context", RightRule.System.Common.RulesToReturn.First);

        Assert.Equal(AppliedRule.DefaultDecision, appliedRules[0].Decision);
    }

    [Fact]
    public void AllRules_All_OneMatched()
    {
        // Rule
        DefinedRule rule1 = new()
        {
            Number = 1,
            Name = "Equals Bool Value Rule",
            Statement = @"Test.intvalue equals 100",
            Context = "Context",
            Decision = "EQUALS100",
        };
        DefinedRule rule2 = new()
        {
            Number = 2,
            Name = "Equals Bool Value Rule",
            Statement = @"Test.intvalue equals 101",
            Context = "Context",
            Decision = "EQUALS101",
        };

        BeSoft.IRightRule rr = new BeSoft.RightRule();
        var newRule1 = rr.RegisterRule(rule1);
        var newRule2 = rr.RegisterRule(rule2);
        rr.RegisterData("Test", _Data);

        var appliedRules = rr.EvaluateAll("Context", RightRule.System.Common.RulesToReturn.All);

        Assert.Equal(rule1.Decision, appliedRules[0].Decision);
    }

    [Fact]
    public void AllRules_All_AllMatched()
    {
        // Rule
        DefinedRule rule1 = new()
        {
            Number = 1,
            Name = "Equals Bool Value Rule1",
            Statement = @"Test.intvalue equals 100",
            Context = "Context",
            Decision = "EQUALS100_1",
        };
        DefinedRule rule2 = new()
        {
            Number = 2,
            Name = "Equals Bool Value Rule2",
            Statement = @"Test.intvalue equals 100",
            Context = "Context",
            Decision = "EQUALS100_2",
        };

        BeSoft.IRightRule rr = new BeSoft.RightRule();
        var newRule1 = rr.RegisterRule(rule1);
        var newRule2 = rr.RegisterRule(rule2);
        rr.RegisterData("Test", _Data);

        var appliedRules = rr.EvaluateAll("Context", RightRule.System.Common.RulesToReturn.All);

        Assert.Equal(rule1.Decision, appliedRules[0].Decision);
        Assert.Equal(rule2.Decision, appliedRules[1].Decision);
    }

    [Fact]
    public void AllRules_All_NoneMatched()
    {
        // Rule
        DefinedRule rule1 = new()
        {
            Number = 1,
            Name = "Equals Bool Value Rule1",
            Statement = @"Test.intvalue equals 101",
            Context = "Context",
            Decision = "EQUALS101",
        };
        DefinedRule rule2 = new()
        {
            Number = 2,
            Name = "Equals Bool Value Rule2",
            Statement = @"Test.intvalue equals 102",
            Context = "Context",
            Decision = "EQUALS102",
        };

        BeSoft.IRightRule rr = new BeSoft.RightRule();
        var newRule1 = rr.RegisterRule(rule1);
        // var newRule2 = rr.RegisterRule(rule2);
        rr.RegisterData("Test", _Data);

        var appliedRules = rr.EvaluateAll("Context", RightRule.System.Common.RulesToReturn.All);

        Assert.Equal(AppliedRule.DefaultDecision, appliedRules[0].Decision);
        Assert.Equal(AppliedRule.DefaultDecision, appliedRules[1].Decision);
    }
}
