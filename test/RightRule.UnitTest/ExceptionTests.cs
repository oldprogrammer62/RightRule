using RightRule.RuleModels.RuleModels;

namespace RightRuleTests;

public class ExceptionTests
{
    [Fact]
    public void Test_RegisterData_NoDataName()
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
            { "DataItem1", "Value" }
        };

        BeSoft.IRightRule rr = new BeSoft.RightRule();

        Assert.Throws<ArgumentNullException>(() => rr.RegisterData("DataName", null));
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
