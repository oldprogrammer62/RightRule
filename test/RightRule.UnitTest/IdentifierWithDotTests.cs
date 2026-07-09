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
            { "highvalue", 200 }
        };

        // Data
        _Data = new Dictionary<string, object>(StringComparer.Ordinal)
        {
            { "NewCust", true },
            { "ordervalue", 200 }
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
            Statement = "Data.ordervalue equals Constants.highvalue",
            Context = "Context",
            Decision = "EQUALS",
        };

        RightRule.RightRule rr = new RightRule.RightRule();
        var newRule = rr.RegisterRule(rule);
        rr.RegisterData("Data", _Data, StaticData: false);
        rr.RegisterData("Constants", _Constants, StaticData: true);

        var appliedRule = rr.ApplyRule("Context");

        Assert.Equal(rule.Decision, appliedRule.Decision);
    }

    [Fact]
    public void Equals_Int_And_Int2()
    {
        // Rule
        RuleDefinition rule = new()
        {
            Number = 1,
            Name = "Equals Int Value Rule",
            Statement = "NewCust = true and ordervalue = Constants.highvalue",
            Context = "Context",
            Decision = "EQUALS",
        };

        RightRule.RightRule rr = new RightRule.RightRule();
        var newRule = rr.RegisterRule(rule);
        rr.RegisterData("Data", _Data, StaticData: false);
        rr.RegisterData("Constants", _Constants, StaticData: true);

        var appliedRule = rr.ApplyRule("Context");

        Assert.Equal(rule.Decision, appliedRule.Decision);
    }
}
