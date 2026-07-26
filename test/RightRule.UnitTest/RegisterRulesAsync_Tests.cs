using RightRule.Rules;

namespace RightRuleTests;

public class RegisterRulesAsync_Tests
{
    [Fact]
    public async Task RegisterRulesAsync()
    {
        RuleDefinition rule1 = new RuleDefinition()
        {
            Context = "Context",
            Name = "Rule1",
            Number = 1,
            Decision = "DONE",
            Statement = "a.b = c.d"
        };
        RuleDefinition rule2 = new RuleDefinition()
        {
            Context = "Context",
            Name = "Rule2",
            Number = 2,
            Decision = "DONE",
            Statement = "a.b = c.d"
        };
        List<RuleDefinition> rules = new List<RuleDefinition>()
        {
            rule1,
            rule2
        };

        BeSoft.IRightRule rr = new BeSoft.RightRule();
        var registeredRules = await rr.RegisterRulesAsync(rules);

        Assert.NotNull(registeredRules);
        Assert.Equal(2, registeredRules.Count);
    }

    [Fact]
    public async Task RegisterRuleAsync()
    {
        RuleDefinition rule1 = new RuleDefinition()
        {
            Context = "Context",
            Name = "Rule1",
            Number = 1,
            Decision = "DONE",
            Statement = "a.b = c.d"
        };
        RuleDefinition rule2 = new RuleDefinition()
        {
            Context = "Context",
            Name = "Rule2",
            Number = 2,
            Decision = "DONE",
            Statement = "a.b = c.d"
        };

        BeSoft.IRightRule rr = new BeSoft.RightRule();
        var registeredRule1 = await rr.RegisterRuleAsync(rule1);
        var registeredRule2 = await rr.RegisterRuleAsync(rule2);
        Assert.NotNull(registeredRule1);
        Assert.NotNull(registeredRule2);

        var registeredRules = rr.RegisteredRules();
        Assert.NotNull(registeredRules);
        Assert.Equal(2, registeredRules.Count);
    }
}
