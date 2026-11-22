using RightRule;
using RightRule.RuleServices.RuleModels;

namespace RightRuleTests;

public class RuleTests
{
    DefinedRule rule1;
    DefinedRule rule2;

    public RuleTests()
    {
        rule1 = new DefinedRule()
        {
            Number = 1,
            Name = "Rule1",
            Context = "Rule1_Context",
            Statement = "a.b = c.d",
            Decision = "DONE",
        };

        rule2 = new DefinedRule()
        {
            Number = 2,
            Name = "Rule2",
            Context = "Rule2_Context",
            Statement = "a.b = c.d",
            Decision = "DONE",
        };
    }

    [Fact]
    public void GetRule1()
    {
        var rr = new RightRule.RightRule().CreateSession();
        rr.RuleService.RegisterRule(rule1);
        rr.RuleService.RegisterRule(rule2);
        Assert.Equal(2, rr.RuleService.GetRegisteredRules().Count);

        var rules = rr.RuleService.GetRegisteredRules("Rule1_Context");
        Assert.NotNull(rules);
        Assert.Single(rules);

        var r1 = rules[0];
        Assert.Equal("Rule1", r1.Name);
    }

    [Fact]
    public void GetRule2()
    {
        var rr = new RightRule.RightRule().CreateSession();
        rr.RuleService.RegisterRule(rule1);
        rr.RuleService.RegisterRule(rule2);
        Assert.Equal(2, rr.RuleService.GetRegisteredRules().Count);

        var rules = rr.RuleService.GetRegisteredRules("Rule2_Context");
        Assert.NotNull(rules);
        Assert.Single(rules);

        var r2 = rules[0];
        Assert.Equal("Rule2", r2.Name);
    }

    [Fact]
    public void GetRules()
    {
        var rr = new RightRule.RightRule().CreateSession();
        rr.RuleService.RegisterRule(rule1);
        rr.RuleService.RegisterRule(rule2);
        Assert.Equal(2, rr.RuleService.GetRegisteredRules().Count);

        List<ParsedRule>? registeredRules = rr.RuleService.GetRegisteredRules();
        Assert.NotNull(registeredRules);
        Assert.Equal(2, registeredRules.Count);

        var r1 = registeredRules[0];
        Assert.Equal("Rule1", r1.Name);

        var r2 = registeredRules[1];
        Assert.Equal("Rule2", r2.Name);
    }

    [Fact]
    public void GetRules_Indexed()
    {
        var rr = new RightRule.RightRule().CreateSession();
        rr.RuleService.RegisterRule(rule1);
        rr.RuleService.RegisterRule(rule2);
        Assert.Equal(2, rr.RuleService.GetRegisteredRules().Count);

        List<ParsedRule> registeredRules = rr.RuleService.GetRegisteredRules();
        Assert.NotNull(registeredRules);
        Assert.Equal(2, registeredRules.Count);

        var r1 = registeredRules[0];
        Assert.Equal("Rule1", r1.Name);

        var r2 = registeredRules[1];
        Assert.Equal("Rule2", r2.Name);

        var indexed_rule1 = registeredRules[0];
        Assert.NotNull (rule1);
    }

    [Fact]
    public void GetRule_ByName()
    {
        var rr = new RightRule.RightRule().CreateSession();
        rr.RuleService.RegisterRule(rule1);
        rr.RuleService.RegisterRule(rule2);
        Assert.Equal(2, rr.RuleService.GetRegisteredRules().Count);

        List<ParsedRule>? registeredRules = rr.RuleService.GetRegisteredRules();
        Assert.NotNull(registeredRules);
        Assert.Equal(2, registeredRules.Count);

        string RuleName = "Rule1";
        ParsedRule? pr1 = rr.RuleService.GetRegisteredRule("Rule1_Context", RuleName);
        Assert.NotNull(pr1);
        Assert.Equal(RuleName, pr1.Name);
    }

    [Fact]
    public void GetRule_ByName2()
    {
        var rr = new RightRule.RightRule().CreateSession();
        rr.RuleService.RegisterRule(rule1);
        rr.RuleService.RegisterRule(rule2);
        Assert.Equal(2, rr.RuleService.GetRegisteredRules().Count);

        List<ParsedRule>? registeredRules = rr.RuleService.GetRegisteredRules();
        Assert.NotNull(registeredRules);
        Assert.Equal(2, registeredRules.Count);

        string RuleName = "RULE1";
        ParsedRule? pr1 = rr.RuleService.GetRegisteredRule("Rule1_Context", RuleName);
        Assert.NotNull(pr1);
        Assert.Equal(RuleName, pr1.Name.ToUpper());
    }

    [Fact]
    public void GetRule_ByNumber()
    {
        var rr = new RightRule.RightRule().CreateSession();
        rr.RuleService.RegisterRule(rule1);
        rr.RuleService.RegisterRule(rule2);
        Assert.Equal(2, rr.RuleService.GetRegisteredRules().Count);

        List<ParsedRule>? registeredRules = rr.RuleService.GetRegisteredRules();
        Assert.NotNull(registeredRules);
        Assert.Equal(2, registeredRules.Count);

        int RuleNumber = 1;
        ParsedRule? pr1 = rr.RuleService.GetRegisteredRule("Rule1_Context", RuleNumber);
        Assert.NotNull(pr1);
        Assert.Equal(RuleNumber, pr1.Number);
    }

    [Fact]
    public void DeregisterRule_ByNumber()
    {
        var rr = new RightRule.RightRule().CreateSession();
        rr.RuleService.RegisterRule(rule1);
        rr.RuleService.RegisterRule(rule2);
        Assert.Equal(2, rr.RuleService.GetRegisteredRules().Count);

        var done = rr.RuleService.DeRegisterRule(rule1.Context, rule1.Number);
        Assert.True(done);
        Assert.Equal(1, rr.RuleService.GetRegisteredRules().Count);
    }

    [Fact]
    public void DeregisterRule_ByName()
    {
        var rr = new RightRule.RightRule().CreateSession();
        rr.RuleService.RegisterRule(rule1);
        rr.RuleService.RegisterRule(rule2);
        Assert.Equal(2, rr.RuleService.GetRegisteredRules().Count);

        var done = rr.RuleService.DeRegisterRule(rule2.Context, rule2.Name);
        Assert.True(done);
        Assert.Equal(1, rr.RuleService.GetRegisteredRules().Count);
    }
}
