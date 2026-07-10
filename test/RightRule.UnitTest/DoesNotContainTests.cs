using RightRule.Extensions;
using RightRule.Rules;

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
        RuleDefinition rule = new()
        {
            Number = 1,
            Name = "Name Rule",
            Statement = @"Test.fullname doesnotcontain ""XXDoeXX""",
            Context = "NameContext",
            Decision = "NAMEDECISION",
        };

        RightRule.RightRule rr = new RightRule.RightRule();
        var newRule = rr.RegisterRule(rule);
        rr.RegisterData("Test", _Data);

        var appliedRule = rr.ApplyRule("NameContext");

        Assert.Equal(rule.Decision, appliedRule.Decision);
    }

    [Fact]
    public void DoesNotContain2()
    {
        // Rule
        RuleDefinition rule = new()
        {
            Number = 1,
            Name = "Name Rule",
            Statement = @"Test.fullname notc ""Doe""",
            Context = "NameContext",
            Decision = "NAMEDECISION",
        };

        RightRule.RightRule rr = new RightRule.RightRule();
        var newRule = rr.RegisterRule(rule);
        rr.RegisterData("Test", _Data);

        var appliedRule = rr.ApplyRule("NameContext");

        Assert.Equal(AppliedRule.DefaultRule().Decision, appliedRule.Decision);
    }
}
