using RightRule.RuleServices.RuleModels;

namespace RightRuleTests;

public class ApplyRules_Async_Tests
{
    [Fact]
    public async Task Full_Async_Test()
    {
        Dictionary<string, object> data = new Dictionary<string, object>()
        {
            { "field1", "value1" }
        };

        DefinedRule rule1 = new DefinedRule()
        {
            Context = "Context",
            Number = 1,
            Name = "Rule1",
            Statement = "Data.field1 = \"value1\"",
            Decision = "DONE",
            FalseDecision = "NOT_DONE"
        };

        BeSoft.IRightRule rr = new BeSoft.RightRule();
        var dataRegistered = rr.RegisterData("Data", data);
        var ruleRegistered = rr.RegisterRule(rule1);

        var appliedRule = await rr.ApplyRuleAsync("Context");

        Assert.Equal(appliedRule.Decision, rule1.Decision);
    }
}
