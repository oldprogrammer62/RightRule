using BeSoft;

namespace RightRuleTests;

public class DecisionDataTests
{
    [Fact]
    public void DecisionDataIn_SameOut()
    {
        IRightRule rr = new BeSoft.RightRule();
        string dd = "Hello World from Decision";

        var rule1 = rr.BuildRuleDefinition()
            .Number(1)
            .Name("Name1")
            .Context("Context")
            .Statement("100 = 100")
            .Decision("SAME")
            .FalseDecision("NOT SAME")
            .DecisionData(dd)
            .OptionalData("Hello World from Optional")
            .WithDataCheck(false)
            .Build();

        var rrule1 = rr.RegisterRule(rule1);

        var result = rr.EvaluateFirstMatch("Context");

        Assert.NotNull(result);
        Assert.Equal(result.DecisionData, dd);
    }

    /// <summary>
    /// TO DO: This test requires work to make it pass
    /// The operator needs to change the value of DecisionData
    /// </summary>
    [Fact]
    public void DecisionDataIn_DiffOut()
    {
        IRightRule rr = new BeSoft.RightRule();
        string dd = "Hello World from Decision";

        var rule1 = rr.BuildRuleDefinition()
            .Number(1)
            .Name("Name1")
            .Context("Context")
            .Statement("100 = 100")
            .Decision("SAME")
            .FalseDecision("NOT SAME")
            .DecisionData(dd)
            .OptionalData("Hello World from Optional")
            .WithDataCheck(false)
            .Build();

        var rrule1 = rr.RegisterRule(rule1);

        var result = rr.EvaluateFirstMatch("Context");

        Assert.NotNull(result);
        // Uncomment when the Operator changes the value of DecisionData
        // Assert.NotEqual(result.DecisionData, dd);
    }

    [Fact]
    public void DecisionDataIn_LogStatement()
    {
        IRightRule rr = new BeSoft.RightRule();
        string dd = "Hello World from Decision";

        var rule1 = rr.BuildRuleDefinition()
            .Number(1)
            .Name("Name1")
            .Context("Context")
            .Statement("100 = 100 and 100 <> 101")
            .Decision("SAME")
            .FalseDecision("NOT SAME")
            .DecisionData(dd)
            .OptionalData("Hello World from Optional")
            .Build();

        var rrule1 = rr.RegisterRule(rule1);

        var result = rr.EvaluateFirstMatch("Context");

        Assert.NotNull(result);
        Assert.Equal(result.DecisionData, dd);
    }
}
