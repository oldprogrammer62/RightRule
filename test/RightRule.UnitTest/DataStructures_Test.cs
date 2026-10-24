using RightRule;

namespace RightRuleTests;

public class DataStructures_Test
{
    [Fact]
    public void Dictionary_Test()
    {
        IRightRule rr = new RightRule.RightRule();

        Dictionary<string, object> data = new Dictionary<string, object>()
        {
            { "Value", 100 }
        };
        rr.RegisterData("Data", data);

        var rule = rr.BuildRule()
            .Number(1)
            .Name("Name")
            .Context("Context")
            .Statement("Data.Value = 100")
            .TrueDecision("DONE")
            .FalseDecision("NOT DONE")
            .Build();
        var prule = rr.RegisterRule(rule);

        var result = rr.ApplyRule("Context");
        Assert.NotNull(result);
        Assert.Equal(rule.TrueDecision, result.Decision);
    }

    [Fact]
    public void Dictionary_Tuple()
    {
        IRightRule rr = new RightRule.RightRule();

        rr.RegisterData("Data1", 9000);

        (string Name, int Number) data2 = (Name: "Value", Number: 100);
        rr.RegisterData("Data2", data2);

        (string Name, int Number) data3 = (Name: "Value", Number: 200);
        rr.RegisterData("Data3", data3);

        var rule = rr.BuildRule()
            .Number(1)
            .Name("Name")
            .Context("Context")
            .Statement("Data1 = 9000 and Data2.Value = 100 and Data3.Value = 200")
            .TrueDecision("DONE")
            .FalseDecision("NOT DONE")
            .Build();
        var prule = rr.RegisterRule(rule);

        var result = rr.ApplyRule("Context");
        Assert.NotNull(result);
        Assert.Equal(rule.TrueDecision, result.Decision);
    }

    [Fact]
    public void Dictionary_ListOfTuple()
    {
        IRightRule rr = new RightRule.RightRule();

        rr.RegisterData("Data1", 9000);

        List<(string Name, int Number)> data2 = new()
        {
            (Name: "Value", Number: 1001),
            (Name: "Value1", Number: 1002),
            (Name: "Value2", Number: 1003),
            (Name: "Value3", Number: 1004),
            (Name: "Value4", Number: 1005),
        };

        rr.RegisterData("Data2", data2);

        (string Name, int Number) data3 = (Name: "Value", Number: 200);
        rr.RegisterData("Data3", data3);

        var rule = rr.BuildRule()
            .Number(1)
            .Name("Name")
            .Context("Context")
            .Statement("Data1 = 9000 and Data2.Value3 = 1004 and Data3.Value = 200")
            .TrueDecision("DONE")
            .FalseDecision("NOT DONE")
            .Build()
            .Register(rr.RuleService);

        var result = rr.ApplyRule("Context");
        Assert.NotNull(result);
        Assert.Equal(rule?.RegisteredRule?.TrueDecision, result.Decision);
    }

    [Fact]
    public void Dictionary_Variable()
    {
        IRightRule rr = new RightRule.RightRule();

        int Value = 100;

        rr.RegisterData(nameof(Value), Value);

        var rule = rr.BuildRule()
            .Number(1)
            .Name("Name")
            .Context("Context")
            .Statement("Value = 100")
            .TrueDecision("DONE")
            .FalseDecision("NOT DONE")
            .Build();
        var prule = rr.RegisterRule(rule);

        var result = rr.ApplyRule("Context");
        Assert.NotNull(result);
        Assert.Equal(rule.TrueDecision, result.Decision);
    }

    [Fact]
    public void Dictionary_Variable2()
    {
        IRightRule rr = new RightRule.RightRule();

        var dataRegResult = rr.RegisterData("Variable", "Hello World");

        var rule = rr.BuildRule()
            .Number(1)
            .Name("Name")
            .Context("Context")
            .Statement("Variable = 'Hello World'")
            .TrueDecision("DONE")
            .FalseDecision("NOT DONE")
            .Build();
        var prule = rr.RegisterRule(rule);

        var result = rr.ApplyRule("Context");
        Assert.NotNull(result);
        Assert.Equal(rule.TrueDecision, result.Decision);
    }
}
