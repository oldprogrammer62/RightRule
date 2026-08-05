using RightRule.Operators;
using RightRule.StatementProcessor;
using RightRule.StatementProcessor.Parser;
using RightRule.StatementProcessor.Tokenizer;
using System.Text.RegularExpressions;

namespace RightRule.SyntaxChecker;

internal sealed class StatementSyntaxChecker
{
    private static readonly Regex PosRegex = new(@"\bposition\s*(\d+)|\bpos\s*(\d+)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
    private const int ContextRadius = 12; // characters either side for snippet

    internal static Task<CheckStatementResult> CheckAsync(string statement)
    {
        return Task.Run<CheckStatementResult>(() =>
        {
            var diagnostics = new List<Diagnostic>();

            if (string.IsNullOrWhiteSpace(statement))
            {
                diagnostics.Add(new Diagnostic
                {
                    Severity = DiagnosticSeverity.Error,
                    Message = "Statement is empty.",
                    Position = 0,
                    Length = 0,
                    Snippet = string.Empty,
                    Suggestion = "Provide a rule statement to validate."
                });
                return new CheckStatementResult(diagnostics);
            }

            // Obtain operator names (fall back to empty set)
            HashSet<string> operatorNames;
            try
            {
                var ops = new OperatorsService();
                operatorNames = ops.OperatorNames ?? new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            }
            catch (Exception)
            {
                operatorNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            }

            // Token-scan to detect structural issues (balanced parens, unmatched closings, etc.)
            try
            {
                var scanner = new Tokenizer(statement, operatorNames);
                int parenBalance = 0;
                int lastLParenPos = -1;
                while (true)
                {
                    var tok = scanner.NextToken();
                    if (tok.Type == TokenType.End) break;

                    // Detect improper casing of logical keywords (AND/OR/NOT). They must be lower-case.
                    // After tokenizer change, uppercase/mixed-case logical words will be tokenized as Identifier.
                    if (tok.Type == TokenType.Identifier)
                    {
                        var v = tok.Value ?? string.Empty;
                        if (string.Equals(v, "AND", StringComparison.OrdinalIgnoreCase) ||
                            string.Equals(v, "OR", StringComparison.OrdinalIgnoreCase) ||
                            string.Equals(v, "NOT", StringComparison.OrdinalIgnoreCase))
                        {
                            // if not exactly lower-case, report diagnostic
                            if (v != v.ToLowerInvariant())
                            {
                                var s = CreateSnippet(statement, tok.Position, tok.Value?.Length ?? 1);
                                diagnostics.Add(new Diagnostic
                                {
                                    Severity = DiagnosticSeverity.Error,
                                    Message = $"Logical operator '{v}' must be lowercase ('and', 'or', 'not').",
                                    Position = tok.Position,
                                    Length = tok.Value?.Length ?? 1,
                                    Snippet = s,
                                    Suggestion = $"Replace '{v}' with '{v.ToLowerInvariant()}'.",
                                    Code = "LOGIC_OPERATOR_CASE"
                                });
                            }
                        }
                    }

                    if (tok.Type == TokenType.LParen)
                    {
                        parenBalance++;
                        lastLParenPos = tok.Position;
                    }
                    else if (tok.Type == TokenType.RParen)
                    {
                        parenBalance--;
                        if (parenBalance < 0)
                        {
                            var s = CreateSnippet(statement, tok.Position, tok.Value?.Length ?? 1);
                            diagnostics.Add(new Diagnostic
                            {
                                Severity = DiagnosticSeverity.Error,
                                Message = "Unmatched closing parenthesis ')'.",
                                Position = tok.Position,
                                Length = tok.Value?.Length ?? 1,
                                Snippet = s,
                                Suggestion = "Remove the extra ')' or add a matching '('.",
                                Code = "PAR_UNMATCHED_RPAREN"
                            });
                            parenBalance = 0; // keep scanning
                        }
                    }
                }

                if (parenBalance > 0)
                {
                    var s = CreateSnippet(statement, lastLParenPos, 1);
                    diagnostics.Add(new Diagnostic
                    {
                        Severity = DiagnosticSeverity.Error,
                        Message = "Unmatched opening parenthesis '(' found.",
                        Position = lastLParenPos,
                        Length = 1,
                        Snippet = s,
                        Suggestion = "Add a closing ')' to match the opening parenthesis.",
                        Code = "PAR_UNMATCHED_LPAREN"
                    });
                }
            }
            catch (Exception ex)
            {
                // Tokenizer-level errors (unterminated string/variable, unexpected char)
                var diag = CreateDiagnosticFromException(ex, "TKN", statement);
                diagnostics.Add(diag);
                return new CheckStatementResult(diagnostics);
            }

            // Run full parser to capture grammar errors
            try
            {
                var parserTokenizer = new Tokenizer(statement, operatorNames);
                var parser = new Parser(parserTokenizer);
                _ = parser.Parse();

                // if no diagnostics were collected above, statement is valid
                if (diagnostics.Count == 0) return CheckStatementResult.Success();
                return new CheckStatementResult(diagnostics);
            }
            catch (Exception ex)
            {
                var diag = CreateDiagnosticFromException(ex, "PRS", statement);
                diagnostics.Add(diag);

                // heuristics: add friendly hints based on parser messages
                var msg = ex.Message ?? string.Empty;
                if (msg.Contains("Expected operator", StringComparison.OrdinalIgnoreCase))
                {
                    diagnostics.Add(new Diagnostic
                    {
                        Severity = DiagnosticSeverity.Info,
                        Message = "Ensure you used a valid operator name (e.g. equals, contains, notequals) or check operator registration.",
                        Position = diag.Position,
                        Length = diag.Length,
                        Snippet = diag.Snippet,
                        Suggestion = "Verify operator spelling and that the operator is registered.",
                        Code = "HINT_OPERATOR"
                    });
                }
                else if (msg.Contains("Expected left-hand", StringComparison.OrdinalIgnoreCase))
                {
                    diagnostics.Add(new Diagnostic
                    {
                        Severity = DiagnosticSeverity.Info,
                        Message = "Left-hand side must be a variable identifier or a literal (string/number/boolean/placeholder).",
                        Position = diag.Position,
                        Length = diag.Length,
                        Snippet = diag.Snippet,
                        Suggestion = "Use an identifier or literal as the left operand, e.g. fieldName or \"text\".",
                        Code = "HINT_LEFT_OPERAND"
                    });
                }
                else if (msg.Contains("Expected right-hand", StringComparison.OrdinalIgnoreCase))
                {
                    diagnostics.Add(new Diagnostic
                    {
                        Severity = DiagnosticSeverity.Info,
                        Message = "Right-hand side must be a variable identifier, literal or placeholder.",
                        Position = diag.Position,
                        Length = diag.Length,
                        Snippet = diag.Snippet,
                        Suggestion = "Use an identifier, literal, or placeholder as the right operand, e.g. {arg} or 123.",
                        Code = "HINT_RIGHT_OPERAND"
                    });
                }

                return new CheckStatementResult(diagnostics);
            }
        });
    }

    private static Diagnostic CreateDiagnosticFromException(Exception ex, string code, string statement)
    {
        int pos = ExtractPosition(ex.Message);
        int length = 0;
        string snippet;

        if (pos >= 0 && pos < statement.Length)
        {
            length = GetTokenLengthAtPosition(statement, pos);
            snippet = CreateSnippet(statement, pos, length);
        }
        else
        {
            // fallback: show small context around start
            snippet = statement.Length <= ContextRadius * 2 ? statement : statement.Substring(0, Math.Min(statement.Length, ContextRadius * 2)) + (statement.Length > ContextRadius * 2 ? "…" : "");
            length = 0;
        }

        var diag = new Diagnostic
        {
            Severity = DiagnosticSeverity.Error,
            Message = ex.Message ?? "Parsing error.",
            Position = pos,
            Length = length,
            Snippet = snippet,
            Code = code,
            Suggestion = BuildSuggestionFromMessage(ex.Message)
        };
        return diag;
    }

    private static int ExtractPosition(string? message)
    {
        if (string.IsNullOrEmpty(message)) return -1;
        var m = PosRegex.Match(message);
        if (m.Success)
        {
            for (int i = 1; i <= m.Groups.Count - 1; i++)
            {
                var g = m.Groups[i];
                if (g.Success && int.TryParse(g.Value, out var p))
                    return p;
            }
        }

        var idx = message.IndexOf("pos", StringComparison.OrdinalIgnoreCase);
        if (idx >= 0)
        {
            var rest = message.Substring(idx);
            var numMatch = Regex.Match(rest, @"\d+");
            if (numMatch.Success && int.TryParse(numMatch.Value, out var p2)) return p2;
        }
        return -1;
    }

    private static int GetTokenLengthAtPosition(string statement, int pos)
    {
        if (pos < 0 || pos >= statement.Length) return 0;

        // If char is quote, find string token length (up to next quote or end)
        if (statement[pos] == '"')
        {
            int i = pos + 1;
            while (i < statement.Length)
            {
                if (statement[i] == '"' && statement[i - 1] != '\\') { i++; break; }
                i++;
            }
            return Math.Max(1, i - pos);
        }

        // Otherwise, scan forward until whitespace or paren or punctuation that separates tokens
        int end = pos;
        while (end < statement.Length && !char.IsWhiteSpace(statement[end]) && statement[end] != '(' && statement[end] != ')' )
            end++;
        return Math.Max(1, end - pos);
    }

    private static string CreateSnippet(string statement, int position, int length)
    {
        if (string.IsNullOrEmpty(statement)) return string.Empty;
        if (position < 0 || position >= statement.Length)
        {
            // return trimmed statement
            return statement.Length <= ContextRadius * 2 ? statement : statement.Substring(0, ContextRadius * 2) + "…";
        }

        int start = Math.Max(0, position - ContextRadius);
        int end = Math.Min(statement.Length, position + length + ContextRadius);
        var snippet = statement.Substring(start, end - start);
        // Optionally mark the token with brackets for easier UI reading — keep raw to avoid modifying content
        return snippet;
    }

    private static string? BuildSuggestionFromMessage(string? message)
    {
        if (string.IsNullOrEmpty(message)) return null;
        if (message.Contains("Unterminated string", StringComparison.OrdinalIgnoreCase) ||
            message.Contains("Unterminated string literal", StringComparison.OrdinalIgnoreCase))
            return "Close the string with a double-quote '\"'.";
        if (message.Contains("Unterminated Variable", StringComparison.OrdinalIgnoreCase) ||
            message.Contains("Unterminated variable", StringComparison.OrdinalIgnoreCase))
            return "Close the placeholder with a '}' or correct the placeholder syntax like {name}.";
        if (message.Contains("Unexpected character", StringComparison.OrdinalIgnoreCase))
            return "Remove or escape the unexpected character.";
        if (message.Contains("Expected operator", StringComparison.OrdinalIgnoreCase))
            return "Use a registered operator or check the operator name's spelling.";
        return null;
    }
}