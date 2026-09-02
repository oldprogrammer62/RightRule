using RightRule.RuleModels;

namespace RightRuleTests;

public class ApplyRuleMultipleDatasetsTests
{
    private readonly Dictionary<string, object> _DataA;
    private readonly Dictionary<string, object> _DataB;
    private readonly DefinedRule rule1;
    private readonly DefinedRule rule2;

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
            Statement = @"TestA.numberoforders equals 10",
            Context = "AllContext",
            Decision = "GOODRULE1",
            FalseDecision = "BADRULE1"
        };

        // Rule matching dataset B by surname
        rule2 = new()
        {
            Number = 2,
            Name = "Surname is Smith Rule",
            Statement = @"TestB.surname equals ""Smith""",
            Context = "AllContext",
            Decision = "GOODRULE2",
            FalseDecision = "BADRULE2"
        };
    }

    [Fact]
    public void ApplyRule_Targets_Correct_Dataset_A()
    {
        var rr = new BeSoft.RightRule();

        rr.RegisterRule(rule1);
        rr.RegisterRule(rule2);

        rr.RegisterData("TestA", _DataA);
        rr.RegisterData("TestB", _DataB);

        var appliedA = rr.ApplyRules("AllContext");

        Assert.Equal(rule1.Decision, appliedA.Decision);
    }

    [Fact]
    public void ApplyRule_Targets_Correct_Dataset_B()
    {
        var rr = new BeSoft.RightRule();

        rr.RegisterRule(rule1);
        rr.RegisterRule(rule2);

        rr.RegisterData("TestA", _DataA);
        rr.RegisterData("TestB", _DataB);

        var appliedB = rr.ApplyRules("AllContext");

        Assert.Equal(rule1.Decision, appliedB.Decision);
    }
}