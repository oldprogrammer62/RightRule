using RightRule.RuleModels;

namespace RightRuleTests;

public class OperatorUnloadTests
{
    private readonly Dictionary<string, object> _Data;

    public OperatorUnloadTests()
    {
        // Data
        _Data = new Dictionary<string, object>(StringComparer.Ordinal)
        {
            { "value", 10 }
        };
    }

    [Fact]
    public void TestItWorks_UnloadOperator_DoesNotContain()
    {
        // Force RightRule to load all operators
        BeSoft.IRightRule rr = new BeSoft.RightRule();

        // Unload the "Contains" operator
        bool result = rr.UnloadOperator("DoesNotContain");
        Assert.True(result);
    }

    [Fact]
    public void TestItWorks_UnloadOperator_GreaterThanOrEquals()
    {
        // Force RightRule to load all operators
        BeSoft.IRightRule rr = new BeSoft.RightRule();

        // Unload the "GreaterThanOrEquals" operator
        bool result = rr.UnloadOperator("GreaterThanOrEquals");
        Assert.True(result);
    }

    [Fact]
    public void TestItWorks_UseItLoseIt_Equals()
    {
        // Rule
        DefinedRule rule = new()
        {
            Number = 1,
            Name = "Equals Value Rule",
            Statement = @"Test.value = 10",
            Context = "Context",
            Decision = "VALUEEQUALS",
            FalseDecision = "VALUEUNEQUALS"
        };

        BeSoft.IRightRule rr = new BeSoft.RightRule();
        var newRule = rr.RegisterRule(rule);
        rr.RegisterData("Test", _Data);

        // Use EQUALS
        var appliedRuleBefore = rr.ApplyRules("Context");
        Assert.Equal(rule.Decision, appliedRuleBefore.Decision);

        // Unload the "Equals" operator
        bool result = rr.UnloadOperator("Equals");
        Assert.True(result);

        // Use EQUALS
        var appliedRuleAfter = rr.ApplyRules("Context");
        Assert.Equal(ErrorRule.DECISIONWORD, appliedRuleAfter.Decision);
    }
}
