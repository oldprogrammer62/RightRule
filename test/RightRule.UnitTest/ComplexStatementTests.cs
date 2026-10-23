using RightRule;
using RightRule.Models;
using RightRule.RuleServices.RuleModels;

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
    public void ComplexLiteralsTest()
    {
        string statement = "1 = 1";
        //string statement = "1 = 1 and 2 = 2";
        //string statement = "1 = 1 or 2 = 2";
        //string statement = "1 = 1 and not 2 = 2";
        //string statement = "1 = 1 and (2 = 2 or 3 = 3)";
        //string statement = "1 = 1 and 2 = 2 and not 3 = 3";

        IRightRule rr = new RightRule.RightRule();
        var rule1 = rr.BuildRule()
            .Number(1)
            .Name("Rule1")
            .Context("Context")
            .Statement(statement)
            .Decision("GOOD")
            .FalseDecision("BAD")
            .DecisionData(null)
            .FalseDecisionData(null)
            .OptionalData(null)
            .Build()
            .Register(rr.RuleService);
        Assert.NotNull(rule1);
        Assert.NotNull(rule1.RegisteredRule);
        Assert.Equal(RightRuleStatusEnum.SUCCESS, rule1.Status);

        var result = rr.ApplyRule("Context");
        Assert.NotNull(result);
        Assert.Equal(rule1.RegisteredRule.Decision, result.Decision);
    }

    [Fact]
    public void AllValid_SomeEquals()
    {
        // Arrange
        var statement = "TestDataset.A = 1 and (TestDataset.B = 2 or TestDataset.C = 3)";
        IRightRule rightRule = new RightRule.RightRule();

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
        var appliedRule = rightRule.ApplyRule("Context");

        Assert.Equal(rule.Decision, appliedRule.Decision);
    }

    [Fact]
    public void AllValid_OrSomeEquals()
    {
        // Arrange
        var statement = "TestDataset.A = 10 or TestDataset.B = 2";
        IRightRule rightRule = new RightRule.RightRule();

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
        var appliedRule = rightRule.ApplyRule("Context");

        Assert.Equal(rule.Decision, appliedRule.Decision);
    }

    [Fact]
    public void AllValid_Equals()
    {
        // Arrange
        var statement = "(TestDataset.A = 1 and TestDataset.B = 2) and (TestDataset.C = 3 and TestDataset.D = 4)";
        IRightRule rightRule = new RightRule.RightRule();

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
        var appliedRule = rightRule.ApplyRule("Context");

        Assert.Equal(rule.Decision, appliedRule.Decision);
    }

    [Fact]
    public void AllValid_NotEquals()
    {
        // Arrange
        var statement = "(TestDataset.A = 1 and TestDataset.B = 2) and (TestDataset.C = 3 and not TestDataset.D = 44)";
        IRightRule rightRule = new RightRule.RightRule();

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
        var appliedRule = rightRule.ApplyRule("Context");

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
        IRightRule rightRule = new RightRule.RightRule();

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
        var appliedRule = rightRule.ApplyRule("Context");

        Assert.Equal(rule.Decision, appliedRule.Decision);
    }
}
