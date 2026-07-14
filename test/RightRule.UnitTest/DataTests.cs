using RightRule.Rules;

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
        RuleDefinition rule = new()
        {
            Number = 1,
            Name = "Equals Bool Value Rule",
            Statement = @"Test.whatvalue equals 123",
            Context = "Context",
            Decision = "EQUALS",
        };

        BeSoft.RightRule rr = new BeSoft.RightRule();
        rr.RegisterData("Test", _Data);
        var newRule = rr.RegisterRule(rule);

        if (rr.SilentErrors)
        {
            var appliedRule = rr.ApplyRule("Context");
            Assert.NotNull(appliedRule);
            Assert.Equal(AppliedRule.DefaultRule().Decision, appliedRule.Decision);
        }
        else
        {
            Action test = () => rr.ApplyRule("Context");
            var exception = Record.Exception(test);
            Assert.NotNull(exception);
        }
    }
}
