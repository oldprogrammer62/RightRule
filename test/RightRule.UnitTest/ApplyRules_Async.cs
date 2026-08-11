using RightRule.RuleModels;

namespace RightRuleTests;

public class ApplyRules_Async
{
    [Fact]
    public async Task Full_Async_Test()
    {
        Dictionary<string, object> data = new Dictionary<string, object>()
        {
            { "field1", "value1" }
        };

        RuleDefinition rule1 = new RuleDefinition()
        {
            Context = "Context",
            Number = 1,
            Name = "Rule1",
            Decision = "DONE",
            Statement = "Data.field1 = \"value1\""
        };

        BeSoft.IRightRule rr = new BeSoft.RightRule();
        var dataRegistered = rr.RegisterData("Data", data);
        var ruleRegistered = rr.RegisterRule(rule1);

        var appliedRule = await rr.EvaluateFirstRuleAsync("Context");

        Assert.Equal(appliedRule.Decision, rule1.Decision);
    }
}
