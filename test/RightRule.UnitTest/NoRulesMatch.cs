using RightRule;
using RightRule.LanguageProcessor.Evaluator;
using RightRule.RuleServices.RuleModels;
using RightRule.System.Common;

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
            TrueDecision = "EQUALS100",
            FalseDecision = "NOT_EQUALS100"
        };
        DefinedRule rule2 = new()
        {
            Number = 2,
            Name = "Equals Bool Value Rule",
            Statement = @"Test.intvalue equals 101",
            Context = "Context",
            TrueDecision = "EQUALS101",
            FalseDecision = "NOT_EQUALS101"
        };

        IRightRule rr = new RightRule.RightRule();
        var newRule = rr.RegisterRule(rule1);
        rr.RegisterData("Test", _Data);

        var appliedRule = rr.ApplyRule("Context");

        Assert.Equal(rule1.TrueDecision, appliedRule.Decision);
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
            TrueDecision = "EQUALS101",
            FalseDecision = "NOT_EQUALS101"
        };
        DefinedRule rule2 = new()
        {
            Number = 2,
            Name = "Equals Bool Value Rule2",
            Statement = @"Test.intvalue equals 100",
            Context = "Context",
            TrueDecision = "EQUALS100",
            FalseDecision = "NOT_EQUALS100"
        };

        IRightRule rr = new RightRule.RightRule();
        var newRule1 = rr.RegisterRule(rule1);
        var newRule2 = rr.RegisterRule(rule2);
        rr.RegisterData("Test", _Data);

        var appliedRule = rr.ApplyRule("Context");

        Assert.Equal(rule2.TrueDecision, appliedRule.Decision);
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
            TrueDecision = "EQUALS",
            FalseDecision = "NOT_EQUALS"
        };

        IRightRule rr = new RightRule.RightRule();
        var newRule = rr.RegisterRule(rule1);
        rr.RegisterData("Test", _Data);

        var appliedRule = rr.ApplyRule("Context");

        Assert.Equal(DefaultRule.DECISIONWORD , appliedRule.Decision);
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
            TrueDecision = "EQUALS",
            FalseDecision = "NOT_EQUALS"
        };
        DefinedRule rule2 = new()
        {
            Number = 2,
            Name = "Equals Bool Value Rule",
            Statement = @"Test.intvalue equals 101",
            Context = "Context",
            TrueDecision = "EQUALS",
            FalseDecision = "NOT_EQUALS"
        };

        IRightRule rr = new RightRule.RightRule();
        var newRule1 = rr.RegisterRule(rule1);
        var newRule2 = rr.RegisterRule(rule2);
        rr.RegisterData("Test", _Data);

        var appliedRule = rr.ApplyRule("Context");

        Assert.Equal(DefaultRule.DECISIONWORD, appliedRule.Decision);
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
            TrueDecision = "EQUALS100",
            FalseDecision = "NOT_EQUALS100"
        };
        DefinedRule rule2 = new()
        {
            Number = 2,
            Name = "Equals Bool Value Rule",
            Statement = @"Test.intvalue equals 101",
            Context = "Context",
            TrueDecision = "EQUALS101",
            FalseDecision = "NOT_EQUALS101"
        };

        IRightRule rr = new RightRule.RightRule();
        var newRule1 = rr.RegisterRule(rule1);
        var newRule2 = rr.RegisterRule(rule2);
        rr.RegisterData("Test", _Data);

        var appliedRules = rr.ApplyRules("Context", EvaluationTypeEnum.FIRST);

        Assert.Equal(rule1.TrueDecision, appliedRules[0].Decision);
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
            TrueDecision = "EQUALS101",
            FalseDecision = "NOT_EQUALS101"
        };
        DefinedRule rule2 = new()
        {
            Number = 2,
            Name = "Equals Bool Value Rule2",
            Statement = @"Test.intvalue equals 100",
            Context = "Context",
            TrueDecision = "EQUALS100",
            FalseDecision = "NOT_EQUALS100"
        };

        IRightRule rr = new RightRule.RightRule();
        var newRule1 = rr.RegisterRule(rule1);
        var newRule2 = rr.RegisterRule(rule2);
        rr.RegisterData("Test", _Data);

        var appliedRules = rr.ApplyRules("Context", EvaluationTypeEnum.FIRST);

        Assert.Equal(rule2.TrueDecision, appliedRules[0].Decision);
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
            TrueDecision = "EQUALS101",
            FalseDecision = "NOT_EQUALS101"
        };
        DefinedRule rule2 = new()
        {
            Number = 2,
            Name = "Equals Bool Value Rule",
            Statement = @"Test.intvalue equals 102",
            Context = "Context",
            TrueDecision = "EQUALS102",
            FalseDecision = "NOT_EQUALS102"
        };

        IRightRule rr = new RightRule.RightRule();
        var newRule1 = rr.RegisterRule(rule1);
        var newRule2 = rr.RegisterRule(rule2);
        rr.RegisterData("Test", _Data);

        var appliedRules = rr.ApplyRules("Context", EvaluationTypeEnum.FIRST);

        Assert.Equal(DefaultRule.DECISIONWORD, appliedRules[0].Decision);
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
            TrueDecision = "EQUALS100",
            FalseDecision = "NOT_EQUALS100"
        };
        DefinedRule rule2 = new()
        {
            Number = 2,
            Name = "Equals Bool Value Rule",
            Statement = @"Test.intvalue equals 101",
            Context = "Context",
            TrueDecision = "EQUALS101",
            FalseDecision = "NOT_EQUALS101"
        };

        IRightRule rr = new RightRule.RightRule();
        var newRule1 = rr.RegisterRule(rule1);
        var newRule2 = rr.RegisterRule(rule2);
        rr.RegisterData("Test", _Data);

        var appliedRules = rr.ApplyRules("Context", EvaluationTypeEnum.ANY);

        Assert.Equal(rule1.TrueDecision, appliedRules[0].Decision);
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
            TrueDecision = "EQUALS100_1",
            FalseDecision = "NOT_EQUALS100_1"
        };
        DefinedRule rule2 = new()
        {
            Number = 2,
            Name = "Equals Bool Value Rule2",
            Statement = @"Test.intvalue equals 100",
            Context = "Context",
            TrueDecision = "EQUALS100_2",
            FalseDecision = "NOT_EQUALS100_2"
        };

        IRightRule rr = new RightRule.RightRule();
        var newRule1 = rr.RegisterRule(rule1);
        var newRule2 = rr.RegisterRule(rule2);
        rr.RegisterData("Test", _Data);

        var appliedRules = rr.ApplyRules("Context", EvaluationTypeEnum.ANY);

        Assert.Equal(rule1.TrueDecision, appliedRules[0].Decision);
        Assert.Equal(rule2.TrueDecision, appliedRules[1].Decision);
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
            TrueDecision = "EQUALS101",
            FalseDecision = "NOT_EQUALS101"
        };
        DefinedRule rule2 = new()
        {
            Number = 2,
            Name = "Equals Bool Value Rule2",
            Statement = @"Test.intvalue equals 102",
            Context = "Context",
            TrueDecision = "EQUALS102",
            FalseDecision = "NOT_EQUALS102"
        };

        IRightRule rr = new RightRule.RightRule();
        var newRule1 = rr.RegisterRule(rule1);
        var newRule2 = rr.RegisterRule(rule2);
        rr.RegisterData("Test", _Data);

        var appliedRules = rr.ApplyRules("Context", EvaluationTypeEnum.ANY);

        Assert.Equal(rule1.FalseDecision, appliedRules[0].Decision);
        Assert.Equal(rule2.FalseDecision, appliedRules[1].Decision);
    }
}
