namespace RightRuleTests;

public class ExceptionTests
{
    [Fact]
    public void Test_RegisterData_NoDatasetName()
    {
        Dictionary<string, object> data = new Dictionary<string, object>()
        {
            { "DataItem1", "Value" }
        };

        BeSoft.IRightRule rr = new BeSoft.RightRule();
        
        Assert.Throws<ArgumentException>(() => rr.RegisterDataset("", data));
    }

    [Fact]
    public void Test_RegisterData_NoData()
    {
        Dictionary<string, object> data = new Dictionary<string, object>()
        {
            { "DataItem1", "Value" }
        };

        BeSoft.IRightRule rr = new BeSoft.RightRule();

        Assert.Throws<ArgumentNullException>(() => rr.RegisterDataset("Datasetname", null));
    }

    [Fact]
    public void Test_RegisterRule_InvalidFormat()
    {
        // TO DO
        // This test returns misleading message
        // And it does not indicate an Error has occurred

        var rule = new RightRule.RuleModels.RuleDefinition()
        {
            Context = "Context",
            Decision = "Done",
            Name = "RuleName",
            Number = 1,
            Statement = "a.b was c.d"
        };
        var rr = new BeSoft.RightRule();

        var ruleReg = rr.RegisterRule(rule);
        Assert.NotNull(ruleReg);
    }
}
