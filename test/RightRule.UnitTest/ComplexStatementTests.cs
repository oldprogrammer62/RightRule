using RightRule.RuleModels;

namespace RightRuleTests;

public class ComplexStatementTests
{
    private readonly Dictionary<string, object> _Data;

    public ComplexStatementTests()
    {
        // Data
        _Data = new Dictionary<string, object>(StringComparer.Ordinal)
        {
            { "A", 1 },
            { "B", 2 },
            { "C", 3 },
            { "D", 4 },
            { "E", 1 }
        };
    }


    [Fact]
    public void AllValid_SomeEquals()
    {
        // Arrange
        var statement = "TestDataset.A = 1 and (TestDataset.B = 2 or TestDataset.C = 3)";
        var rightRule = new BeSoft.RightRule();

        // Rule
        DefinedRule rule = new()
        {
            Number = 1,
            Name = "Rule 1",
            Statement = statement,
            Context = "Context",
            Decision = "EQUALS",
            FalseDecision = "NOT_EQUALS"
        };

        // Register the data
        rightRule.RegisterData("TestDataset", _Data);
        rightRule.RegisterRule(rule);
        var appliedRule = rightRule.EvaluateFirstMatch("Context");

        Assert.Equal(rule.Decision, appliedRule.Decision);
    }

    [Fact]
    public void AllValid_OrSomeEquals()
    {
        // Arrange
        var statement = "TestDataset.A = 10 or TestDataset.B = 2";
        var rightRule = new BeSoft.RightRule();

        // Rule
        DefinedRule rule = new()
        {
            Number = 1,
            Name = "Rule 1",
            Statement = statement,
            Context = "Context",
            Decision = "EQUALS",
            FalseDecision = "NOT_EQUALS"
        };

        // Register the data
        rightRule.RegisterData("TestDataset", _Data);
        rightRule.RegisterRule(rule);
        var appliedRule = rightRule.EvaluateFirstMatch("Context");

        Assert.Equal(rule.Decision, appliedRule.Decision);
    }

    [Fact]
    public void AllValid_Equals()
    {
        // Arrange
        var statement = "(TestDataset.A = 1 and TestDataset.B = 2) and (TestDataset.C = 3 and TestDataset.D = 4)";
        var rightRule = new BeSoft.RightRule();

        // Rule
        DefinedRule rule = new()
        {
            Number = 1,
            Name = "Rule 1",
            Statement = statement,
            Context = "Context",
            Decision = "EQUALS",
            FalseDecision = "NOT_EQUALS"
        };

        // Register the data
        rightRule.RegisterData("TestDataset", _Data);
        rightRule.RegisterRule(rule);
        var appliedRule = rightRule.EvaluateFirstMatch("Context");

        Assert.Equal(rule.Decision, appliedRule.Decision);
    }

    [Fact]
    public void AllValid_NotEquals()
    {
        // Arrange
        var statement = "(TestDataset.A = 1 and TestDataset.B = 2) and (TestDataset.C = 3 and not TestDataset.D = 44)";
        var rightRule = new BeSoft.RightRule();

        // Rule
        DefinedRule rule = new()
        {
            Number = 1,
            Name = "Rule 1",
            Statement = statement,
            Context = "Context",
            Decision = "EQUALS",
            FalseDecision = "NOT_EQUALS"
        };

        // Register the data
        rightRule.RegisterData("TestDataset", _Data);
        rightRule.RegisterRule(rule);
        var appliedRule = rightRule.EvaluateFirstMatch("Context");

        Assert.Equal(rule.Decision, appliedRule.Decision);
    }

    [Fact]
    public void AllValid_Try()
    {
        var _arguments = new Dictionary<string, object>(StringComparer.Ordinal)
        {
            { "X", 1 },
        };

        // Arrange
        var statement = "(1 = 1 and 2 = 2 and 3 = 3) and not 4 = 44 or not (5 = 5)";
        var rightRule = new BeSoft.RightRule();

        // Rule
        DefinedRule rule = new()
        {
            Number = 1,
            Name = "Rule 1",
            Statement = statement,
            Context = "Context",
            Decision = "EQUALS",
            FalseDecision = "NOT_EQUALS"
        };

        // Register the data
        rightRule.RegisterData("TestDataset", _Data);
        rightRule.RegisterRule(rule);
        var appliedRule = rightRule.EvaluateFirstMatch("Context");

        Assert.Equal(rule.Decision, appliedRule.Decision);
    }
}
