using RightRule.DataServices;
using RightRule.DataServices.DataValidator;
using RightRule.Exceptions;
using RightRule.OperatorServices;
using System.Text;

namespace RightRule.LanguageProcessor.Tokenizer;

internal sealed class Tokenizer : ITokenizer
{
    private int _pos = 0;
    private readonly bool _dataCheck = false;
    private readonly string _statement = string.Empty;

    private readonly IOperatorService _operatorService;
    private readonly IDataService _dataService;
    private readonly IDataValidatorService _dataValidator;

    public Tokenizer(IOperatorService operatorService, IDataService dataService, IDataValidatorService dataValidator)
    {
        _operatorService = operatorService;
        _dataService = dataService;
        _dataValidator = dataValidator;
    }

    public Tokenizer(IOperatorService operatorService, IDataService dataService, IDataValidatorService dataValidator, string statement, int pos, bool dataCheck)
    {
        _operatorService = operatorService;
        _dataService = dataService;
        _dataValidator = dataValidator;
        _statement = statement;
        _pos = pos;
        _dataCheck = dataCheck;
    }

    public Token NextToken()
    {
        return ReadNextToken();
    }

    private Token ReadNextToken()
    {
        SkipWhitespace();

        if (_pos >= _statement.Length)
            return new Token(TokenTypeEnum.END, "", _pos);

        char c = _statement[_pos];

        // Parentheses
        if (c == '(') { _pos++; return new Token(TokenTypeEnum.LPAREN, "(", _pos - 1); }
        if (c == ')') { _pos++; return new Token(TokenTypeEnum.RPAREN, ")", _pos - 1); }
        if (c == '[') return ReadArrayToken();

        // Number (simple integer support)
        // Must start with a digit, and can contain digits
        if (char.IsDigit(c))
        {
            int start = _pos;
            var sb = new StringBuilder();
            while (_pos < _statement.Length && (char.IsDigit(_statement[_pos]) || _statement[_pos] == '.'))
                sb.Append(_statement[_pos++]);
            return new Token(TokenTypeEnum.NUMBER, sb.ToString(), start);
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
            if (value.Equals("and", StringComparison.Ordinal))
                return new Token(TokenTypeEnum.AND, value, start);
            if (value.Equals("or", StringComparison.Ordinal))
                return new Token(TokenTypeEnum.OR, value, start);
            if (value.Equals("not", StringComparison.Ordinal))
                return new Token(TokenTypeEnum.NOT, value, start);

            // Logical keywords - wrong case
            if (value.ToUpper().Equals("AND", StringComparison.Ordinal))
                throw new InvalidIdentifierException(value, start);
            if (value.ToUpper().Equals("OR", StringComparison.Ordinal))
                throw new InvalidIdentifierException(value, start);
            if (value.ToUpper().Equals("NOT", StringComparison.Ordinal))
                throw new InvalidIdentifierException(value, start);

            // boolean - case sensitive: valid only as lower-case
            if (value.Equals("true", StringComparison.Ordinal))
                return new Token(TokenTypeEnum.BOOLEAN, value, start);
            if (value.Equals("false", StringComparison.Ordinal))
                return new Token(TokenTypeEnum.BOOLEAN, value, start);

            // boolean - wrong case
            if (value.ToUpper().Equals("TRUE", StringComparison.Ordinal))
                throw new InvalidIdentifierException(value, start);
            if (value.ToUpper().Equals("FALSE", StringComparison.Ordinal))
                throw new InvalidIdentifierException(value, start);

            // External operator names
            if (IsLogicOperator(value))
                return new Token(TokenTypeEnum.OPERATOR, value, start);

            // Variable (No dot notation)
            if (!foundDot)
            {
                if (!_dataCheck)
                    return new Token(TokenTypeEnum.VARIABLE, value, start);

                if (IsValidData(value))
                {
                    return new Token(TokenTypeEnum.VARIABLE, value, start);
                }
            }
            // Dataset identifier (dot notation)
            if (foundDot && FoundOneDot(value))
            {
                if (!_dataCheck)
                    return new Token(TokenTypeEnum.IDENTIFIER, value, start);

                if (IsValidData(value))
                {
                    return new Token(TokenTypeEnum.IDENTIFIER, value, start);
                }
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
            return new Token(TokenTypeEnum.STRING, sb.ToString(), start);
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
            return new Token(TokenTypeEnum.STRING, sb.ToString(), start);
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
            if (IsLogicOperator(value))
                return new Token(TokenTypeEnum.OPERATOR, value, start);

            throw new FormatException($"Unexpected '{sb}' at position {_pos}.");
        }
        throw new FormatException($"Unexpected character '{c}' at position {_pos}.");
    }

    private Token ReadArrayToken()
    {
        int start = _pos;
        char quote = '\0';
        _pos++; // Skip '['
        while (_pos < _statement.Length)
        {
            char current = _statement[_pos];
            if (quote != '\0')
            {
                if (current == '\\' && _pos + 1 < _statement.Length)
                {
                    _pos += 2; // Skip escaped character
                    continue;
                }
                if (current == quote)
                    quote = '\0';
                _pos++;
                continue;
            }
            if (current is '\'' or '"')
            {
                quote = current;
                _pos++;
                continue;
            }
            if (current == '[')
                throw new FormatException($"Nested arrays are not supported at position {_pos}.");

            if (current == ']')
            {
                _pos++;
                string literal = _statement[start.._pos];
                _ = ArrayLiteralParser.Parse(literal); // Validate the literal now.
                return new Token(TokenTypeEnum.ARRAY, literal, start);
            }
            _pos++;
        }
        throw new FormatException($"Unterminated array literal at position {start}.");
    }

    private bool IsLogicOperator(string OpName)
    {
        var _operatorNames = _operatorService.OperatorAliases.Keys;
        if (_operatorNames == null || _operatorNames.Count == 0)
        {
            return false;
        }
        return _operatorNames.Contains(OpName, StringComparer.Ordinal);
    }

    private bool IsValidData(string DatasetName)
    {
        return _dataValidator.IsDataIdentifier(_dataService, DatasetName);
    }

    private void SkipWhitespace()
    {
        while (_pos < _statement.Length && char.IsWhiteSpace(_statement[_pos]))
            _pos++;
    }

    private bool FoundOneDot(string Value)
    {
        if (string.IsNullOrWhiteSpace(Value))
        {
            return false;
        }
        char dot = '.';
        int dotCount = 1;
        return Value.Count(c => c == dot) == dotCount;
    }
}
