using RightRule;
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

        var rr = new RightRule.RightRule().CreateSession();
        
        Assert.Throws<ArgumentException>(() => rr.DataService.RegisterData("", data));
    }

    [Fact]
    public void Test_RegisterData_NoData()
    {
        Dictionary<string, object> data = new Dictionary<string, object>()
        {
        };

        var rr = new RightRule.RightRule().CreateSession();

        var dataReg = rr.DataService.RegisterData("DatasetName", data);
        Assert.NotNull(dataReg);
    }

    [Fact]
    public void Test_RegisterRule_InvalidFormat()
    {
        var rule = new DefinedRule()
        {
            Context = "Context",
            TrueDecision = "Done",
            FalseDecision = "NotDone",
            Name = "RuleName",
            Number = 1,
            Statement = "a.b was c.d"
        };
        var rr = new RightRule.RightRule().CreateSession();

        var ruleReg = rr.RuleService.RegisterRule(rule);
        Assert.NotNull(ruleReg);
    }
}
