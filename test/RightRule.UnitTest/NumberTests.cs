using RightRule.RuleModels;

namespace RightRuleTests;

public class NumberTests
{
    private readonly Dictionary<string, object> _Data;

    public NumberTests()
    {
        // Data
        _Data = new Dictionary<string, object>(StringComparer.Ordinal)
        {
            { "value", 100 }
        };
    }

    [Fact]
    public void IntegerValueTest()
    {
        // Rule
        DefinedRule rule = new()
        {
            Number = 1,
            Name = "Greater Than Value Rule",
            Statement = @"Test.value greaterthanorequals 10",
            Context = "Context",
            Decision = "GREATERTHANOREQUALS",
            FalseDecision = "LESSTHANOREQUALS"
        };

        BeSoft.IRightRule rr = new BeSoft.RightRule();
        var newRule = rr.RegisterRule(rule);
        rr.RegisterData("Test", _Data);

        var appliedRule = rr.ApplyRules("Context");

        Assert.Equal(rule.Decision, appliedRule.Decision);
    }

    [Fact]
    public void DecimalValueTest_True()
    {
        // Rule
        DefinedRule rule = new()
        {
            Number = 1,
            Name = "Greater Than Value Rule",
            Statement = @"Test.value greaterthanorequals 10.1",
            Context = "Context",
            Decision = "GREATERTHANOREQUALS",
            FalseDecision = "LESSTHANOREQUALS"
        };

        BeSoft.IRightRule rr = new BeSoft.RightRule();
        var newRule = rr.RegisterRule(rule);
        rr.RegisterData("Test", _Data);

        var appliedRule = rr.ApplyRules("Context");

        Assert.Equal(rule.Decision, appliedRule.Decision);
    }

    [Fact]
    public void DecimalValueTest_True2()
    {
        // Rule
        DefinedRule rule = new()
        {
            Number = 1,
            Name = "Greater Than Value Rule",
            Statement = @"Test.value greaterthanorequals 99.99",
            Context = "Context",
            Decision = "GREATERTHANOREQUALS",
            FalseDecision = "LESSTHANOREQUALS"
        };

        BeSoft.IRightRule rr = new BeSoft.RightRule();
        var newRule = rr.RegisterRule(rule);
        rr.RegisterData("Test", _Data);

        var appliedRule = rr.ApplyRules("Context");

        Assert.Equal(rule.Decision, appliedRule.Decision);
    }

    [Fact]
    public void DecimalValueTest_False()
    {
        // Rule
        DefinedRule rule = new()
        {
            Number = 1,
            Name = "Greater Than Value Rule",
            Statement = @"Test.value greaterthanorequals 100.00001",
            Context = "Context",
            Decision = "GREATERTHANOREQUALS",
            FalseDecision = "LESSTHANOREQUALS"
        };

        BeSoft.IRightRule rr = new BeSoft.RightRule();
        var newRule = rr.RegisterRule(rule);
        rr.RegisterData("Test", _Data);

        var appliedRule = rr.ApplyRules("Context");

        Assert.Equal(DefaultRule.DECISIONWORD, appliedRule.Decision);
    }

    [Fact]
    public void DecimalValueTest_False_TwoConditions()
    {
        // Rule
        DefinedRule rule = new()
        {
            Number = 1,
            Name = "Greater Than Value Rule",
            Statement = @"Test.value >= 99.9999 and Test.value < 200.001",
            Context = "Context",
            Decision = "GREATERTHANOREQUALS",
            FalseDecision = "LESSTHANOREQUALS"
        };

        BeSoft.IRightRule rr = new BeSoft.RightRule();
        var newRule = rr.RegisterRule(rule);
        rr.RegisterData("Test", _Data);

        var appliedRule = rr.ApplyRules("Context");
        Assert.Equal(rule.Decision, appliedRule.Decision);
    }
}
