using RightRule.Rules;

namespace RightRuleTests;

public class DataRegistrationTests
{
    private readonly Dictionary<string, object> _Data;

    public DataRegistrationTests()
    {
        // Data
        _Data = new Dictionary<string, object>(StringComparer.Ordinal)
        {
            { "name", "John" },
            { "surname", "Doe" },
            { "fullname", "John D. Doe" },
            { "age", 28 },
            { "numberoforders", 10 },
            { "goodpayer", true },
            { "joined", new DateTime(2020, 1, 1) } // yyyy mm dd
        };
    }

    [Fact]
    public void RegisterDataTest()
    {
        RuleDefinition rule1 = new RuleDefinition
        {
            Number = 1,
            Name = "Age Rule",
            Statement = @"age equals 28",
            Context = "AllContext",
            Decision = "OLDENOUGH",
        };

        var rr = new RightRule.RightRule();

        rr.RegisterData("Test", _Data);
        var registeredRule = rr.RegisterRule(rule1);

        Assert.NotNull(registeredRule);
    }

    [Fact]
    public void RegisterListDataTest()
    {
        RuleDefinition rule1 = new RuleDefinition
        {
            Number = 1,
            Name = "Age Rule",
            Statement = @"age equals 28",
            Context = "AllContext",
            Decision = "OLDENOUGH",
        };

        var rr = new RightRule.RightRule();

        rr.RegisterData<Dictionary<string, object>>("Test", _Data);
        var registeredRule = rr.RegisterRule(rule1);

        Assert.NotNull(registeredRule);
    }

    [Fact]
    public void RegisterDataAndApplyRulesTest()
    {
        // Rule
        RuleDefinition rule1 = new RuleDefinition
        {
            Number = 1,
            Name = "Age Rule",
            Statement = @"age equals 28",
            Context = "AllContext",
            Decision = "OLDENOUGH",
        };

        RightRule.RightRule rr = new RightRule.RightRule();

        rr.RegisterData("Test", _Data);
        var newRule1 = rr.RegisterRule(rule1);

        var appliedRule = rr.ApplyRules("AllContext", RightRule.System.Common.RulesToApply.First);

        Assert.Equal(appliedRule.Decision, rule1.Decision);
    }
}
