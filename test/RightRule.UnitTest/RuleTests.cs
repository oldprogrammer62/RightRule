using BeSoft;
using RightRule.Rules;

namespace RightRuleTests;

public class RuleTests
{
    RuleDefinition rule1;
    RuleDefinition rule2;

    public RuleTests()
    {
        rule1 = new RuleDefinition()
        {
            Number = 1,
            Name = "Rule1",
            Context = "Rule1_Context",
            Decision = "DONE",
            Statement = "a.b = c.d"
        };

        rule2 = new RuleDefinition()
        {
            Number = 2,
            Name = "Rule2",
            Context = "Rule2_Context",
            Decision = "DONE",
            Statement = "a.b = c.d"
        };
    }

    [Fact]
    public void GetRule1()
    {
        IRightRule rr = new BeSoft.RightRule();
        rr.RegisterRule(rule1);
        rr.RegisterRule(rule2);
        Assert.Equal(2, rr.RegisteredRules.Count);

        var rules = rr.GetRules("Rule1_Context");
        Assert.NotNull(rules);
        Assert.Single(rules);

        var r1 = rules[0];
        Assert.Equal("Rule1", r1.Name);
    }

    [Fact]
    public void GetRule2()
    {
        IRightRule rr = new BeSoft.RightRule();
        rr.RegisterRule(rule1);
        rr.RegisterRule(rule2);
        Assert.Equal(2, rr.RegisteredRules.Count);

        var rules = rr.GetRules("Rule2_Context");
        Assert.NotNull(rules);
        Assert.Single(rules);

        var r2 = rules[0];
        Assert.Equal("Rule2", r2.Name);
    }

    [Fact]
    public void GetRules()
    {
        IRightRule rr = new BeSoft.RightRule();
        rr.RegisterRule(rule1);
        rr.RegisterRule(rule2);
        Assert.Equal(2, rr.RegisteredRules.Count);

        RegisteredRules? registeredRules = rr.RegisteredRules;
        Assert.NotNull(registeredRules);
        Assert.Equal(2, registeredRules.Count);

        var r1 = registeredRules.Rules[0];
        Assert.Equal("Rule1", r1.Name);

        var r2 = registeredRules.Rules[1];
        Assert.Equal("Rule2", r2.Name);
    }
}
