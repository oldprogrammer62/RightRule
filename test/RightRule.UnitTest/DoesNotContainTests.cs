using RightRule;
using RightRule.RuleServices.RuleModels;

namespace RightRuleTests;

public class DoesNotContainTests
{
    private readonly Dictionary<string, object> _Data;

    public DoesNotContainTests()
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
    }

    [Fact]
    public void DoesNotContain()
    {
        // Rule
        DefinedRule rule = new()
        {
            Number = 1,
            Name = "Name Rule",
            Statement = @"Test.fullname doesnotcontain ""XXDoeXX""",
            Context = "NameContext",
            Decision = "NAMEDECISION",
        };

        var rr = new RightRule.RightRule().CreateSession();
        var newRule = rr.RuleService.RegisterRule(rule);
        rr.DataService.RegisterData("Test", _Data);

        var appliedRule = rr.RuleService.ApplyRule("NameContext");

        Assert.Equal(rule.Decision, appliedRule.Decision);
    }

    [Fact]
    public void DoesNotContain2()
    {
        // Rule
        DefinedRule rule = new()
        {
            Number = 1,
            Name = "Name Rule",
            Statement = @"Test.fullname notc ""Doe""",
            Context = "NameContext",
            Decision = "NAMEDECISION",
        };

        var rr = new RightRule.RightRule().CreateSession();
        var newRule = rr.RuleService.RegisterRule(rule);
        rr.DataService.RegisterData("Test", _Data);

        var appliedRule = rr.RuleService.ApplyRule("NameContext");

        Assert.Equal(DefaultRule.DECISIONWORD, appliedRule.Decision);
    }
}
