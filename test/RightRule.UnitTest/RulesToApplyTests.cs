using RightRule.Models.RuleModels;
using RightRule.RuleApplication;
using RightRule.System.Common;

namespace RightRuleTests;

public class RulesToApplyTests
{
    private readonly Dictionary<string, object> _Data;
    private readonly DefinedRule rule1;
    private readonly DefinedRule rule2;

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

        // Rule
        rule1 = new()
        {
            Number = 1,
            Name = "Number of orders Rule",
            Statement = @"Test.numberoforders equals 10",
            Context = "AllContext",
            Decision = "GOODRULE1",
            FalseDecision = "BADRULE1"
        };

        // Rule
        rule2 = new()
        {
            Number = 2,
            Name = "Name Rule",
            Statement = @"Test.surname equals ""Doe""",
            Context = "AllContext",
            Decision = "GOODRULE2",
            FalseDecision = "BADRULE2"
        };
    }

    [Fact]
    public void OneRule_First_Unspecified()
    {
        BeSoft.IRightRule rr = new BeSoft.RightRule();
        var newRule1 = rr.RegisterRule(rule1);
        rr.RegisterData("Test", _Data);

        var appliedRule = rr.ApplyRule("AllContext");

        Assert.Equal(appliedRule.Decision, rule1.Decision);
    }

    [Fact]
    public void OneRule_All_Unspecified()
    {
        BeSoft.IRightRule rr = new BeSoft.RightRule();
        var newRule1 = rr.RegisterRule(rule1);
        rr.RegisterData("Test", _Data);

        var appliedRules = rr.ApplyRules("AllContext", RulesToReturnEnum.ALL);
        var appliedRule = appliedRules.FirstOrDefault();
        Assert.Equal(appliedRule.Decision, rule1.Decision);
    }

    [Fact]
    public void TwoRules_First_Unspecified()
    {
        BeSoft.IRightRule rr = new BeSoft.RightRule();
        var newRule1 = rr.RegisterRule(rule1);
        var newRule2 = rr.RegisterRule(rule2);
        rr.RegisterData("Test", _Data);

        var appliedRule = rr.ApplyRule("AllContext");

        Assert.Equal(appliedRule.Decision, rule1.Decision);
    }

    [Fact]
    public void TwoRules_All_Unspecified()
    {
        BeSoft.IRightRule rr = new BeSoft.RightRule();
        var newRule1 = rr.RegisterRule(rule1);
        var newRule2 = rr.RegisterRule(rule2);
        rr.RegisterData("Test", _Data);

        var appliedRules = rr.ApplyRules("AllContext", RulesToReturnEnum.ALL);
        var appliedRule = appliedRules.FirstOrDefault();
        Assert.Equal(appliedRule.Decision, rule1.Decision);
    }

    [Fact]
    public void TwoRules_First_Unspecified_WithDataSet()
    {
        BeSoft.IRightRule rr = new BeSoft.RightRule();
        var newRule1 = rr.RegisterRule(rule1);
        var newRule2 = rr.RegisterRule(rule2);

        var appliedRule = rr.ApplyRule("AllContext", "Test", _Data);
        Assert.Equal(appliedRule.Decision, rule1.Decision);
    }

    [Fact]
    public void TwoRules_All_Unspecified_WithDataSet()
    {
        BeSoft.IRightRule rr = new BeSoft.RightRule();
        var newRule1 = rr.RegisterRule(rule1);
        var newRule2 = rr.RegisterRule(rule2);

        var appliedRules = rr.ApplyRules("AllContext", "Test", _Data, RulesToReturnEnum.ALL, DataRetentionEnum.DISCARD);
        var appliedRule = appliedRules.FirstOrDefault();
        Assert.Equal(appliedRule.Decision, rule1.Decision);
    }

    [Fact]
    public void TwoRules_FirstSpecified_WithDataSet()
    {
        BeSoft.IRightRule rr = new BeSoft.RightRule();
        var newRule1 = rr.RegisterRule(rule1);
        var newRule2 = rr.RegisterRule(rule2);

        var appliedRules = rr.ApplyRules("AllContext", "Test", _Data, RulesToReturnEnum.ALL, DataRetentionEnum.DISCARD);
        var appliedRule = appliedRules.FirstOrDefault();
        Assert.Equal(appliedRule.Decision, rule1.Decision);
    }

    [Fact]
    public void TwoRules_AllSpecified_WithDataSet()
    {
        BeSoft.IRightRule rr = new BeSoft.RightRule();
        var newRule1 = rr.RegisterRule(rule1);
        var newRule2 = rr.RegisterRule(rule2);

        var appliedRules = rr.ApplyRules("AllContext", "Test", _Data, RulesToReturnEnum.ALL, DataRetentionEnum.KEEP);
        var appliedRule = appliedRules.FirstOrDefault();
        Assert.Equal(appliedRule.Decision, rule1.Decision);
    }

    [Fact]
    public void TwoRules_First_WithMultiRuleRegistration()
    {
        var rules = new List<DefinedRule>() { rule1, rule2 };

        BeSoft.IRightRule rr = new BeSoft.RightRule();
        var newRules = rr.RegisterRules(rules);
        rr.RegisterData("Test", _Data);

        var appliedRule = rr.ApplyRule("AllContext");

        Assert.Equal(appliedRule.Decision, rule1.Decision);
    }

    [Fact]
    public void TestItWorks_All()
    {
        BeSoft.IRightRule rr = new BeSoft.RightRule();
        var newRule1 = rr.RegisterRule(rule1);
        var newRule2 = rr.RegisterRule(rule2);
        rr.RegisterData("Test", _Data); 

        var appliedRules = rr.ApplyRules("AllContext", RulesToReturnEnum.ALL);
        var appliedRule = appliedRules.LastOrDefault();
        Assert.Equal(appliedRule.Decision, rule2.Decision);
    }

    [Fact]
    public void TestItWorks_All_None_First()
    {
        BeSoft.IRightRule rr = new BeSoft.RightRule();
        var newRule1 = rr.RegisterRule(rule1);
        var newRule2 = rr.RegisterRule(rule2);
        rr.RegisterData("Test", _Data);

        var appliedRules = rr.ApplyRules("AllContext", RulesToReturnEnum.ALL);
        var appliedRule = appliedRules.FirstOrDefault();

        Assert.Equal(appliedRule.Decision, newRule1.RegisteredRule.Decision);
    }

    [Fact]
    public void TestItWorks_All_None_Last()
    {
        BeSoft.IRightRule rr = new BeSoft.RightRule();
        var newRule1 = rr.RegisterRule(rule1);
        var newRule2 = rr.RegisterRule(rule2);
        rr.RegisterData("Test", _Data);

        var appliedRules = rr.ApplyRules("AllContext", RulesToReturnEnum.ALL);
        var appliedRule = appliedRules.LastOrDefault();

        Assert.Equal(appliedRule.Decision, newRule2.RegisteredRule.Decision);
    }
}
