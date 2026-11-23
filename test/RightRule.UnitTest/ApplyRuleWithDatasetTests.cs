using RightRule;
using RightRule.Models;
using RightRule.RuleServices.RuleModels;

namespace RightRuleTests;

public class ApplyRuleWithDatasetTests
{
    private readonly Dictionary<string, object> _Data;
    private readonly DefinedRule rule1;
    private readonly DefinedRule rule2;
    private readonly DefinedRule rule3;

    public ApplyRuleWithDatasetTests()
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

        // Rule 1
        rule1 = new()
        {
            Number = 1,
            Name = "Number of orders Rule",
            Statement = @"Test.numberoforders equals 10",
            Context = "AllContext",
            Decision = "GOODRULE1",
        };

        // Rule 2
        rule2 = new()
        {
            Number = 2,
            Name = "Name Rule",
            Statement = @"Test.surname equals ""Doe""",
            Context = "AllContext",
            Decision = "GOODRULE2",
        };

        // Rule 3
        rule3 = new()
        {
            Number = 3,
            Name = "Name Rule",
            Statement = @"XXTest.surname equals ""Doe""",
            Context = "AllContext",
            Decision = "GOODRULE3",
        };
    }

    [Fact]
    public void OneRule_WithDataSet()
    {
        var rr = new RightRule.RightRule().CreateSession();
        rr.DataService.RegisterData("Test", _Data);
        var newRule1 = rr.RuleService.RegisterRule(rule1);

        var appliedRule = rr.RuleService.ApplyRule("AllContext");

        Assert.Equal(appliedRule.Decision, rule1.Decision);
    }

    [Fact]
    public void TwoRules_WithDataSet()
    {
        var rr = new RightRule.RightRule().CreateSession();
        var newRule1 = rr.RuleService.RegisterRule(rule1);
        var newRule2 = rr.RuleService.RegisterRule(rule2);
        rr.DataService.RegisterData("Test", _Data);

        var appliedRule = rr.RuleService.ApplyRule("AllContext");

        Assert.Equal(appliedRule.Decision, rule1.Decision);
    }

    [Fact]
    public void TwoRules_WithInvalidDataSet()
    {
        var rr = new RightRule.RightRule().CreateSession();
        var newRule3 = rr.RuleService.RegisterRule(rule3);
        rr.DataService.RegisterData("Test", _Data);

        var appliedRule = rr.RuleService.ApplyRule("AllContext");

        Assert.Equal(ErrorRule.DECISIONWORD, appliedRule.Decision);
        Assert.Equal(RightRuleResultEnum.ERROR, appliedRule.Status);
    }
}