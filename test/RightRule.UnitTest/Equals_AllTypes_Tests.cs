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

        RuleDefinition rule = new RuleDefinition()
        {
            Number = 1,
            Name = "Rule1",
            Context = "Context",
            Decision = "SAME",
            Statement = "intVal = intVal1 and intVal2 = 102 and intVal3 = 103 and intVal4 = 104 and intVal5 = 105"
        };
        var rrule = rr.RegisterRule(rule);
        Assert.True(rrule.Success);

        var result = rr.EvaluateFirstRule("Context");

        Assert.NotNull(result);
        Assert.True(result.Result);
        Assert.Equal(rule.Decision, result.Decision);
    }

    [Fact]
    public void Test_Dec()
    {
        BeSoft.IRightRule rr = new BeSoft.RightRule();
        rr.RegisterData("testVal", decVal);

        decimal localVal = 100.01M;
        rr.RegisterData("localVal", localVal);

        RuleDefinition rule = new RuleDefinition()
        {
            Number = 1,
            Name = "Rule1",
            Context = "Context",
            Decision = "SAME",
            Statement = "localVal.testVal = testVal.decVal"
        };
        rr.RegisterRule(rule);

        var result = rr.EvaluateFirstRule("Context");

        Assert.NotNull(result);
        Assert.True(result.Result);
        Assert.Equal(rule.Decision, result.Decision);
    }
}
