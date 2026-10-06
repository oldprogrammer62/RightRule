using RightRule;
using RightRule.RuleServices.RuleModels;
using RightRule.RuleServices.RuleRegistration;

namespace RightRuleTests;

public class Equals_AllTypes_Tests
{
    int intVal = 101;
    decimal decVal = 100.01M;
    double dblVal = 100.02;
    long lngVal = 101;
    Single sinVal = 100;
    float fltVal = 100.09F;

    string strVal = "Hello";
    char chrVal = 'C';

    bool boolVal = true;
    string boolStr = "true";

    static DateOnly dtonlyVal = new DateOnly(1970, 01, 01);
    static TimeOnly tmonlyVal = new TimeOnly(12, 13, 14);
    DateTime dttimeVal = new DateTime(dtonlyVal, tmonlyVal);

    [Fact]
    public void Test_Int()
    {
        IRightRule rr = new RightRule.RightRule();

        rr.RegisterData(nameof(intVal), intVal);

        int intVal1 = 101;
        rr.RegisterData(nameof(intVal1), intVal1);

        int intVal2 = 102;
        rr.RegisterData(nameof(intVal2), intVal2);

        int intVal3 = 103;
        rr.RegisterData(nameof(intVal3), intVal3);

        int intVal4 = 104;
        rr.RegisterData(nameof(intVal4), intVal4);

        int intVal5 = 105;
        rr.RegisterData(nameof(intVal5), intVal5);

        DefinedRule ruleDef = new DefinedRule()
        {
            Number = 1,
            Name = "Rule1",
            Context = "Context",
            Statement = "intVal = intVal1 and intVal2 = 102 and intVal3 = 103 and intVal4 = 104 and intVal5 = 105",
            Decision = "SAME",
            FalseDecision = "DIFFERENT"
        };

        var rrule = rr.RegisterRule(ruleDef);
        Assert.Equal(RegistrationStatusEnum.SUCCESS, rrule.Status);

        var result = rr.ApplyRule("Context");

        Assert.NotNull(result);
        Assert.Equal(rrule.RegisteredRule.Decision, result.Decision);
    }

    [Fact]
    public void Test_Dec()
    {
        IRightRule rr = new RightRule.RightRule();
        rr.RegisterData(nameof(decVal), decVal);

        decimal localVal = 100.01M;
        rr.RegisterData(nameof(localVal), localVal);

        DefinedRule rule = new DefinedRule()
        {
            Number = 1,
            Name = "Rule1",
            Context = "Context",
            Statement = "localVal.localVal = decVal.decVal",
            Decision = "SAME",
            FalseDecision = "DIFFERENT"
        };
        rr.RegisterRule(rule);

        var result = rr.ApplyRule("Context");

        Assert.NotNull(result);
        Assert.Equal(rule.Decision, result.Decision);
    }

    [Fact]
    public void Test_Dec2()
    {
        IRightRule rr = new RightRule.RightRule();
        rr.RegisterData(nameof(decVal), decVal);

        decimal localVal = 100.01M;
        rr.RegisterData(nameof(localVal), localVal);

        var rule = rr.BuildRule()
            .Number(1)
            .Name("NameOfRule")
            .Context("Context")
            .Statement("decVal.decVal = localVal.localVal")
            .Decision("GOOD")
            .FalseDecision("DIFFERENT")
            .WithDataCheck(false)
            .WithDiagnostics(true)
            .Build();
            
        var prule = rr.RegisterRule(rule);

        var result = rr.ApplyRule("Context");

        Assert.NotNull(result);
        Assert.Equal(rule.Decision, result.Decision);
    }

    [Fact]
    public void Test_Dbl()
    {
        IRightRule rr = new RightRule.RightRule();
        rr.RegisterData(nameof(dblVal), dblVal);

        double localVal = dblVal;
        var registeredDat = rr.RegisterData(nameof(localVal), localVal);

        DefinedRule rule = new DefinedRule()
        {
            Number = 1,
            Name = "Rule1",
            Context = "Context",
            Statement = "localVal = dblVal",
            Decision = "SAME",
            FalseDecision = "DIFFERENT"
        };
        var registeredRule = rr.RegisterRule(rule);

        var result = rr.ApplyRule("Context");

        Assert.NotNull(result);
        Assert.Equal(rule.Decision, result.Decision);
    }

    [Fact]
    public void Test_Lng()
    {
        IRightRule rr = new RightRule.RightRule();
        rr.RegisterData(nameof(lngVal), lngVal);

        var localVal = lngVal;
        var registeredData = rr.RegisterData(nameof(localVal), localVal);

        DefinedRule rule = new DefinedRule()
        {
            Number = 1,
            Name = "Rule1",
            Context = "Context",
            Statement = "localVal.localVal = lngVal",
            Decision = "SAME",
            FalseDecision = "DIFFERENT"
        };
        var registeredRule = rr.RegisterRule(rule);

        var result = rr.ApplyRule("Context");

        Assert.NotNull(result);
        Assert.Equal(rule.Decision, result.Decision);
    }

    [Fact]
    public void Test_Single()
    {
        IRightRule rr = new RightRule.RightRule();
        rr.RegisterData(nameof(sinVal), sinVal);

        Single localVal = sinVal;
        var registeredData = rr.RegisterData(nameof(localVal), localVal);

        DefinedRule rule = new DefinedRule()
        {
            Number = 1,
            Name = "Rule1",
            Context = "Context",
            Statement = "localVal = sinVal.sinVal",
            Decision = "SAME",
            FalseDecision = "DIFFERENT"
        };
        var registeredRule = rr.RegisterRule(rule);

        var result = rr.ApplyRule("Context");

        Assert.NotNull(result);
        Assert.Equal(rule.Decision, result.Decision);
    }

    [Fact]
    public void Test_Float_Dict()
    {
        IRightRule rr = new RightRule.RightRule();
        rr.RegisterData(nameof(fltVal), fltVal);

        Dictionary<string, object> data = new Dictionary<string, object>()
        {
            { "entry", fltVal }
        };
        var registeredData = rr.RegisterData("localData", data);

        DefinedRule rule = new DefinedRule()
        {
            Number = 1,
            Name = "Rule1",
            Context = "Context",
            Statement = "localData.entry = fltVal",
            Decision = "SAME",
            FalseDecision = "DIFFERENT"
        };
        var registeredRule = rr.RegisterRule(rule);

        var result = rr.ApplyRule("Context");

        Assert.NotNull(result);
        Assert.Equal(rule.Decision, result.Decision);
    }

    [Fact]
    public void Test_String()
    {
        IRightRule rr = new RightRule.RightRule();
        rr.RegisterData(nameof(strVal), strVal);

        string localVal = strVal;
        var registeredData = rr.RegisterData(nameof(localVal), localVal);

        DefinedRule rule = new DefinedRule()
        {
            Number = 1,
            Name = "Rule1",
            Context = "Context",
            Statement = "localVal = strVal",
            Decision = "SAME",
            FalseDecision = "DIFFERENT"
        };
        var registeredRule = rr.RegisterRule(rule);

        var result = rr.ApplyRule("Context");

        Assert.NotNull(result);
        Assert.Equal(rule.Decision, result.Decision);
    }

    [Fact]
    public void Test_Char()
    {
        IRightRule rr = new RightRule.RightRule();
        rr.RegisterData(nameof(chrVal), chrVal);

        char localVal = chrVal;
        var registeredData = rr.RegisterData(nameof(localVal), localVal);

        DefinedRule rule = new DefinedRule()
        {
            Number = 1,
            Name = "Rule1",
            Context = "Context",
            Statement = "localVal = chrVal",
            Decision = "SAME",
            FalseDecision = "DIFFERENT"
        };
        var registeredRule = rr.RegisterRule(rule);

        var result = rr.ApplyRule("Context");

        Assert.NotNull(result);
        Assert.Equal(rule.Decision, result.Decision);
    }

    [Fact]
    public void Test_Bool_Bool()
    {
        IRightRule rr = new RightRule.RightRule();
        rr.RegisterData(nameof(boolVal), boolVal);

        bool localVal = boolVal;
        var registeredData = rr.RegisterData(nameof(localVal), localVal);

        DefinedRule rule = new DefinedRule()
        {
            Number = 1,
            Name = "Rule1",
            Context = "Context",
            Statement = "localVal = boolVal",
            Decision = "SAME",
            FalseDecision = "DIFFERENT"
        };
        var registeredRule = rr.RegisterRule(rule);

        var result = rr.ApplyRule("Context");

        Assert.NotNull(result);
        Assert.Equal(rule.Decision, result.Decision);
    }

    [Fact]
    public void Test_Bool_String()
    {
        IRightRule rr = new RightRule.RightRule();
        rr.RegisterData(nameof(boolStr), boolStr);

        string localVal = boolStr;
        var registeredData = rr.RegisterData(nameof(localVal), localVal);

        DefinedRule rule = new DefinedRule()
        {
            Number = 1,
            Name = "Rule1",
            Context = "Context",
            Statement = "localVal = boolStr",
            Decision = "SAME",
            FalseDecision = "DIFFERENT"
        };
        var registeredRule = rr.RegisterRule(rule);

        var result = rr.ApplyRule("Context");

        Assert.NotNull(result);
        Assert.Equal(rule.Decision, result.Decision);
    }

    [Fact]
    public void Test_DateOnly()
    {
        IRightRule rr = new RightRule.RightRule();
        rr.RegisterData(nameof(dtonlyVal), dtonlyVal);

        DateOnly localVal = dtonlyVal;
        var registeredData = rr.RegisterData(nameof(localVal), localVal);

        DefinedRule rule = new DefinedRule()
        {
            Number = 1,
            Name = "Rule1",
            Context = "Context",
            Decision = "SAME",
            FalseDecision = "DIFFERENT",
            Statement = "localVal = dtonlyVal"
        };
        var registeredRule = rr.RegisterRule(rule);

        var result = rr.ApplyRule("Context");

        Assert.NotNull(result);
        Assert.Equal(rule.Decision, result.Decision);
    }

    [Fact]
    public void Test_TimeOnly()
    {
        IRightRule rr = new RightRule.RightRule();
        rr.RegisterData(nameof(tmonlyVal), tmonlyVal);

        TimeOnly localVal = tmonlyVal;
        var registeredData = rr.RegisterData(nameof(localVal), localVal);

        DefinedRule rule = new DefinedRule()
        {
            Number = 1,
            Name = "Rule1",
            Context = "Context",
            Statement = "localVal = tmonlyVal",
            Decision = "SAME",
            FalseDecision = "DIFFERENT"
        };
        var registeredRule = rr.RegisterRule(rule);

        var result = rr.ApplyRule("Context");

        Assert.NotNull(result);
        Assert.Equal(rule.Decision, result.Decision);
    }

    [Fact]
    public void Test_DateTime()
    {
        IRightRule rr = new RightRule.RightRule();
        rr.RegisterData(nameof(dttimeVal), dttimeVal);

        DateTime localVal = dttimeVal;
        var registeredData = rr.RegisterData(nameof(localVal), localVal);

        DefinedRule rule = new DefinedRule()
        {
            Number = 1,
            Name = "Rule1",
            Context = "Context",
            Statement = "localVal = dttimeVal",
            Decision = "SAME",
            FalseDecision = "DIFFERENT"
        };
        var registeredRule = rr.RegisterRule(rule);

        var result = rr.ApplyRule("Context");

        Assert.NotNull(result);
        Assert.Equal(rule.Decision, result.Decision);
    }
}
