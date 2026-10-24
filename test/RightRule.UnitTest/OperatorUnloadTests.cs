using RightRule;
using RightRule.RuleServices.RuleModels;

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
        IRightRule rr = new RightRule.RightRule();

        // Unload the "Contains" operator
        bool result = rr.UnloadOperator("DoesNotContain");
        Assert.True(result);
    }

    [Fact]
    public void TestItWorks_UnloadOperator_GreaterThanOrEquals()
    {
        // Force RightRule to load all operators
        IRightRule rr = new RightRule.RightRule();

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
            TrueDecision = "VALUEEQUALS",
            FalseDecision = "VALUEUNEQUALS"
        };

        IRightRule rr = new RightRule.RightRule();
        var newRule = rr.RegisterRule(rule);
        rr.RegisterData("Test", _Data);

        // Use EQUALS
        var appliedRuleBefore = rr.ApplyRule("Context");
        Assert.Equal(rule.TrueDecision, appliedRuleBefore.Decision);

        // Unload the "Equals" operator
        bool result = rr.UnloadOperator("Equals");
        Assert.True(result);

        // Use EQUALS
        var appliedRuleAfter = rr.ApplyRule("Context");
        Assert.Equal(ErrorRule.DECISIONWORD, appliedRuleAfter.Decision);
    }
}
