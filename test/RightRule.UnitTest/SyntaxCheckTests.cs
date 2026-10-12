using RightRule;
using RightRule.LanguageProcessor.SyntaxChecker;
using RightRule.RuleServices.RuleModels;

namespace RightRuleTests;

public class SyntaxCheckTests
{
    private readonly Dictionary<string, object> _Data;

    public SyntaxCheckTests()
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
    public async Task ValidStatement_ReturnsValid()
    {
        IRightRule rr = new RightRule.RightRule();
        CheckStatementResult? result = await rr.SyntaxCheckAsync("Data.name equals \"John\" and Data.numberoforders equals 10");

        Assert.True(result.IsValid);
        Assert.DoesNotContain(result.Diagnostics, d => d.Severity == DiagnosticSeverityEnum.ERROR);
    }

    [Fact]
    public async Task ValidStatement_WithArray()
    {
        IRightRule rr = new RightRule.RightRule();
        CheckStatementResult? result = await rr.SyntaxCheckAsync("number in [1,2,3,4,5]");

        Assert.True(result.IsValid);
        Assert.DoesNotContain(result.Diagnostics, d => d.Severity == DiagnosticSeverityEnum.ERROR);
    }

    [Fact]
    public async Task ValidStatement_WithArrayInvalid()
    {
        IRightRule rr = new RightRule.RightRule();
        CheckStatementResult? result = await rr.SyntaxCheckAsync("number in [1,2,3,4,5");

        Assert.False(result.IsValid);
        Assert.Contains(result.Diagnostics, d => d.Severity == DiagnosticSeverityEnum.ERROR);
    }

    [Fact]
    public async Task UnterminatedString_ReportsDiagnostic_WithSuggestion()
    {
        IRightRule rr = new RightRule.RightRule();
        var stmt = "Data.name equals \"John";
        var result = await rr.SyntaxCheckAsync(stmt);

        Assert.False(result.IsValid);
        var diag = result.Diagnostics.FirstOrDefault();
        Assert.NotNull(diag);
        Assert.Contains("Unterminated", diag.Message, System.StringComparison.OrdinalIgnoreCase);
        Assert.NotNull(diag.Suggestion);
        Assert.True(diag.Position >= 0);
        Assert.True(diag.Length >= 1);
        Assert.NotNull(diag.Snippet);
    }

    [Fact]
    public async Task UnmatchedLeftParenthesis_ReportsDiagnostic_Code()
    {
        IRightRule rr = new RightRule.RightRule();
        var stmt = "(Test.numberoforders equals 10";
        var result = await rr.SyntaxCheckAsync(stmt);

        Assert.False(result.IsValid);
        var diag = result.Diagnostics.FirstOrDefault(d => d.Code == "PAR_UNMATCHED_LPAREN");
        Assert.NotNull(diag);
        Assert.Contains("Unmatched opening parenthesis", diag.Message, System.StringComparison.OrdinalIgnoreCase);
        Assert.True(diag.Position >= 0);
        Assert.Equal(1, diag.Length); // length of '(' token
        Assert.NotNull(diag.Snippet);
    }

    [Fact]
    public async Task UnexpectedCharacter_ReportsDiagnostic()
    {
        IRightRule rr = new RightRule.RightRule();
        var stmt = "Test.name equals #invalid";
        var result = await rr.SyntaxCheckAsync(stmt);

        Assert.False(result.IsValid);
        var diag = result.Diagnostics.FirstOrDefault();
        Assert.NotNull(diag);
        Assert.Contains("Unexpected '#invalid'", diag.Message, System.StringComparison.OrdinalIgnoreCase);
        Assert.True(diag.Position >= 0);
        Assert.True(diag.Length >= 0);
        Assert.NotNull(diag.Snippet);
    }

    [Fact]
    public void SyntaxError_Exception()
    {
        // Arrange
        var statement = "(1 = 1 and 2 = 2 and 3 = 3) and not 4 = 44 or not (5 = 5";

        IRightRule rightRule = new RightRule.RightRule();

        // Rule
        DefinedRule rule = new()
        {
            Number = 1,
            Name = "Rule 1",
            Statement = statement,
            Context = "Context",
            Decision = "EQUALS",
            FalseDecision = "NOT_EQUALS",
        };

        // Register the data
        rightRule.RegisterData("TestDataset", _Data);
        var regRule = rightRule.RegisterRule(rule);
        
        Assert.NotNull(regRule);
    }
}