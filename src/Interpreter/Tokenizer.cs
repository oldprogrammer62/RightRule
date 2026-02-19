namespace RightRule.Interpreter;

using System;
using System.Collections.Generic;
using System.Text;

public sealed class Tokenizer
{
    private readonly string _input;
    private int _pos;

    private readonly HashSet<string> _operators; // case-insensitive
    public Tokenizer(string input, IEnumerable<string> operatorNames)
    {
        _input = input ?? string.Empty;
        _operators = new HashSet<string>(operatorNames ?? Array.Empty<string>(), StringComparer.OrdinalIgnoreCase);
    }

    public Token NextToken()
    {
        SkipWhitespace();

        if (_pos >= _input.Length)
            return new Token(TokenType.End, "", _pos);

        char c = _input[_pos];

        // Parentheses
        if (c == '(') { _pos++; return new Token(TokenType.LParen, "(", _pos - 1); }
        if (c == ')') { _pos++; return new Token(TokenType.RParen, ")", _pos - 1); }

        // Number (simple integer support)
        if (char.IsDigit(c))
        {
            int start = _pos;
            var sb = new StringBuilder();
            while (_pos < _input.Length && char.IsDigit(_input[_pos]))
                sb.Append(_input[_pos++]);
            return new Token(TokenType.Number, sb.ToString(), start);
        }

        // Identifier / Operator / Logical keywords
        if (char.IsLetter(c) || c == '_')
        {
            int start = _pos;
            var sb = new StringBuilder();
            sb.Append(_input[_pos++]);

            while (_pos < _input.Length && (char.IsLetterOrDigit(_input[_pos]) || _input[_pos] == '_'))
                sb.Append(_input[_pos++]);

            var value = sb.ToString();

            // Logical keywords
            if (value.Equals("AND", StringComparison.OrdinalIgnoreCase))
                return new Token(TokenType.And, value, start);
            if (value.Equals("OR", StringComparison.OrdinalIgnoreCase))
                return new Token(TokenType.Or, value, start);
            if (value.Equals("NOT", StringComparison.OrdinalIgnoreCase))
                return new Token(TokenType.Not, value, start);

            // External operator names
            if (_operators.Contains(value))
                return new Token(TokenType.Operator, value, start);

            // Otherwise it's a variable identifier
            return new Token(TokenType.Identifier, value, start);
        }

        // String literal "..."
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

        throw new Exception($"Unexpected character '{c}' at position {_pos}.");
    }

    private void SkipWhitespace()
    {
        while (_pos < _input.Length && char.IsWhiteSpace(_input[_pos]))
            _pos++;
    }
}
