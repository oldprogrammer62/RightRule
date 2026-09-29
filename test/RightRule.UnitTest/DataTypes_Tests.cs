using BeSoft;

namespace RightRuleTests;

public class DataTypes_Tests
{
    [Fact]
    public void Equals_Int()
    {
        IRightRule rr = new BeSoft.RightRule();

        int a = 100;
        var data1Reg = rr.RegisterData("a", a);

        var rule1Reg = rr.BuildRule()
            .Number(1)
            .Name("Rule1")
            .Context("Context")
            .Statement("a = 100")
            .Decision("SAME")
            .FalseDecision("DIFF")
            .Build()
            .Register(rr.RuleService);

        var result = rr.ApplyRule("Context");
        Assert.NotNull(result);
        Assert.Equal(result.Decision, rule1Reg?.RegisteredRule?.Decision);
    }

    [Fact]
    public void Equals_Dec()
    {
        IRightRule rr = new BeSoft.RightRule();

        double a = 100.001;
        var data1Reg = rr.RegisterData("a", a);

        var rule1Reg = rr.BuildRule()
            .Number(1)
            .Name("Rule1")
            .Context("Context")
            .Statement("a = 100.001")
            .Decision("SAME")
            .FalseDecision("DIFF")
            .Build()
            .Register(rr.RuleService);

        var result = rr.ApplyRule("Context");
        Assert.NotNull(result);
        Assert.Equal(result.Decision, rule1Reg?.RegisteredRule?.Decision);
    }

    [Fact]
    public void Equals_String()
    {
        IRightRule rr = new BeSoft.RightRule();

        string a = "100";
        var data1Reg = rr.RegisterData("a", a);

        var rule1Reg = rr.BuildRule()
            .Number(1)
            .Name("Rule1")
            .Context("Context")
            .Statement("a = \"100\"")
            .Decision("SAME")
            .FalseDecision("DIFF")
            .Build()
            .Register(rr.RuleService);

        var result = rr.ApplyRule("Context");
        Assert.NotNull(result);
        Assert.Equal(result.Decision, rule1Reg?.RegisteredRule?.Decision);
    }

    [Fact]
    public void Equals_Char()
    {
        IRightRule rr = new BeSoft.RightRule();

        char a = 'C';
        var data1Reg = rr.RegisterData("a", a);

        var rule1Reg = rr.BuildRule()
            .Number(1)
            .Name("Rule1")
            .Context("Context")
            .Statement("a = \'C\'")
            .Decision("SAME")
            .FalseDecision("DIFF")
            .Build()
            .Register(rr.RuleService);

        var result = rr.ApplyRule("Context");
        Assert.NotNull(result);
        Assert.Equal(result.Decision, rule1Reg?.RegisteredRule?.Decision);
    }

    [Fact]
    public void Equals_Boolean()
    {
        IRightRule rr = new BeSoft.RightRule();

        bool a = true;
        var data1Reg = rr.RegisterData("a", a);

        var rule1Reg = rr.BuildRule()
            .Number(1)
            .Name("Rule1")
            .Context("Context")
            .Statement("a = true")
            .Decision("SAME")
            .FalseDecision("DIFF")
            .Build()
            .Register(rr.RuleService);

        var result = rr.ApplyRule("Context");
        Assert.NotNull(result);
        Assert.Equal(result.Decision, rule1Reg?.RegisteredRule?.Decision);
    }

    [Fact]
    public void Equals_BooleanString()
    {
        IRightRule rr = new BeSoft.RightRule();

        bool a = true;
        var data1Reg = rr.RegisterData("a", a);

        var rule1Reg = rr.BuildRule()
            .Number(1)
            .Name("Rule1")
            .Context("Context")
            .Statement("a = \"true\"")
            .Decision("SAME")
            .FalseDecision("DIFF")
            .Build()
            .Register(rr.RuleService);

        var result = rr.ApplyRule("Context");
        Assert.NotNull(result);
        Assert.Equal(result.Decision, rule1Reg?.RegisteredRule?.Decision);
    }

    [Fact]
    public void Equals_DateTime()
    {
        IRightRule rr = new BeSoft.RightRule();

        DateTime a = new DateTime(1970, 01, 01, 12, 12, 12);
        var data1Reg = rr.RegisterData("a", a);

        var rule1Reg = rr.BuildRule()
            .Number(1)
            .Name("Rule1")
            .Context("Context")
            .Statement("a = \"01/01/1970 12:12:12\"")
            .Decision("SAME")
            .FalseDecision("DIFF")
            .Build()
            .Register(rr.RuleService);

        var result = rr.ApplyRule("Context");
        Assert.NotNull(result);
        Assert.Equal(result.Decision, rule1Reg?.RegisteredRule?.Decision);
    }

    [Fact]
    public void Equals_DateOnly()
    {
        IRightRule rr = new BeSoft.RightRule();

        DateOnly a = new DateOnly(1970, 01, 01);
        var data1Reg = rr.RegisterData("a", a);

        var rule1Reg = rr.BuildRule()
            .Number(1)
            .Name("Rule1")
            .Context("Context")
            .Statement("a = \"01/01/1970\"")
            .Decision("SAME")
            .FalseDecision("DIFF")
            .Build()
            .Register(rr.RuleService);

        var result = rr.ApplyRule("Context");
        Assert.NotNull(result);
        Assert.Equal(result.Decision, rule1Reg?.RegisteredRule?.Decision);
    }

    [Fact]
    public void Equals_TimeOnly()
    {
        IRightRule rr = new BeSoft.RightRule();

        TimeOnly a = new TimeOnly(12, 30, 30);
        var data1Reg = rr.RegisterData("a", a);

        var rule1Reg = rr.BuildRule()
            .Number(1)
            .Name("Rule1")
            .Context("Context")
            .Statement("a = \"12:30:30\"")
            .Decision("SAME")
            .FalseDecision("DIFF")
            .Build()
            .Register(rr.RuleService);

        var result = rr.ApplyRule("Context");
        Assert.NotNull(result);
        Assert.Equal(result.Decision, rule1Reg?.RegisteredRule?.Decision);
    }
}
