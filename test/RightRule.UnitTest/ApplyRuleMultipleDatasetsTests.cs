using RightRule.Extensions;
using RightRule.Rules;

namespace RightRuleTests;

public class ApplyRuleMultipleDatasetsTests
{
    private readonly Dictionary<string, object> _DataA;
    private readonly Dictionary<string, object> _DataB;
    private readonly RuleDefinition rule1;
    private readonly RuleDefinition rule2;

    public ApplyRuleMultipleDatasetsTests()
    {
        // Dataset A
        _DataA = new Dictionary<string, object>(StringComparer.Ordinal)
        {
            { "name", "John" },
            { "surname", "Doe" },
            { "fullname", "John D. Doe" },
            { "numberoforders", 10 },
            { "goodpayer", true },
            { "joined", new DateTime(2020, 1, 1) }
        };

        // Dataset B (different values so different rule matches)
        _DataB = new Dictionary<string, object>(StringComparer.Ordinal)
        {
            { "name", "Jane" },
            { "surname", "Smith" },
            { "fullname", "Jane S. Smith" },
            { "numberoforders", 5 },
            { "goodpayer", false },
            { "joined", new DateTime(2021, 6, 1) }
        };

        // Rule matching dataset A by numberoforders
        rule1 = new()
        {
            Number = 1,
            Name = "Number of orders Rule",
            Statement = @"numberoforders equals 10",
            Context = "AllContext",
            Decision = "GOODRULE1",
        };

        // Rule matching dataset B by surname
        rule2 = new()
        {
            Number = 2,
            Name = "Surname is Smith Rule",
            Statement = @"surname equals ""Smith""",
            Context = "AllContext",
            Decision = "GOODRULE2",
        };
    }

    [Fact]
    public void ApplyRule_Targets_Correct_Dataset_A()
    {
        var rr = new RightRule.RightRule();

        rr.RegisterRule(rule1);
        rr.RegisterRule(rule2);

        rr.RegisterData("TestA", _DataA);
        rr.RegisterData("TestB", _DataB);

        var appliedA = rr.ApplyRule("AllContext", "TestA");

        Assert.Equal(rule1.Decision, appliedA.Decision);
    }

    [Fact]
    public void ApplyRule_Targets_Correct_Dataset_B()
    {
        var rr = new RightRule.RightRule();

        rr.RegisterRule(rule1);
        rr.RegisterRule(rule2);

        rr.RegisterData("TestA", _DataA);
        rr.RegisterData("TestB", _DataB);

        var appliedB = rr.ApplyRule("AllContext", "TestB");

        Assert.Equal(rule2.Decision, appliedB.Decision);
    }
}