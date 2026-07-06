using RightRule.Extensions;
using RightRule.SyntaxChecker;

namespace RightRuleTests;

public class SyntaxCheckTests
{
    [Fact]
    public void ValidStatement_ReturnsValid()
    {
        var rr = new RightRule.RightRule();
        var result = rr.CheckStatement("name equals \"John\" and numberoforders equals 10");

        Assert.True(result.IsValid);
        Assert.DoesNotContain(result.Diagnostics, d => d.Severity == DiagnosticSeverity.Error);
    }

    [Fact]
    public void UnterminatedString_ReportsDiagnostic_WithSuggestion()
    {
        var rr = new RightRule.RightRule();
        var stmt = "name equals \"John";
        var result = rr.CheckStatement(stmt);

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
    public void UnmatchedLeftParenthesis_ReportsDiagnostic_Code()
    {
        var rr = new RightRule.RightRule();
        var stmt = "(numberoforders equals 10";
        var result = rr.CheckStatement(stmt);

        Assert.False(result.IsValid);
        var diag = result.Diagnostics.FirstOrDefault(d => d.Code == "PAR_UNMATCHED_LPAREN");
        Assert.NotNull(diag);
        Assert.Contains("Unmatched opening parenthesis", diag.Message, System.StringComparison.OrdinalIgnoreCase);
        Assert.True(diag.Position >= 0);
        Assert.Equal(1, diag.Length); // length of '(' token
        Assert.NotNull(diag.Snippet);
    }

    [Fact]
    public void UnexpectedCharacter_ReportsDiagnostic()
    {
        var rr = new RightRule.RightRule();
        var stmt = "name equals #invalid";
        var result = rr.CheckStatement(stmt);

        Assert.False(result.IsValid);
        var diag = result.Diagnostics.FirstOrDefault();
        Assert.NotNull(diag);
        Assert.Contains("Unexpected character", diag.Message, System.StringComparison.OrdinalIgnoreCase);
        Assert.True(diag.Position >= 0);
        Assert.True(diag.Length >= 0);
        Assert.NotNull(diag.Snippet);
    }
}