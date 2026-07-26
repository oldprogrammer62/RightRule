using RightRule.System.Common.Exceptions;
using System.Text;

namespace RightRule.StatementProcessor;

public sealed class Tokenizer
{
    private readonly string _input;
    private int _pos;
    private readonly HashSet<string> _operators; // case-insensitive

    internal Tokenizer(string input, HashSet<string> operatorNames)
    {
        _input = input ?? string.Empty;
        _operators = operatorNames;
    }

    internal Token NextToken() => NextTokenAsync().GetAwaiter().GetResult();

    internal Task<Token> NextTokenAsync()
    {
        return Task.Run(() => ReadNextToken());
    }

    private Token ReadNextToken()
    {
        SkipWhitespace();

        if (_pos >= _input.Length)
            return new Token(TokenType.End, "", _pos);

        char c = _input[_pos];

        // Parentheses
        if (c == '(') { _pos++; return new Token(TokenType.LParen, "(", _pos - 1); }
        if (c == ')') { _pos++; return new Token(TokenType.RParen, ")", _pos - 1); }

        // Number (simple integer support)
        // Must start with a digit, and can contain digits
        if (char.IsDigit(c))
        {
            int start = _pos;
            var sb = new StringBuilder();
            while (_pos < _input.Length && (char.IsDigit(_input[_pos]) || _input[_pos] == '.'))
                sb.Append(_input[_pos++]);
            return new Token(TokenType.Number, sb.ToString(), start);
        }

        // Identifier / Operator / Logical keywords
        // Must start with a letter or underscore, and can contain letters/digits/underscore
        // Must contain one dot (only) to be considered a variable identifier with dataset reference (dot notation)
        if (char.IsLetter(c) || c == '_')
        {
            bool foundDot = false;
            int start = _pos;
            var sb = new StringBuilder();
            sb.Append(_input[_pos++]);

            while (_pos < _input.Length && (char.IsLetterOrDigit(_input[_pos]) || _input[_pos] == '_' || _input[_pos] == '.'))
            {
                if (!foundDot && _input[_pos] == '.') foundDot = true;
                sb.Append(_input[_pos++]);
            }
            var value = sb.ToString();

            // Logical keywords - case sensitive: valid only as lower-case
            if (value == "and")
                return new Token(TokenType.And, value, start);
            if (value == "or")
                return new Token(TokenType.Or, value, start);
            if (value == "not")
                return new Token(TokenType.Not, value, start);

            // boolean
            if (value.Equals("TRUE", StringComparison.OrdinalIgnoreCase))
                return new Token(TokenType.Boolean, value, start);
            if (value.Equals("FALSE", StringComparison.OrdinalIgnoreCase))
                return new Token(TokenType.Boolean, value, start);

            // External operator names
            if (_operators.Contains(value))
                return new Token(TokenType.Operator, value, start);

            // Otherwise it's a variable identifier with dataset reference (dot notation)
            if (foundDot && value.Count(c => c == '.') == 1)
                return new Token(TokenType.Identifier, value, start);

            throw new InvalidIdentifierException(value, start);
        }

        // String literal "..."
        // Supports basic escaping of \" and \\ (optional)
        if (c == '"')
        {
            int start = _pos;
            _pos++; // Skip opening quote
            var sb = new StringBuilder();

            while (_pos < _input.Length && _input[_pos] != '"')
            {
                // basic escape support for \" and \\ (optional)
                if (_input[_pos] == '\\' && _pos + 1 < _input.Length)
                {
                    char next = _input[_pos + 1];
                    if (next == '"' || next == '\\')
                    {
                        sb.Append(next);
                        _pos += 2;
                        continue;
                    }
                }
                sb.Append(_input[_pos++]);
            }

            if (_pos >= _input.Length || _input[_pos] != '"')
                throw new Exception($"Unterminated string literal at position {start}.");

            _pos++; // closing quote
            return new Token(TokenType.String, sb.ToString(), start);
        }

        // Symbolised Operator
        // Must start with a symbol or punctuation character, and can contain letters/digits/symbols/punctuation/underscore
        if (char.IsSymbol(c) || char.IsPunctuation(c))
        {
            int start = _pos;
            var sb = new StringBuilder();
            sb.Append(_input[_pos++]);

            while (_pos < _input.Length &&
                (
                char.IsLetterOrDigit(_input[_pos]) ||
                char.IsSymbol(_input[_pos]) ||
                char.IsPunctuation(_input[_pos]) ||
                _input[_pos] == '_')
                )
                sb.Append(_input[_pos++]);

            var value = sb.ToString();

            // External operator names
            if (_operators.Contains(value))
                return new Token(TokenType.Operator, value, start);
        }
        throw new Exception($"Unexpected character '{c}' at position {_pos}.");
    }

    private void SkipWhitespace()
    {
        while (_pos < _input.Length && char.IsWhiteSpace(_input[_pos]))
            _pos++;
    }
}
