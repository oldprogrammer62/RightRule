using RightRule.Rules;

namespace RightRuleTests;

public class IdentifierWithDotTests
{
    private readonly Dictionary<string, object> _Constants;
    private readonly Dictionary<string, object> _Data;

    public IdentifierWithDotTests()
    {
        // Constants
        _Constants = new Dictionary<string, object>(StringComparer.Ordinal)
        {
            { "intvalue", 100 }
        };

        // Data
        _Data = new Dictionary<string, object>(StringComparer.Ordinal)
        {
            { "intvalue", 100 }
        };
    }

    [Fact]
    public void Equals_Int_And_Int()
    {
        // Rule
        RuleDefinition rule = new()
        {
            Number = 1,
            Name = "Equals Int Value Rule",
            Statement = "Data.intvalue equals Constants.intvalue",
            Context = "Context",
            Decision = "EQUALS",
        };

        RightRule.RightRule rr = new RightRule.RightRule();
        var newRule = rr.RegisterRule(rule);
        rr.RegisterData("Data", _Data);
        rr.RegisterData("Constants", _Constants);

        var appliedRule = rr.ApplyRule("Context");

        Assert.Equal(rule.Decision, appliedRule.Decision);
    }
}
