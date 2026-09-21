using BeSoft;
using RightRule.RightRuleService;

namespace RightRuleTests;

public class RuleDeregistrationTests
{
    [Fact]
    public void DeregisterByContextAndNumber()
    {
        IRightRule rr = new BeSoft.RightRule();

        string Context = "Context";
        int RuleNumber1 = 1;
        string RuleName1 = "Rule1";

        var rule1Def = rr.BuildRule()
            .Number(RuleNumber1)
            .Name(RuleName1)
            .Context(Context)
            .Statement("10 = 10")
            .Decision("Good")
            .FalseDecision("Bad")
            .Build();

        var rule1reg = rr.RegisterRule(rule1Def);
        Assert.NotNull(rule1reg);
        Assert.Single(rr.GetRegisteredRules());

        var rule1Dereg = rr.DeRegisterRule(Context, RuleNumber1);
        Assert.True(rule1Dereg);
        Assert.Empty(rr.GetRegisteredRules());
    }

    [Fact]
    public void DeregisterByContextAndName()
    {
        IRightRule rr = new BeSoft.RightRule();

        string Context = "Context";
        int RuleNumber1 = 1;
        string RuleName1 = "Rule1";

        var rule1Def = rr.BuildRule()
            .Number(RuleNumber1)
            .Name(RuleName1)
            .Context(Context)
            .Statement("10 = 10")
            .Decision("Good")
            .FalseDecision("Bad")
            .Build();

        var rule1reg = rr.RegisterRule(rule1Def);
        Assert.NotNull(rule1reg);
        Assert.Single(rr.GetRegisteredRules());

        var rule1Dereg = rr.DeRegisterRule(Context, RuleName1);
        Assert.True(rule1Dereg);
        Assert.Empty(rr.GetRegisteredRules());
    }

    [Fact]
    public void DeregisterByContextAndNumber2()
    {
        IRightRule rr = new BeSoft.RightRule();

        string Context = "Context";
        int RuleNumber1 = 1;
        string RuleName1 = "Rule1";

        int RuleNumber2 = 2;
        string RuleName2 = "Rule2";

        var rule1Def = rr.BuildRule()
            .Number(RuleNumber1)
            .Name(RuleName1)
            .Context(Context)
            .Statement("10 = 10")
            .Decision("Good")
            .FalseDecision("Bad")
            .Build();
        var rule1reg = rr.RegisterRule(rule1Def);

        Assert.NotNull(rule1reg);
        Assert.Single(rr.GetRegisteredRules());

        var rule2Def = rr.BuildRule()
            .Number(RuleNumber2)
            .Name(RuleName2)
            .Context(Context)
            .Statement("10 = 10")
            .Decision("Good")
            .FalseDecision("Bad")
            .Build();
        var rule2reg = rr.RegisterRule(rule2Def);

        Assert.NotNull(rule2reg);
        Assert.Equal(2, rr.GetRegisteredRules().Count);

        // Deregister first rule
        var rule1Dereg = rr.DeRegisterRule(Context, RuleNumber1);
        Assert.True(rule1Dereg);
        Assert.Single(rr.GetRegisteredRules());
    }

    [Fact]
    public void DeregisterByContextAndName2()
    {
        IRightRule rr = new BeSoft.RightRule();

        string Context = "Context";
        int RuleNumber1 = 1;
        string RuleName1 = "Rule1";

        int RuleNumber2 = 2;
        string RuleName2 = "Rule2";

        var rule1Def = rr.BuildRule()
            .Number(RuleNumber1)
            .Name(RuleName1)
            .Context(Context)
            .Statement("10 = 10")
            .Decision("Good")
            .FalseDecision("Bad")
            .Build();
        var rule1reg = rr.RegisterRule(rule1Def);

        Assert.NotNull(rule1reg);
        Assert.Single(rr.GetRegisteredRules());

        var rule2Def = rr.BuildRule()
            .Number(RuleNumber2)
            .Name(RuleName2)
            .Context(Context)
            .Statement("10 = 10")
            .Decision("Good")
            .FalseDecision("Bad")
            .Build();
        var rule2reg = rr.RegisterRule(rule2Def);

        Assert.NotNull(rule2reg);
        Assert.Equal(2, rr.GetRegisteredRules().Count);

        // Deregister first rule
        var rule1Dereg = rr.DeRegisterRule(Context, RuleName1);
        Assert.True(rule1Dereg);
        Assert.Single(rr.GetRegisteredRules());
    }

    [Fact]
    public void DeregisterByDiffContextAndName()
    {
        IRightRule rr = new BeSoft.RightRule();

        string Context1 = "Context1";
        int RuleNumber1 = 1;
        string RuleName1 = "Rule1";

        string Context2 = "Context2";
        int RuleNumber2 = 1;
        string RuleName2 = "Rule2";

        var rule1Def = rr.BuildRule()
            .Number(RuleNumber1)
            .Name(RuleName1)
            .Context(Context1)
            .Statement("10 = 10")
            .Decision("Good")
            .FalseDecision("Bad")
            .Build();
        var rule1reg = rr.RegisterRule(rule1Def);

        Assert.NotNull(rule1reg);
        Assert.Single(rr.GetRegisteredRules());

        var rule2Def = rr.BuildRule()
            .Number(RuleNumber2)
            .Name(RuleName2)
            .Context(Context2)
            .Statement("10 = 10")
            .Decision("Good")
            .FalseDecision("Bad")
            .Build();
        var rule2reg = rr.RegisterRule(rule2Def);

        Assert.NotNull(rule2reg);
        Assert.Equal(2, rr.GetRegisteredRules().Count);

        // Deregister first rule
        var rule1Dereg = rr.DeRegisterRule(Context1, RuleName1);
        Assert.True(rule1Dereg);
        Assert.Single(rr.GetRegisteredRules());
    }


    [Fact]
    public void DeregisterByDiffContextAndNumber()
    {
        IRightRule rr = new BeSoft.RightRule();

        string Context1 = "Context1";
        int RuleNumber1 = 1;
        string RuleName1 = "Rule1";

        string Context2 = "Context2";
        int RuleNumber2 = 1;
        string RuleName2 = "Rule2";

        var rule1Def = rr.BuildRule()
            .Number(RuleNumber1)
            .Name(RuleName1)
            .Context(Context1)
            .Statement("10 = 10")
            .Decision("Good")
            .FalseDecision("Bad")
            .Build();
        var rule1reg = rr.RegisterRule(rule1Def);

        Assert.NotNull(rule1reg);
        Assert.Single(rr.GetRegisteredRules());

        var rule2Def = rr.BuildRule()
            .Number(RuleNumber2)
            .Name(RuleName2)
            .Context(Context2)
            .Statement("10 = 10")
            .Decision("Good")
            .FalseDecision("Bad")
            .Build();
        var rule2reg = rr.RegisterRule(rule2Def);

        Assert.NotNull(rule2reg);
        Assert.Equal(2, rr.GetRegisteredRules().Count);

        // Deregister first rule
        var ruleDereg = rr.DeRegisterRule(Context2, RuleNumber2);
        Assert.True(ruleDereg);
        Assert.Single(rr.GetRegisteredRules());
    }
}
