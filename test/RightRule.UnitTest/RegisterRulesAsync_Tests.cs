using RightRule;
using RightRule.RuleServices.RuleModels;

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
            Statement = "a.b = c.d",
            TrueDecision = "DONE",
            FalseDecision = "NOT_DONE",
        };
        DefinedRule rule2 = new DefinedRule()
        {
            Context = "Context",
            Name = "Rule2",
            Number = 2,
            Statement = "a.b = c.d",
            TrueDecision = "DONE",
            FalseDecision = "NOT_DONE",
        };
        DefinedRule rule3 = new DefinedRule()
        {
            Context = "Context",
            Name = "Rule3",
            Number = 3,
            Statement = "a.b = c.d",
            TrueDecision = "DONE",
            FalseDecision = ""
        };
        List<DefinedRule> rules = new List<DefinedRule>()
        {
            rule1,
            rule2,
            rule3 // This rule will fail validation - Missing False Decision
        };

        IRightRule rr = new RightRule.RightRule();
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
            Statement = "a.b = c.d",
            TrueDecision = "DONE",
            FalseDecision = "NOT_DONE"
        };
        DefinedRule rule2 = new DefinedRule()
        {
            Context = "Context",
            Name = "Rule2",
            Number = 2,
            Statement = "a.b = c.d",
            TrueDecision = "DONE",
            FalseDecision = "NOT_DONE"
        };

        IRightRule rr = new RightRule.RightRule();
        var registeredRule1 = rr.RegisterRule(rule1);
        var registeredRule2 = rr.RegisterRule(rule2);
        Assert.NotNull(registeredRule1);
        Assert.NotNull(registeredRule2);

        var registeredRules = rr.GetRegisteredRules();
        Assert.NotNull(registeredRules);
        Assert.Equal(2, registeredRules.Count);
    }
}
