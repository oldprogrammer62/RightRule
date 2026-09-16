using RightRule.Exceptions;
using System.Text;

namespace RightRule.LanguageProcessor.Tokenizer;

internal sealed class Tokenizer : ITokenizer
{
    private readonly string _statement;
    private int _pos;
    private readonly HashSet<string> _operators;

    public Tokenizer(string statement, HashSet<string> OperatorNames)
    {
        _statement = statement ?? string.Empty;
        _operators = OperatorNames;
    }

    public Token NextToken(bool DataCheck)
    {
        return ReadNextToken(DataCheck);
    }

    private Token ReadNextToken(bool DataCheck)
    {
        SkipWhitespace();

        if (_pos >= _statement.Length)
            return new Token(TokenType.End, "", _pos);

        char c = _statement[_pos];

        // Parentheses
        if (c == '(') { _pos++; return new Token(TokenType.LParen, "(", _pos - 1); }
        if (c == ')') { _pos++; return new Token(TokenType.RParen, ")", _pos - 1); }

        // Number (simple integer support)
        // Must start with a digit, and can contain digits
        if (char.IsDigit(c))
        {
            int start = _pos;
            var sb = new StringBuilder();
            while (_pos < _statement.Length && (char.IsDigit(_statement[_pos]) || _statement[_pos] == '.'))
                sb.Append(_statement[_pos++]);
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
            sb.Append(_statement[_pos++]);

            while (_pos < _statement.Length && (char.IsLetterOrDigit(_statement[_pos]) || _statement[_pos] == '_' || _statement[_pos] == '.'))
            {
                if (!foundDot && _statement[_pos] == '.') foundDot = true;
                sb.Append(_statement[_pos++]);
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
            if (value.Equals("true", StringComparison.Ordinal))
                return new Token(TokenType.Boolean, value, start);
            if (value.Equals("false", StringComparison.Ordinal))
                return new Token(TokenType.Boolean, value, start);

            // External operator names
            if (_operators.Contains(value, StringComparer.Ordinal))
                return new Token(TokenType.Operator, value, start);

            // Variable (No dot notation)
            if (!foundDot)
            {
                if (!DataCheck)
                    return new Token(TokenType.Variable, value, start);

                // TO DO Data check look at existing data
                //if (_objectNames is not null && _objectNames.Contains(value))
                //{
                //    return new Token(TokenType.Variable, value, start);
                //}
            }
            // Dataset identifier (dot notation)
            if (foundDot && value.Count(c => c == '.') == 1)
            {
                if (!DataCheck)
                    return new Token(TokenType.Identifier, value, start);

                // TO DO Data check look at existing data
                //if (DataCheck && _datasetNames is not null && _datasetNames.Contains(value.Split('.')[0]))
                //{
                //    return new Token(TokenType.Identifier, value, start);
                //}
            }
            // Not known - Error
            throw new InvalidIdentifierException(value, start);
        }

        // String literal '...'
        if (c == '\'')
        {
            int start = _pos;
            _pos++; // Skip opening quote
            var sb = new StringBuilder();

            while (_pos < _statement.Length && _statement[_pos] != '\'')
            {
                // basic escape support for \" and \\ (optional)
                if (_statement[_pos] == '\\' && _pos + 1 < _statement.Length)
                {
                    char next = _statement[_pos + 1];
                    if (next == '"' || next == '\\')
                    {
                        sb.Append(next);
                        _pos += 2;
                        continue;
                    }
                }
                sb.Append(_statement[_pos++]);
            }

            if (_pos >= _statement.Length || _statement[_pos] != '\'')
                throw new Exception($"Unterminated string literal at position {start}.");

            _pos++; // closing quote
            return new Token(TokenType.String, sb.ToString(), start);
        }

        // String literal "..."
        // Supports basic escaping of \" and \\ (optional)
        if (c == '"')
        {
            int start = _pos;
            _pos++; // Skip opening quote
            var sb = new StringBuilder();

            while (_pos < _statement.Length && _statement[_pos] != '"')
            {
                // basic escape support for \" and \\ (optional)
                if (_statement[_pos] == '\\' && _pos + 1 < _statement.Length)
                {
                    char next = _statement[_pos + 1];
                    if (next == '"' || next == '\\')
                    {
                        sb.Append(next);
                        _pos += 2;
                        continue;
                    }
                }
                sb.Append(_statement[_pos++]);
            }

            if (_pos >= _statement.Length || _statement[_pos] != '"')
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
            sb.Append(_statement[_pos++]);

            while (_pos < _statement.Length &&
                (
                char.IsLetter(_statement[_pos]) ||
                char.IsLetterOrDigit(_statement[_pos]) ||
                char.IsSymbol(_statement[_pos]) ||
                char.IsPunctuation(_statement[_pos]) ||
                _statement[_pos] == '_')
                )
                sb.Append(_statement[_pos++]);

            var value = sb.ToString();

            // External operator names
            if (_operators.Contains(value))
                return new Token(TokenType.Operator, value, start);

            throw new FormatException($"Unexpected '{sb}' at position {_pos}.");
        }
        throw new FormatException($"Unexpected character '{c}' at position {_pos}.");
    }

    private void SkipWhitespace()
    {
        while (_pos < _statement.Length && char.IsWhiteSpace(_statement[_pos]))
            _pos++;
    }
}
