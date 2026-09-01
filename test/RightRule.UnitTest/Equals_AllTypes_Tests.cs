using RightRule.RuleModels;

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
        BeSoft.IRightRule rr = new BeSoft.RightRule();

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
        Assert.True(rrule.IsSuccess);

        var result = rr.EvaluateFirstMatch("Context");

        Assert.NotNull(result);
        Assert.Equal(rrule.RegisteredRule.RuleDefinition.Decision, result.Decision);
    }

    [Fact]
    public void Test_Dec()
    {
        BeSoft.IRightRule rr = new BeSoft.RightRule();
        rr.RegisterData("testVal", decVal);

        decimal localVal = 100.01M;
        rr.RegisterData("localVal", localVal);

        DefinedRule rule = new DefinedRule()
        {
            Number = 1,
            Name = "Rule1",
            Context = "Context",
            Statement = "localVal.testVal = testVal.decVal",
            Decision = "SAME",
            FalseDecision = "DIFFERENT"
        };
        rr.RegisterRule(rule);

        var result = rr.EvaluateFirstMatch("Context");

        Assert.NotNull(result);
        Assert.Equal(rule.Decision, result.Decision);
    }

    [Fact]
    public void Test_Dec2()
    {
        BeSoft.IRightRule rr = new BeSoft.RightRule();
        rr.RegisterData("testVal", decVal);

        decimal localVal = 100.01M;
        rr.RegisterData("localVal", localVal);

        var rule = rr.BuildRuleDefinition()
            .Number(1)
            .Name("NameOfRule")
            .Context("Context")
            .Statement("testVal = localVal")
            .Decision("GOOD")
            .FalseDecision("DIFFERENT")
            .WithDataCheck(true)
            .WithDiagnostics(true)
            .Build();
            
        var prule = rr.RegisterRule(rule);

        var result = rr.EvaluateFirstMatch("Context");

        Assert.NotNull(result);
        Assert.Equal(rule.Decision, result.Decision);
    }

    [Fact]
    public void Test_Dbl()
    {
        BeSoft.IRightRule rr = new BeSoft.RightRule();
        rr.RegisterData("testVal", dblVal);

        double localVal = dblVal;
        var registeredDat = rr.RegisterData("localVal", localVal);

        DefinedRule rule = new DefinedRule()
        {
            Number = 1,
            Name = "Rule1",
            Context = "Context",
            Statement = "localVal.testVal = testVal.dblVal",
            Decision = "SAME",
            FalseDecision = "DIFFERENT"
        };
        var registeredRule = rr.RegisterRule(rule);

        var result = rr.EvaluateFirstMatch("Context");

        Assert.NotNull(result);
        Assert.Equal(rule.Decision, result.Decision);
    }

    [Fact]
    public void Test_Lng()
    {
        BeSoft.IRightRule rr = new BeSoft.RightRule();
        rr.RegisterData("testVal", lngVal);

        var localVal = lngVal;
        var registeredData = rr.RegisterData("localVal", localVal);

        DefinedRule rule = new DefinedRule()
        {
            Number = 1,
            Name = "Rule1",
            Context = "Context",
            Statement = "localVal.testVal = testVal.lngVal",
            Decision = "SAME",
            FalseDecision = "DIFFERENT"
        };
        var registeredRule = rr.RegisterRule(rule);

        var result = rr.EvaluateFirstMatch("Context");

        Assert.NotNull(result);
        Assert.Equal(rule.Decision, result.Decision);
    }

    [Fact]
    public void Test_Sin()
    {
        BeSoft.IRightRule rr = new BeSoft.RightRule();
        rr.RegisterData("testVal", sinVal);

        Single localVal = sinVal;
        var registeredData = rr.RegisterData("localData", localVal);

        DefinedRule rule = new DefinedRule()
        {
            Number = 1,
            Name = "Rule1",
            Context = "Context",
            Statement = "localData = testVal",
            Decision = "SAME",
            FalseDecision = "DIFFERENT"
        };
        var registeredRule = rr.RegisterRule(rule);

        var result = rr.EvaluateFirstMatch("Context");

        Assert.NotNull(result);
        Assert.Equal(rule.Decision, result.Decision);
    }

    [Fact]
    public void Test_Float_Dict()
    {
        BeSoft.IRightRule rr = new BeSoft.RightRule();
        rr.RegisterData("testVal", fltVal);

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
            Statement = "localData.entry = testVal",
            Decision = "SAME",
            FalseDecision = "DIFFERENT"
        };
        var registeredRule = rr.RegisterRule(rule);

        var result = rr.EvaluateFirstMatch("Context");

        Assert.NotNull(result);
        Assert.Equal(rule.Decision, result.Decision);
    }

    [Fact]
    public void Test_String()
    {
        BeSoft.IRightRule rr = new BeSoft.RightRule();
        rr.RegisterData("testVal", strVal);

        string localVal = strVal;
        var registeredData = rr.RegisterData("localData", localVal);

        DefinedRule rule = new DefinedRule()
        {
            Number = 1,
            Name = "Rule1",
            Context = "Context",
            Statement = "localData = testVal",
            Decision = "SAME",
            FalseDecision = "DIFFERENT"
        };
        var registeredRule = rr.RegisterRule(rule);

        var result = rr.EvaluateFirstMatch("Context");

        Assert.NotNull(result);
        Assert.Equal(rule.Decision, result.Decision);
    }

    [Fact]
    public void Test_Char()
    {
        BeSoft.IRightRule rr = new BeSoft.RightRule();
        rr.RegisterData("testVal", chrVal);

        char localVal = chrVal;
        var registeredData = rr.RegisterData("localData", localVal);

        DefinedRule rule = new DefinedRule()
        {
            Number = 1,
            Name = "Rule1",
            Context = "Context",
            Statement = "localData = testVal",
            Decision = "SAME",
            FalseDecision = "DIFFERENT"
        };
        var registeredRule = rr.RegisterRule(rule);

        var result = rr.EvaluateFirstMatch("Context");

        Assert.NotNull(result);
        Assert.Equal(rule.Decision, result.Decision);
    }

    [Fact]
    public void Test_Bool_Bool()
    {
        BeSoft.IRightRule rr = new BeSoft.RightRule();
        rr.RegisterData("testVal", boolVal);

        bool localVal = boolVal;
        var registeredData = rr.RegisterData("localData", localVal);

        DefinedRule rule = new DefinedRule()
        {
            Number = 1,
            Name = "Rule1",
            Context = "Context",
            Statement = "localData = testVal",
            Decision = "SAME",
            FalseDecision = "DIFFERENT"
        };
        var registeredRule = rr.RegisterRule(rule);

        var result = rr.EvaluateFirstMatch("Context");

        Assert.NotNull(result);
        Assert.Equal(rule.Decision, result.Decision);
    }

    [Fact]
    public void Test_Bool_String()
    {
        BeSoft.IRightRule rr = new BeSoft.RightRule();
        rr.RegisterData("testVal", boolStr);

        string localVal = boolStr;
        var registeredData = rr.RegisterData("localData", localVal);

        DefinedRule rule = new DefinedRule()
        {
            Number = 1,
            Name = "Rule1",
            Context = "Context",
            Statement = "localData = testVal",
            Decision = "SAME",
            FalseDecision = "DIFFERENT"
        };
        var registeredRule = rr.RegisterRule(rule);

        var result = rr.EvaluateFirstMatch("Context");

        Assert.NotNull(result);
        Assert.Equal(rule.Decision, result.Decision);
    }

    [Fact]
    public void Test_DateOnly()
    {
        BeSoft.IRightRule rr = new BeSoft.RightRule();
        rr.RegisterData("testVal", dtonlyVal);

        DateOnly localVal = dtonlyVal;
        var registeredData = rr.RegisterData("localData", localVal);

        DefinedRule rule = new DefinedRule()
        {
            Number = 1,
            Name = "Rule1",
            Context = "Context",
            Decision = "SAME",
            FalseDecision = "DIFFERENT",
            Statement = "localData = testVal"
        };
        var registeredRule = rr.RegisterRule(rule);

        var result = rr.EvaluateFirstMatch("Context");

        Assert.NotNull(result);
        Assert.Equal(rule.Decision, result.Decision);
    }

    [Fact]
    public void Test_TimeOnly()
    {
        BeSoft.IRightRule rr = new BeSoft.RightRule();
        rr.RegisterData("testVal", tmonlyVal);

        TimeOnly localVal = tmonlyVal;
        var registeredData = rr.RegisterData("localData", localVal);

        DefinedRule rule = new DefinedRule()
        {
            Number = 1,
            Name = "Rule1",
            Context = "Context",
            Statement = "localData = testVal",
            Decision = "SAME",
            FalseDecision = "DIFFERENT"
        };
        var registeredRule = rr.RegisterRule(rule);

        var result = rr.EvaluateFirstMatch("Context");

        Assert.NotNull(result);
        Assert.Equal(rule.Decision, result.Decision);
    }

    [Fact]
    public void Test_DateTime()
    {
        BeSoft.IRightRule rr = new BeSoft.RightRule();
        rr.RegisterData("testVal", dttimeVal);

        DateTime localVal = dttimeVal;
        var registeredData = rr.RegisterData("localData", localVal);

        DefinedRule rule = new DefinedRule()
        {
            Number = 1,
            Name = "Rule1",
            Context = "Context",
            Statement = "localData = testVal",
            Decision = "SAME",
            FalseDecision = "DIFFERENT"
        };
        var registeredRule = rr.RegisterRule(rule);

        var result = rr.EvaluateFirstMatch("Context");

        Assert.NotNull(result);
        Assert.Equal(rule.Decision, result.Decision);
    }
}
