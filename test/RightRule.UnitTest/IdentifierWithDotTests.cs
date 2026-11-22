using RightRule;
using RightRule.RuleServices.RuleModels;

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
        };

        var rr = new RightRule.RightRule().CreateSession();
        rr.DataService.RegisterData("Data", _Data);
        rr.DataService.RegisterData("Constants", _Constants);

        var newRule = rr.RuleService.RegisterRule(rule);

        var appliedRule = rr.RuleService.ApplyRule("Context");

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
        };

        var rr = new RightRule.RightRule().CreateSession();
        var newRule = rr.RuleService.RegisterRule(rule);
        rr.DataService.RegisterData("Data", _Data);
        rr.DataService.RegisterData("Constants", _Constants);

        var appliedRule = rr.RuleService.ApplyRule("Context");

        Assert.Equal(rule.Decision, appliedRule.Decision);
    }
}
