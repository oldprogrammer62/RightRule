using RightRule;
using RightRule.RuleServices.RuleModels;
using RightRule.RuleServices.RuleModels.BaseModels;

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

        IRightRule rr = new RightRule.RightRule();
        var dataRegistered = rr.RegisterData("Data", data);
        RightRule.RuleServices.RuleRegistration.RuleRegistrationResult? ruleRegistered = rr.RegisterRule(rule1);

        RightRuleResult<IProcessedRule>? appliedRule = await rr.ApplyRuleAsync("Context");

        Assert.Equal(appliedRule.Decision, rule1.Decision);
    }
}
