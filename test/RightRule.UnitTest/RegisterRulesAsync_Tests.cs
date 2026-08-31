using RightRule.RuleModels;

namespace RightRuleTests;

public class RegisterRulesAsync_Tests
{
    [Fact]
    public void RegisterRulesAsync()
    {
        DefinedRule rule1 = new DefinedRule()
        {
            Context = "Context",
            Name = "Rule1",
            Number = 1,
            Decision = "DONE",
            Statement = "a.b = c.d"
        };
        DefinedRule rule2 = new DefinedRule()
        {
            Context = "Context",
            Name = "Rule2",
            Number = 2,
            Decision = "DONE",
            Statement = "a.b = c.d"
        };
        List<DefinedRule> rules = new List<DefinedRule>()
        {
            rule1,
            rule2
        };

        BeSoft.IRightRule rr = new BeSoft.RightRule();
        var registeredRules = rr.RegisterRules(rules);

        Assert.NotNull(registeredRules);
        Assert.Equal(2, registeredRules.Count);
    }

    [Fact]
    public void RegisterRuleAsync()
    {
        DefinedRule rule1 = new DefinedRule()
        {
            Context = "Context",
            Name = "Rule1",
            Number = 1,
            Decision = "DONE",
            Statement = "a.b = c.d"
        };
        DefinedRule rule2 = new DefinedRule()
        {
            Context = "Context",
            Name = "Rule2",
            Number = 2,
            Decision = "DONE",
            Statement = "a.b = c.d"
        };

        BeSoft.IRightRule rr = new BeSoft.RightRule();
        var registeredRule1 = rr.RegisterRule(rule1);
        var registeredRule2 = rr.RegisterRule(rule2);
        Assert.NotNull(registeredRule1);
        Assert.NotNull(registeredRule2);

        var registeredRules = rr.RegisteredRules();
        Assert.NotNull(registeredRules);
        Assert.Equal(2, registeredRules.Count);
    }
}
