using RightRule.RuleModels.RuleModels;

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
            { "highvalue", 200 },
            { "name", "123" }
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
        DefinedRule rule = new()
        {
            Number = 1,
            Name = "Equals Int Value Rule",
            Statement = "Data.ordervalue equals Constants.highvalue",
            Context = "Context",
            Decision = "EQUALS",
            FalseDecision = "NOT EQUALS"
        };

        BeSoft.IRightRule rr = new BeSoft.RightRule();
        rr.RegisterData("Data", _Data);
        rr.RegisterData("Constants", _Constants);

        var newRule = rr.RegisterRule(rule);

        var appliedRule = rr.ApplyRules("Context");

        Assert.Equal(rule.Decision, appliedRule.Decision);
    }

    [Fact]
    public void Equals_Int_And_Int2()
    {
        // Rule
        DefinedRule rule = new()
        {
            Number = 1,
            Name = "Equals Int Value Rule",
            Statement = @"Data.NewCust = true and Data.ordervalue = Constants.highvalue and ""123"" = Constants.name",
            Context = "Context",
            Decision = "EQUALS",
            FalseDecision = "NOT EQUALS"
        };

        BeSoft.IRightRule rr = new BeSoft.RightRule();
        var newRule = rr.RegisterRule(rule);
        rr.RegisterData("Data", _Data);
        rr.RegisterData("Constants", _Constants);

        var appliedRule = rr.ApplyRules("Context");

        Assert.Equal(rule.Decision, appliedRule.Decision);
    }
}
