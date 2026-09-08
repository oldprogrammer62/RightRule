using BeSoft;
using RightRule.RegistrationServices.Rules;
using RightRule.RuleModels.RuleModels;

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
            FalseDecision = "NOT_DONE",
        };

        rule2 = new DefinedRule()
        {
            Number = 2,
            Name = "Rule2",
            Context = "Rule2_Context",
            Statement = "a.b = c.d",
            Decision = "DONE",
            FalseDecision = "NOT_DONE",
        };
    }

    [Fact]
    public void GetRule1()
    {
        IRightRule rr = new BeSoft.RightRule();
        rr.RegisterRule(rule1);
        rr.RegisterRule(rule2);
        Assert.Equal(2, rr.RegisteredRules().Count);

        var rules = rr.RegisteredRules("Rule1_Context");
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
        Assert.Equal(2, rr.RegisteredRules().Count);

        var rules = rr.RegisteredRules("Rule2_Context");
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
        Assert.Equal(2, rr.RegisteredRules().Count);

        IRegisteredRules? registeredRules = rr.RegisteredRules();
        Assert.NotNull(registeredRules);
        Assert.Equal(2, registeredRules.Count);

        var r1 = registeredRules.Rules[0];
        Assert.Equal("Rule1", r1.Name);

        var r2 = registeredRules.Rules[1];
        Assert.Equal("Rule2", r2.Name);
    }

    [Fact]
    public void GetRules_Indexed()
    {
        IRightRule rr = new BeSoft.RightRule();
        rr.RegisterRule(rule1);
        rr.RegisterRule(rule2);
        Assert.Equal(2, rr.RegisteredRules().Count);

        IRegisteredRules? registeredRules = rr.RegisteredRules();
        Assert.NotNull(registeredRules);
        Assert.Equal(2, registeredRules.Count);

        var r1 = registeredRules.Rules[0];
        Assert.Equal("Rule1", r1.Name);

        var r2 = registeredRules.Rules[1];
        Assert.Equal("Rule2", r2.Name);

        var indexed_rule1 = registeredRules[0];
        Assert.NotNull (rule1);
    }

    [Fact]
    public void GetRule_ByName()
    {
        IRightRule rr = new BeSoft.RightRule();
        rr.RegisterRule(rule1);
        rr.RegisterRule(rule2);
        Assert.Equal(2, rr.RegisteredRules().Count);

        IRegisteredRules? registeredRules = rr.RegisteredRules();
        Assert.NotNull(registeredRules);
        Assert.Equal(2, registeredRules.Count);

        string RuleName = "Rule1";
        ParsedRule? pr1 = rr.RegisteredRule(RuleName);
        Assert.NotNull(pr1);
        Assert.Equal(RuleName, pr1.Name);
    }

    [Fact]
    public void GetRule_ByName2()
    {
        IRightRule rr = new BeSoft.RightRule();
        rr.RegisterRule(rule1);
        rr.RegisterRule(rule2);
        Assert.Equal(2, rr.RegisteredRules().Count);

        IRegisteredRules? registeredRules = rr.RegisteredRules();
        Assert.NotNull(registeredRules);
        Assert.Equal(2, registeredRules.Count);

        string RuleName = "RULE1";
        ParsedRule? pr1 = rr.RegisteredRule(RuleName);
        Assert.NotNull(pr1);
        Assert.Equal(RuleName, pr1.Name.ToUpper());
    }

    [Fact]
    public void GetRule_ByNumber()
    {
        IRightRule rr = new BeSoft.RightRule();
        rr.RegisterRule(rule1);
        rr.RegisterRule(rule2);
        Assert.Equal(2, rr.RegisteredRules().Count);

        IRegisteredRules? registeredRules = rr.RegisteredRules();
        Assert.NotNull(registeredRules);
        Assert.Equal(2, registeredRules.Count);

        int RuleNumber = 2;
        ParsedRule? pr1 = rr.RegisteredRule(RuleNumber);
        Assert.NotNull(pr1);
        Assert.Equal(RuleNumber, pr1.Number);
    }

    [Fact]
    public void DeregisterRule_ByNumber()
    {
        IRightRule rr = new BeSoft.RightRule();
        rr.RegisterRule(rule1);
        rr.RegisterRule(rule2);
        Assert.Equal(2, rr.RegisteredRules().Count);

        var done = rr.DeRegisterRule(1);
        Assert.True(done);
        Assert.Equal(1, rr.RegisteredRules().Count);
    }

    [Fact]
    public void DeregisterRule_ByName()
    {
        IRightRule rr = new BeSoft.RightRule();
        rr.RegisterRule(rule1);
        rr.RegisterRule(rule2);
        Assert.Equal(2, rr.RegisteredRules().Count);

        var done = rr.DeRegisterRule("rule2");
        Assert.True(done);
        Assert.Equal(1, rr.RegisteredRules().Count);
    }
}
