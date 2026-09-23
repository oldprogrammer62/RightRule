using RightRule.RuleServices.RuleModels;

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
        
        Assert.Throws<ArgumentException>(() => rr.RegisterData("", data));
    }

    [Fact]
    public void Test_RegisterData_NoData()
    {
        Dictionary<string, object> data = new Dictionary<string, object>()
        {
        };

        BeSoft.IRightRule rr = new BeSoft.RightRule();

        var dataReg = rr.RegisterData("DatasetName", data);
        Assert.NotNull(dataReg);
    }

    [Fact]
    public void Test_RegisterRule_InvalidFormat()
    {
        var rule = new DefinedRule()
        {
            Context = "Context",
            Decision = "Done",
            FalseDecision = "NotDone",
            Name = "RuleName",
            Number = 1,
            Statement = "a.b was c.d"
        };
        var rr = new BeSoft.RightRule();

        var ruleReg = rr.RegisterRule(rule);
        Assert.NotNull(ruleReg);
    }
}
