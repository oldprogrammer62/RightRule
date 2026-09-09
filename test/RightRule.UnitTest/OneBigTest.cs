using BeSoft;
using RightRule.RegistrationServices.RuleRegistration;

namespace RightRuleTests;

public class OneBigTest
{
    readonly string Context1 = "Context1";
    readonly string Context2 = "Context2";

    readonly string DefaultRule1Name = "DefaultRule1Name";
    readonly string DefaultRule1Decision = "DefaultRule1Decision";

    readonly string DefaultRule2Name = "DefaultRule2Name";
    readonly string DefaultRule2Decision = "DefaultRule2Decision";

    Dictionary<string, object> data1 = new Dictionary<string, object>()
    {
        { "Key1", 100 },
        { "Key2", 200 },
        { "Key3", 300 }
    };

    Dictionary<string, object> data2 = new Dictionary<string, object>()
    {
        { "Key1", "Mark" },
        { "Key2", "Jake" },
        { "Key3", "Lorraine" }
    };

    [Fact]
    public void ApplyRuleContext_True()
    {
        IRightRule rr = new BeSoft.RightRule();

        var data1Registration = rr.RegisterData("data1", data1);
        Assert.NotNull(data1Registration);
        Assert.True(data1Registration.Success);

        var data2Registration = rr.RegisterData("data2", data2);
        Assert.NotNull(data2Registration);
        Assert.True(data2Registration.Success);

        var rule1Defn = rr.BuildRule()
            .Number(1)
            .Name("Rule1Name")
            .Context(Context1)
            .Statement("data1.Key1 = 100 and data2.Key1 = 'Mark'")
            .Decision("Decision1")
            .FalseDecision("NotDecision1")
            .Build();
        var rule1Registration = rr.RegisterRule(rule1Defn);
        Assert.NotNull(rule1Registration);
        Assert.Equal(RegistrationStatus.SUCCESS, rule1Registration.Status);
        var rule1TS = rule1Registration.RegisteredRule.ToString();


        var result = rr.ApplyRules(Context1);
        Assert.NotNull(result);
        Assert.True(result.Result);
        Assert.Equal(result.Decision, rule1Defn.Decision);
    }

    [Fact]
    public void ApplyRuleContext_False_StdDefaultRule()
    {
        IRightRule rr = new BeSoft.RightRule();

        var data1Registration = rr.RegisterData("data1", data1);
        Assert.NotNull(data1Registration);
        Assert.True(data1Registration.Success);

        var data2Registration = rr.RegisterData("data2", data2);
        Assert.NotNull(data2Registration);
        Assert.True(data2Registration.Success);

        var rule1Defn = rr.BuildRule()
            .Number(1)
            .Name("Rule1Name")
            .Context(Context1)
            .Statement("data1.Key1 = 101 and data2.Key1 = 'Mark'")
            .Decision("Decision1")
            .FalseDecision("NotDecision1")
            .Build();
        var rule1Registration = rr.RegisterRule(rule1Defn);
        Assert.NotNull(rule1Registration);
        Assert.Equal(RegistrationStatus.SUCCESS, rule1Registration.Status);

        var result = rr.ApplyRules(Context1);
        Assert.NotNull(result);
        Assert.True(result.Result);
        Assert.Equal(rr.GetDefaultRule(Context1).Decision, result.Decision);
    }

    [Fact]
    public void ApplyRuleContext_False_NewDefaultRule()
    {
        IRightRule rr = new BeSoft.RightRule();

        var data1Registration = rr.RegisterData("data1", data1);
        Assert.NotNull(data1Registration);
        Assert.True(data1Registration.Success);

        var data2Registration = rr.RegisterData("data2", data2);
        Assert.NotNull(data2Registration);
        Assert.True(data2Registration.Success);

        var rule1Defn = rr.BuildRule()
            .Number(1)
            .Name("Rule1Name")
            .Context(Context1)
            .Statement("data1.Key1 = 101 and data2.Key1 = 'Mark'")
            .Decision("Decision1")
            .FalseDecision("NotDecision1")
            .Build();
        var rule1Registration = rr.RegisterRule(rule1Defn);
        Assert.NotNull(rule1Registration);
        Assert.Equal(RegistrationStatus.SUCCESS, rule1Registration.Status);

        var defaultRuleDefn = rr.BuildDefaultRule()
            .Name(DefaultRule1Name)
            .Context(Context1)
            .Decision(DefaultRule1Decision)
            .Build();
        var defaultRule1Registration = rr.RegisterDefaultRule(defaultRuleDefn);
        Assert.NotNull(defaultRule1Registration);
        Assert.Equal(RegistrationStatus.SUCCESS, defaultRule1Registration.Status);

        var result = rr.ApplyRules(Context1);
        Assert.NotNull(result);
        Assert.True(result.Result);
        Assert.Equal(rr.GetDefaultRule(Context1).Decision, result.Decision);
    }

    [Fact]
    public void ApplyRuleContext_False_DefaultRuleContext2()
    {
        IRightRule rr = new BeSoft.RightRule();

        var data1Registration = rr.RegisterData("data1", data1);
        Assert.NotNull(data1Registration);
        Assert.True(data1Registration.Success);

        var data2Registration = rr.RegisterData("data2", data2);
        Assert.NotNull(data2Registration);
        Assert.True(data2Registration.Success);

        var rule1Defn = rr.BuildRule()
            .Number(1)
            .Name("Rule1Name")
            .Context(Context1)
            .Statement("data1.Key1 = 101 and data2.Key1 = 'Mark'")
            .Decision("Decision1")
            .FalseDecision("NotDecision1")
            .Build();
        var rule1Registration = rr.RegisterRule(rule1Defn);
        Assert.NotNull(rule1Registration);
        Assert.Equal(RegistrationStatus.SUCCESS, rule1Registration.Status);

        var defaultRuleDefn = rr.BuildDefaultRule()
            .Name(DefaultRule1Name)
            .Context(Context1)
            .Decision(DefaultRule1Decision)
            .Build();
        var defaultRule1Registration = rr.RegisterDefaultRule(defaultRuleDefn);
        Assert.NotNull(defaultRule1Registration);
        Assert.Equal(RegistrationStatus.SUCCESS, defaultRule1Registration.Status);

        var result1 = rr.ApplyRules(Context1);
        Assert.NotNull(result1);
        Assert.True(result1.Result);
        Assert.Equal(rr.GetDefaultRule(Context1).Decision, result1.Decision);


        var rule2Defn = rr.BuildRule()
            .Number(1)
            .Name("Rule2Name")
            .Context(Context2)
            .Statement("data1.Key1 = 101 and data2.Key1 = 'Mark'")
            .Decision("Decision2")
            .FalseDecision("NotDecision2")
            .Build();
        var rule2Registration = rr.RegisterRule(rule2Defn);
        Assert.NotNull(rule2Registration);
        Assert.Equal(RegistrationStatus.SUCCESS, rule1Registration.Status);

        var result2 = rr.ApplyRules(Context2);
        Assert.NotNull(result2);
        Assert.True(result2.Result);
        Assert.Equal(rr.GetDefaultRule(Context2).Decision, result2.Decision);
    }

    [Fact]
    public void ApplyRuleContext_False_NewDefaultRuleContext2()
    {
        IRightRule rr = new BeSoft.RightRule();

        var data1Registration = rr.RegisterData("data1", data1);
        Assert.NotNull(data1Registration);
        Assert.True(data1Registration.Success);

        var data2Registration = rr.RegisterData("data2", data2);
        Assert.NotNull(data2Registration);
        Assert.True(data2Registration.Success);

        var rule1Defn = rr.BuildRule()
            .Number(1)
            .Name("Rule1Name")
            .Context(Context1)
            .Statement("data1.Key1 = 101 and data2.Key1 = 'Mark'")
            .Decision("Decision1")
            .FalseDecision("NotDecision1")
            .Build();
        var rule1Registration = rr.RegisterRule(rule1Defn);
        Assert.NotNull(rule1Registration);
        Assert.Equal(RegistrationStatus.SUCCESS, rule1Registration.Status);

        var defaultRuleDefn = rr.BuildDefaultRule()
            .Name(DefaultRule1Name)
            .Context(Context1)
            .Decision(DefaultRule1Decision)
            .DecisionData("Hello World Do Something Now1")
            .Build();
        var defaultRule1Registration = rr.RegisterDefaultRule(defaultRuleDefn);
        Assert.NotNull(defaultRule1Registration);
        Assert.Equal(RegistrationStatus.SUCCESS, defaultRule1Registration.Status);

        var result1 = rr.ApplyRules(Context1);
        Assert.NotNull(result1);
        Assert.True(result1.Result);
        Assert.Equal(rr.GetDefaultRule(Context1).Decision, result1.Decision);


        var rule2Defn = rr.BuildRule()
            .Number(1)
            .Name("Rule2Name")
            .Context(Context2)
            .Statement("data1.Key1 = 101 and data2.Key1 = 'Mark'")
            .Decision("Decision2")
            .FalseDecision("NotDecision2")
            .Build();
        var rule2Registration = rr.RegisterRule(rule2Defn);
        Assert.NotNull(rule2Registration);
        Assert.Equal(RegistrationStatus.SUCCESS, rule1Registration.Status);

        var defaultRule2Defn = rr.BuildDefaultRule()
            .Name(DefaultRule2Name)
            .Context(Context2)
            .Decision(DefaultRule2Decision)
            .DecisionData("Hello World Do Something Now2")
            .Build();
        var defaultRule2Registration = rr.RegisterDefaultRule(defaultRule2Defn);
        Assert.NotNull(defaultRule2Registration);
        Assert.Equal(RegistrationStatus.SUCCESS, defaultRule2Registration.Status);

        var result2 = rr.ApplyRules(Context2);
        Assert.NotNull(result2);
        Assert.True(result2.Result);
        Assert.Equal(rr.GetDefaultRule(Context2).Decision, result2.Decision);
    }
}
