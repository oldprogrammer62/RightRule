using RightRule;
using RightRule.DataServices.DataRegistration;
using RightRule.RuleServices.RuleModels;
using RightRule.RuleServices.RuleRegistration;

namespace RightRuleTests;

public class OperatorConfig_Tests
{
    [Fact]
    public void ConfigPassedTo_Equals()
    {
        string OpConfig = "Do something with this config";

        Dictionary<string, object> data = new Dictionary<string, object>
        {
            { "key1", "value1" },
            { "key2", "value2" },
            { "key3", "value3" }
        };

        DefinedRule rule1 = new DefinedRule()
        {
            Context = "Context",
            Name = "Name",
            Number = 1,
            Statement = "Data.key1 = 'value1' and Data.key2 = \'value2\' and Data.key3 = \"value3\"",
            Decision = "Decision",
            FalseDecision = "FalseDecision",
            OptionalData = OpConfig
        };

        IRightRule rr = new RightRule.RightRule();
        DataRegistrationResult? data1Registered = rr.RegisterData("Data", data);
        RuleRegistrationResult? rule1Registered = rr.RegisterRule(rule1);

        var result = rr.ApplyRule("Context");

        Assert.NotNull(result);
        Assert.Equal(OpConfig, rule1Registered.RegisteredRule.OptionalData);
    }
}
