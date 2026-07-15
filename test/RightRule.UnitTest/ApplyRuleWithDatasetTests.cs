using RightRule.Rules;

namespace RightRuleTests;

public class ApplyRuleWithDatasetTests
{
    private readonly Dictionary<string, object> _Data;
    private readonly RuleDefinition rule1;
    private readonly RuleDefinition rule2;
    private readonly RuleDefinition rule3;

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
        RightRule.RightRule rr = new RightRule.RightRule();
        rr.RegisterData("Test", _Data);
        var newRule1 = rr.RegisterRule(rule1);

        var appliedRule = rr.ApplyRule("AllContext");

        Assert.Equal(appliedRule.Decision, rule1.Decision);
    }

    [Fact]
    public void TwoRules_WithDataSet()
    {
        RightRule.RightRule rr = new RightRule.RightRule();
        var newRule1 = rr.RegisterRule(rule1);
        var newRule2 = rr.RegisterRule(rule2);
        rr.RegisterData("Test", _Data);

        var appliedRule = rr.ApplyRule("AllContext");

        Assert.Equal(appliedRule.Decision, rule1.Decision);
    }

    [Fact]
    public void TwoRules_WithInvalidDataSet()
    {
        RightRule.RightRule rr = new RightRule.RightRule();
        var newRule3 = rr.RegisterRule(rule3);
        rr.RegisterData("Test", _Data);

        var appliedRule = rr.ApplyRule("AllContext");

        Assert.Equal(AppliedRule.DefaultRule().Decision, appliedRule.Decision);
    }
}