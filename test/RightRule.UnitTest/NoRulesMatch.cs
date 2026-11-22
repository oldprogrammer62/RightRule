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
        };
        DefinedRule rule2 = new()
        {
            Number = 2,
            Name = "Equals Bool Value Rule",
            Statement = @"Test.intvalue equals 101",
            Context = "Context",
            TrueDecision = "EQUALS101",
        };

        var rr = new RightRule.RightRule().CreateSession();
        var newRule = rr.RuleService.RegisterRule(rule1);
        rr.DataService.RegisterData("Test", _Data);

        var appliedRule = rr.RuleService.ApplyRule("Context");

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
        };
        DefinedRule rule2 = new()
        {
            Number = 2,
            Name = "Equals Bool Value Rule2",
            Statement = @"Test.intvalue equals 100",
            Context = "Context",
            TrueDecision = "EQUALS100",
        };

        var rr = new RightRule.RightRule().CreateSession();
        var newRule1 = rr.RuleService.RegisterRule(rule1);
        var newRule2 = rr.RuleService.RegisterRule(rule2);
        rr.DataService.RegisterData("Test", _Data);

        var appliedRule = rr.RuleService.ApplyRule("Context");

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
        };

        var rr = new RightRule.RightRule().CreateSession();
        var newRule = rr.RuleService.RegisterRule(rule1);
        rr.DataService.RegisterData("Test", _Data);

        var appliedRule = rr.RuleService.ApplyRule("Context");

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
        };
        DefinedRule rule2 = new()
        {
            Number = 2,
            Name = "Equals Bool Value Rule",
            Statement = @"Test.intvalue equals 101",
            Context = "Context",
            TrueDecision = "EQUALS",
        };

        var rr = new RightRule.RightRule().CreateSession();
        var newRule1 = rr.RuleService.RegisterRule(rule1);
        var newRule2 = rr.RuleService.RegisterRule(rule2);
        rr.DataService.RegisterData("Test", _Data);

        var appliedRule = rr.RuleService.ApplyRule("Context");

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
        };
        DefinedRule rule2 = new()
        {
            Number = 2,
            Name = "Equals Bool Value Rule",
            Statement = @"Test.intvalue equals 101",
            Context = "Context",
            TrueDecision = "EQUALS101",
        };

        var rr = new RightRule.RightRule().CreateSession();
        var newRule1 = rr.RuleService.RegisterRule(rule1);
        var newRule2 = rr.RuleService.RegisterRule(rule2);
        rr.DataService.RegisterData("Test", _Data);

        var appliedRules = rr.RuleService.ApplyRules("Context", EvaluationTypeEnum.FIRST);

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
        };
        DefinedRule rule2 = new()
        {
            Number = 2,
            Name = "Equals Bool Value Rule2",
            Statement = @"Test.intvalue equals 100",
            Context = "Context",
            TrueDecision = "EQUALS100",
        };

        var rr = new RightRule.RightRule().CreateSession();
        var newRule1 = rr.RuleService.RegisterRule(rule1);
        var newRule2 = rr.RuleService.RegisterRule(rule2);
        rr.DataService.RegisterData("Test", _Data);

        var appliedRules = rr.RuleService.ApplyRules("Context", EvaluationTypeEnum.FIRST);

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
        };
        DefinedRule rule2 = new()
        {
            Number = 2,
            Name = "Equals Bool Value Rule",
            Statement = @"Test.intvalue equals 102",
            Context = "Context",
            TrueDecision = "EQUALS102",
        };

        var rr = new RightRule.RightRule().CreateSession();
        var newRule1 = rr.RuleService.RegisterRule(rule1);
        var newRule2 = rr.RuleService.RegisterRule(rule2);
        rr.DataService.RegisterData("Test", _Data);

        var appliedRules = rr.RuleService.ApplyRules("Context", EvaluationTypeEnum.FIRST);

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
        };
        DefinedRule rule2 = new()
        {
            Number = 2,
            Name = "Equals Bool Value Rule",
            Statement = @"Test.intvalue equals 101",
            Context = "Context",
            TrueDecision = "EQUALS101",
        };

        var rr = new RightRule.RightRule().CreateSession();
        var newRule1 = rr.RuleService.RegisterRule(rule1);
        var newRule2 = rr.RuleService.RegisterRule(rule2);
        rr.DataService.RegisterData("Test", _Data);

        var appliedRules = rr.RuleService.ApplyRules("Context", EvaluationTypeEnum.ANY);

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
        };
        DefinedRule rule2 = new()
        {
            Number = 2,
            Name = "Equals Bool Value Rule2",
            Statement = @"Test.intvalue equals 100",
            Context = "Context",
            TrueDecision = "EQUALS100_2",
        };

        var rr = new RightRule.RightRule().CreateSession();
        var newRule1 = rr.RuleService.RegisterRule(rule1);
        var newRule2 = rr.RuleService.RegisterRule(rule2);
        rr.DataService.RegisterData("Test", _Data);

        var appliedRules = rr.RuleService.ApplyRules("Context", EvaluationTypeEnum.ANY);

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
        };
        DefinedRule rule2 = new()
        {
            Number = 2,
            Name = "Equals Bool Value Rule2",
            Statement = @"Test.intvalue equals 102",
            Context = "Context",
            TrueDecision = "EQUALS102",
        };

        var rr = new RightRule.RightRule().CreateSession();
        var newRule1 = rr.RuleService.RegisterRule(rule1);
        var newRule2 = rr.RuleService.RegisterRule(rule2);
        rr.DataService.RegisterData("Test", _Data);

        var appliedRules = rr.RuleService.ApplyRules("Context", EvaluationTypeEnum.ANY);

        Assert.Equal(rule1.TrueDecision, appliedRules[0].Decision);
        Assert.Equal(rule2.TrueDecision, appliedRules[1].Decision);
    }
}
