using BeSoft;

namespace RightRuleTests;

public class DataTypes_Test
{
    [Fact]
    public void Dictionary_Test()
    {
        IRightRule rr = new BeSoft.RightRule();

        Dictionary<string, object> data = new Dictionary<string, object>()
        {
            { "Value", 100 }
        };
        rr.RegisterData("Data", data);

        var rule = rr.BuildRuleDefinition()
            .Number(1)
            .Name("Name")
            .Context("Context")
            .Statement("Data.Value = 100")
            .Decision("DONE")
            .WithDataCheck(true)
            .Build();
        var prule = rr.RegisterRule(rule);

        var result = rr.EvaluateFirstMatch("Context");
        Assert.NotNull(result);
        Assert.Equal(rule.Decision, result.Decision);
    }

    [Fact]
    public void Dictionary_Tuple()
    {
        IRightRule rr = new BeSoft.RightRule();

        (string Name, int Number) data = (Name: "Value", Number: 100);
        (string Name, int Number) data22 = (Name: "Value", Number: 100);

        rr.RegisterData("Value", data);
        rr.RegisterData("Value22", data22);

        var rule = rr.BuildRuleDefinition()
            .Number(1)
            .Name("Name")
            .Context("Context")
            .Statement("Value22 = 100")
            .Decision("DONE")
            .WithDataCheck(true)
            .Build();
        var prule = rr.RegisterRule(rule);

        var result = rr.EvaluateFirstMatch("Context");
        Assert.NotNull(result);
        Assert.Equal(rule.Decision, result.Decision);
    }

    [Fact]
    public void Dictionary_Variable()
    {
        IRightRule rr = new BeSoft.RightRule();

        int Value = 100;

        rr.RegisterData("Value", Value);

        var rule = rr.BuildRuleDefinition()
            .Number(1)
            .Name("Name")
            .Context("Context")
            .Statement("Value = 100")
            .Decision("DONE")
            .WithDataCheck(true)
            .Build();
        var prule = rr.RegisterRule(rule);

        var result = rr.EvaluateFirstMatch("Context");
        Assert.NotNull(result);
        Assert.Equal(rule.Decision, result.Decision);
    }
}
