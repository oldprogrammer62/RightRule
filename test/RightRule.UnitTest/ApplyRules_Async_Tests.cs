using RightRule;
using RightRule.DataServices.DataRegistration;
using RightRule.RuleServices.RuleModels;
using RightRule.RuleServices.RuleModels.BaseModels;
using RightRule.RuleServices.RuleRegistration;

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

        IRightRule rr = new RightRule.RightRule();

        DefinedRule rule1 = new DefinedRule()
        {
            Context = "Context",
            Number = 1,
            Name = "Rule1",
            Statement = "Data.field1 = \"value1\"",
            TrueDecision = "DONE",
            FalseDecision = "NOT_DONE"
        };
        RuleRegistrationResult? rule1Registered = rr.RegisterRule(rule1);

        DefinedRule rule2 = new DefinedRule()
        {
            Context = "Context",
            Number = 2,
            Name = "Rule2",
            Statement = "Data.field1 = \"value1\"",
            TrueDecision = "DONE",
            FalseDecision = ""
        };
        RuleRegistrationResult? rule2Registered = rr.RegisterRule(rule2);

        DataRegistrationResult? dataRegistered = rr.RegisterData("Data", data);

        RightRuleResult<IProcessedRule>? appliedRule = await rr.ApplyRuleAsync("Context");

        Assert.Equal(appliedRule.Decision, rule1.TrueDecision);
    }
}
