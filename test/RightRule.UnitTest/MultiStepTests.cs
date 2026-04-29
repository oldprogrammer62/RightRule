using RightRule.Rules;

namespace RightRuleTests;

public class MultiStepTests
{
    private readonly Dictionary<string, object> _Step1Data1;
    private readonly Dictionary<string, object> _Step1Data2;
    private readonly Dictionary<string, object> _Step1Data3;

    private readonly RuleDefinition _Step1Rule1;
    private readonly RuleDefinition _Step1Rule2;
    private readonly RuleDefinition _Step1Rule3;

    private readonly Dictionary<string, object> _Step2Data1;
    private readonly RuleDefinition _Step2Rule;

    public MultiStepTests()
    {
        // Data
        _Step1Data1 = new Dictionary<string, object>(StringComparer.Ordinal)
        {
            { "name", "Step1_1" },
            { "surname", "One" },
            { "fullname", "Step One" },
            { "age", 28 },
            { "numberoforders", 10 },
            { "goodpayer", true },
            { "joined", new DateTime(2020, 1, 1) } // yyyy mm dd
        };
        _Step1Data2 = new Dictionary<string, object>(StringComparer.Ordinal)
        {
            { "name", "Step1_2" },
            { "surname", "One" },
            { "fullname", "Step One" },
            { "age", 29 },
            { "numberoforders", 10 },
            { "goodpayer", true },
            { "joined", new DateTime(2020, 1, 1) } // yyyy mm dd
        };
        _Step1Data3 = new Dictionary<string, object>(StringComparer.Ordinal)
        {
            { "name", "Step1_3" },
            { "surname", "One" },
            { "fullname", "Step One" },
            { "age", 30 },
            { "numberoforders", 10 },
            { "goodpayer", true },
            { "joined", new DateTime(2020, 1, 1) } // yyyy mm dd
        };

        // Rule
        _Step1Rule1 = new()
        {
            Number = 1,
            Name = "Step 1 Rule 1",
            Statement = @"surname = ""One"" and age equals 28",
            Context = "Step1",
            Decision = "OLDENOUGH",
        };

        // Rule
        _Step1Rule2 = new()
        {
            Number = 2,
            Name = "Step 1 Rule 2",
            Statement = @"surname = ""One"" and age equals 29",
            Context = "Step1",
            Decision = "OLDENOUGH",
        };

        // Rule
        _Step1Rule3 = new()
        {
            Number = 3,
            Name = "Step 1 Rule 3",
            Statement = @"surname = ""One"" and age equals 30",
            Context = "Step1",
            Decision = "OLDENOUGH",
        };

        // Data
        _Step2Data1 = new Dictionary<string, object>(StringComparer.Ordinal)
        {
            { "name", "Step" },
            { "surname", "Two" },
            { "fullname", "Step Two" },
            { "age", 60 },
            { "numberoforders", 100 },
            { "goodpayer", true },
            { "joined", new DateTime(2020, 1, 1) } // yyyy mm dd
        };

        // Rule
        _Step2Rule = new()
        {
            Number = 2,
            Name = "Step 2 Rule",
            Statement = @"surname equals ""Two"" and age equals 60",
            Context = "Step2",
            Decision = "NAMEFOUND",
        };
    }

    [Fact]
    public void TestWithMultipleStep1_Rule1()
    {
        RightRule.RightRule rr = new RightRule.RightRule();

        rr.RegisterData("Step1Data1", _Step1Data1);
        rr.RegisterData("Step1Data2", _Step1Data2);
        rr.RegisterData("Step1Data3", _Step1Data3);
        var rule1 = rr.RegisterRule(_Step1Rule1);
        var rule1result = rr.ApplyRules("Step1", RightRule.System.Common.RulesToApply.First);
        Assert.Equal("OLDENOUGH", rule1result.Decision);
        Assert.Equal("Step1Data1", rule1result.DatasetMatched);
    }

    [Fact]
    public void TestWithMultipleStep1_Rule2()
    {
        RightRule.RightRule rr = new RightRule.RightRule();

        rr.RegisterData("Step1Data1", _Step1Data1);
        rr.RegisterData("Step1Data2", _Step1Data2);
        rr.RegisterData("Step1Data3", _Step1Data3);
        var rule1 = rr.RegisterRule(_Step1Rule2);
        var rule1result = rr.ApplyRules("Step1", RightRule.System.Common.RulesToApply.First);
        Assert.Equal("OLDENOUGH", rule1result.Decision);
        Assert.Equal("Step1Data2", rule1result.DatasetMatched);
    }

    [Fact]
    public void TestWithMultipleStep1_Rule3()
    {
        RightRule.RightRule rr = new RightRule.RightRule();

        rr.RegisterData("Step1Data1", _Step1Data1);
        rr.RegisterData("Step1Data2", _Step1Data2);
        rr.RegisterData("Step1Data3", _Step1Data3);
        var rule1 = rr.RegisterRule(_Step1Rule3);
        var rule1result = rr.ApplyRules("Step1", RightRule.System.Common.RulesToApply.First);
        Assert.Equal("OLDENOUGH", rule1result.Decision);
        Assert.Equal("Step1Data3", rule1result.DatasetMatched);
    }

    [Fact]
    public void TestWithMultipleStep2()
    {
        RightRule.RightRule rr = new RightRule.RightRule();

        rr.RegisterData("Step1Data", _Step1Data1);
        var rule1 = rr.RegisterRule(_Step1Rule1);
        var rule1result = rr.ApplyRules("Step1", RightRule.System.Common.RulesToApply.First);
        Assert.Equal("OLDENOUGH", rule1result.Decision);

        rr.RegisterData("Step2Data", _Step2Data1);
        var rule2 = rr.RegisterRule(_Step2Rule);
        var rule2result = rr.ApplyRules("Step2", RightRule.System.Common.RulesToApply.First);
        Assert.Equal("NAMEFOUND", rule2result.Decision);
    }

    [Fact]
    public void TestWithMultipleStep2_WithDeRegistration()
    {
        RightRule.RightRule rr = new RightRule.RightRule();

        rr.RegisterData("Step1Data", _Step1Data1);
        var rule1 = rr.RegisterRule(_Step1Rule1);
        var rule1result = rr.ApplyRules("Step1", RightRule.System.Common.RulesToApply.First);
        Assert.Equal("OLDENOUGH", rule1result.Decision);

        rr.DeRegisterData("Step1Data");
        rr.DeRegisterRules("Step1");

        var rule1result_v2 = rr.ApplyRules("Step1", RightRule.System.Common.RulesToApply.First);
        Assert.Equal(AppliedRule.DefaultRule().Decision, rule1result_v2.Decision);

        rr.RegisterData("Step2Data", _Step2Data1);
        var rule2 = rr.RegisterRule(_Step2Rule);
        var rule2result = rr.ApplyRules("Step2", RightRule.System.Common.RulesToApply.First);
        Assert.Equal("NAMEFOUND", rule2result.Decision);
    }
}
