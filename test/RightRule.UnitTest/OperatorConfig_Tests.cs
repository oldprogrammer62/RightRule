using BeSoft;
using RightRule.RuleModels;
using System.Runtime.CompilerServices;

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

        RuleDefinition rule1 = new RuleDefinition()
        {
            Context = "Context",
            Name = "Name",
            Number = 1,
            Statement = "Data.key1 = 'value1' and Data.key2 = \'value2\' and Data.key3 = \"value3\"",
            Decision = "Decision",
            OperatorConfig = OpConfig
        };

        IRightRule rr = new BeSoft.RightRule();
        var data1Registered = rr.RegisterDataset("Data", data);
        var rule1Registered = rr.RegisterRule(rule1);

        var result = rr.EvaluateFirstRule("Context");

        Assert.NotNull(result);
        Assert.Equal(OpConfig, result.OperatorConfig);
    }
}
