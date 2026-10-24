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
            TrueDecision = "NAMEDECISION",
            FalseDecision = "FALSEDECISION"
        };

        IRightRule rr = new RightRule.RightRule();
        var newRule = rr.RegisterRule(rule);
        rr.RegisterData("Test", _Data);

        var appliedRule = rr.ApplyRule("NameContext");

        Assert.Equal(rule.TrueDecision, appliedRule.Decision);
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
            TrueDecision = "NAMEDECISION",
            FalseDecision = "FALSEDECISION"
        };

        IRightRule rr = new RightRule.RightRule();
        var newRule = rr.RegisterRule(rule);
        rr.RegisterData("Test", _Data);

        var appliedRule = rr.ApplyRule("NameContext");

        Assert.Equal(DefaultRule.DECISIONWORD, appliedRule.Decision);
    }
}
