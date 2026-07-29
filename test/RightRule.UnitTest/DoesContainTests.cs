using RightRule.RuleModels;

namespace RightRuleTests;

public class DoesContainTests
{
    private readonly Dictionary<string, object> _Data;

    public DoesContainTests()
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
    public void DoesContain()
    {
        // Rule
        RuleDefinition rule = new()
        {
            Number = 1,
            Name = "Name Rule",
            Statement = @"Test.fullname contains ""Doe""",
            Context = "NameContext",
            Decision = "NAMEDECISION",
        };

        BeSoft.IRightRule rr = new BeSoft.RightRule();
        var newRule = rr.RegisterRule(rule);
        rr.RegisterDataset("Test", _Data);

        var appliedRule = rr.ApplyRule("NameContext");

        Assert.Equal(rule.Decision, appliedRule.Decision);
    }

    [Fact]
    public void DoesContain2()
    {
        // Rule
        RuleDefinition rule = new()
        {
            Number = 1,
            Name = "Name Rule",
            Statement = @"Test.fullname contains ""XXDoeXX""",
            Context = "NameContext",
            Decision = "NAMEDECISION",
        };

        BeSoft.IRightRule rr = new BeSoft.RightRule();
        var newRule = rr.RegisterRule(rule);
        rr.RegisterDataset("Test", _Data);

        var appliedRule = rr.ApplyRule("NameContext");

        Assert.Equal(AppliedRule.DefaultRule().Decision, appliedRule.Decision);
    }
}
