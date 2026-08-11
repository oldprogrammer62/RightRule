using RightRule.RuleModels;

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
            { "numberoforders", 10 },
            { "goodpayer", true },
            { "joined", new DateTime(2020, 1, 1) } // yyyy mm dd
        };
        _Step1Data2 = new Dictionary<string, object>(StringComparer.Ordinal)
        {
            { "name", "Step1_2" },
            { "surname", "One" },
            { "fullname", "Step One" },
            { "numberoforders", 11 },
            { "goodpayer", true },
            { "joined", new DateTime(2020, 1, 1) } // yyyy mm dd
        };
        _Step1Data3 = new Dictionary<string, object>(StringComparer.Ordinal)
        {
            { "name", "Step1_3" },
            { "surname", "One" },
            { "fullname", "Step One" },
            { "numberoforders", 12 },
            { "goodpayer", true },
            { "joined", new DateTime(2020, 1, 1) } // yyyy mm dd
        };

        // Rule
        _Step1Rule1 = new()
        {
            Number = 1,
            Name = "Step 1 Rule 1",
            Statement = @"Step1Data1.surname = ""One"" and Step1Data1.numberoforders equals 10",
            Context = "Step1",
            Decision = "GOOD",
        };

        // Rule
        _Step1Rule2 = new()
        {
            Number = 2,
            Name = "Step 1 Rule 2",
            Statement = @"Step1Data1.surname = ""One"" and Step1Data2.numberoforders equals 11",
            Context = "Step1",
            Decision = "GOOD",
        };

        // Rule
        _Step1Rule3 = new()
        {
            Number = 3,
            Name = "Step 1 Rule 3",
            Statement = @"Step1Data3.surname = ""One"" and Step1Data3.numberoforders equals 12",
            Context = "Step1",
            Decision = "GOOD",
        };

        // Data
        _Step2Data1 = new Dictionary<string, object>(StringComparer.Ordinal)
        {
            { "name", "Step2_1" },
            { "surname", "Two" },
            { "fullname", "Step Two" },
            { "numberoforders", 100 },
            { "goodpayer", true },
            { "joined", new DateTime(2020, 1, 1) } // yyyy mm dd
        };

        // Rule
        _Step2Rule = new()
        {
            Number = 2,
            Name = "Step 2 Rule",
            Statement = @"Step2Data1.surname equals ""Two"" and Step2Data1.numberoforders equals 100",
            Context = "Step2",
            Decision = "GOOD",
        };
    }

    [Fact]
    public void TestWithMultipleStep1_Rule1()
    {
        BeSoft.IRightRule rr = new BeSoft.RightRule();

        rr.RegisterData("Step1Data1", _Step1Data1);
        rr.RegisterData("Step1Data2", _Step1Data2);
        rr.RegisterData("Step1Data3", _Step1Data3);
        var rule1 = rr.RegisterRule(_Step1Rule1);
        var rule1result = rr.EvaluateFirstRule("Step1");
        Assert.Equal("GOOD", rule1result.Decision);
    }

    [Fact]
    public void TestWithMultipleStep1_Rule2()
    {
        BeSoft.IRightRule rr = new BeSoft.RightRule();

        rr.RegisterData("Step1Data1", _Step1Data1);
        rr.RegisterData("Step1Data2", _Step1Data2);
        rr.RegisterData("Step1Data3", _Step1Data3);
        var rule1 = rr.RegisterRule(_Step1Rule2);
        var rule1result = rr.EvaluateFirstRule("Step1");
        Assert.Equal("GOOD", rule1result.Decision);
    }

    [Fact]
    public void TestWithMultipleStep1_Rule3()
    {
        BeSoft.IRightRule rr = new BeSoft.RightRule();

        rr.RegisterData("Step1Data1", _Step1Data1);
        rr.RegisterData("Step1Data2", _Step1Data2);
        rr.RegisterData("Step1Data3", _Step1Data3);
        var rule1 = rr.RegisterRule(_Step1Rule3);
        var rule1result = rr.EvaluateFirstRule("Step1");
        Assert.Equal("GOOD", rule1result.Decision);
    }

    [Fact]
    public void TestWithMultipleStep2()
    {
        BeSoft.IRightRule rr = new BeSoft.RightRule();

        rr.RegisterData("Step1Data1", _Step1Data1);
        var rule1 = rr.RegisterRule(_Step1Rule1);
        var rule1result = rr.EvaluateFirstRule("Step1");
        Assert.Equal("GOOD", rule1result.Decision);

        rr.RegisterData("Step2Data1", _Step2Data1);
        var rule2 = rr.RegisterRule(_Step2Rule);
        var rule2result = rr.EvaluateFirstRule("Step2");
        Assert.Equal("GOOD", rule2result.Decision);
    }

    [Fact]
    public void TestWithMultipleStep2_WithDeRegistration()
    {
        BeSoft.IRightRule rr = new BeSoft.RightRule();

        rr.RegisterData("Step1Data1", _Step1Data1);
        var rule1 = rr.RegisterRule(_Step1Rule1);
        var rule1result = rr.EvaluateFirstRule("Step1");
        Assert.Equal("GOOD", rule1result.Decision);

        rr.DeRegisterData("Step1Data1");
        rr.DeRegisterRules("Step1");

        var rule1result_v2 = rr.EvaluateFirstRule("Step1");
        Assert.Equal(AppliedRule.ErrorDecision, rule1result_v2.Decision);

        rr.RegisterData("Step2Data1", _Step2Data1);
        var rule2 = rr.RegisterRule(_Step2Rule);
        var rule2result = rr.EvaluateFirstRule("Step2");
        Assert.Equal("GOOD", rule2result.Decision);
    }
}
