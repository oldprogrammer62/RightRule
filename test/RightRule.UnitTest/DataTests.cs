using RightRule.RuleServices.RuleModels;

namespace RightRuleTests;

public class DataTests
{
    private readonly Dictionary<string, object> _Data;

    public DataTests()
    {
        // Data
        _Data = new Dictionary<string, object>(StringComparer.Ordinal)
        {
            { "boolvalue", true },
            { "intvalue", 100 }
        };
    }

    [Fact]
    public void MissingDataException()
    {
        // Depends on SilentEvaluatorExceptions setting being false (default)

        // Rule
        DefinedRule rule = new()
        {
            Number = 1,
            Name = "Equals Bool Value Rule",
            Statement = @"Test.whatvalue equals 123",
            Context = "Context",
            Decision = "EQUALS",
            FalseDecision = "NOTEQUALS"
        };

        BeSoft.IRightRule rr = new BeSoft.RightRule();
        rr.RegisterData("Test", _Data);
        var newRule = rr.RegisterRule(rule);

        var ap = rr.ApplyRule("Context");

        Assert.Equal(ErrorRule.DECISIONWORD, ap.Decision);
    }
}
