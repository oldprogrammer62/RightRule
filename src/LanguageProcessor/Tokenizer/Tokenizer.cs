using RightRule.Exceptions;
using RightRule.OperatorServices;
using RightRule.RegistrationServices.DataRegistration;
using System.Text;

namespace RightRule.LanguageProcessor.Tokenizer;

public sealed class Tokenizer : ITokenizer
{
    public string Statement { get; set; } = string.Empty;
    public int Pos { get; set; } = 0;

    private readonly IOperatorService _operatorService;
    private readonly IDataService _dataService;

    public Tokenizer(IOperatorService operatorService, IDataService dataService)
    {
        _operatorService = operatorService;
        _dataService = dataService;
    }

    public Token NextToken(bool DataCheck)
    {
        return ReadNextToken(DataCheck);
    }

    private Token ReadNextToken(bool DataCheck)
    {
        SkipWhitespace();

        if (Pos >= Statement.Length)
            return new Token(TokenType.End, "", Pos);

        char c = Statement[Pos];

        // Parentheses
        if (c == '(') { Pos++; return new Token(TokenType.LParen, "(", Pos - 1); }
        if (c == ')') { Pos++; return new Token(TokenType.RParen, ")", Pos - 1); }

        // Number (simple integer support)
        // Must start with a digit, and can contain digits
        if (char.IsDigit(c))
        {
            int start = Pos;
            var sb = new StringBuilder();
            while (Pos < Statement.Length && (char.IsDigit(Statement[Pos]) || Statement[Pos] == '.'))
                sb.Append(Statement[Pos++]);
            return new Token(TokenType.Number, sb.ToString(), start);
        }

        // Identifier / Operator / Logical keywords
        // Must start with a letter or underscore, and can contain letters/digits/underscore
        // Must contain one dot (only) to be considered a variable identifier with dataset reference (dot notation)
        if (char.IsLetter(c) || c == '_')
        {
            bool foundDot = false;
            int start = Pos;
            var sb = new StringBuilder();
            sb.Append(Statement[Pos++]);

            while (Pos < Statement.Length && (char.IsLetterOrDigit(Statement[Pos]) || Statement[Pos] == '_' || Statement[Pos] == '.'))
            {
                if (!foundDot && Statement[Pos] == '.') foundDot = true;
                sb.Append(Statement[Pos++]);
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
            if (IsLogicOperator(value))
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
            int start = Pos;
            Pos++; // Skip opening quote
            var sb = new StringBuilder();

            while (Pos < Statement.Length && Statement[Pos] != '\'')
            {
                // basic escape support for \" and \\ (optional)
                if (Statement[Pos] == '\\' && Pos + 1 < Statement.Length)
                {
                    char next = Statement[Pos + 1];
                    if (next == '"' || next == '\\')
                    {
                        sb.Append(next);
                        Pos += 2;
                        continue;
                    }
                }
                sb.Append(Statement[Pos++]);
            }

            if (Pos >= Statement.Length || Statement[Pos] != '\'')
                throw new Exception($"Unterminated string literal at position {start}.");

            Pos++; // closing quote
            return new Token(TokenType.String, sb.ToString(), start);
        }

        // String literal "..."
        // Supports basic escaping of \" and \\ (optional)
        if (c == '"')
        {
            int start = Pos;
            Pos++; // Skip opening quote
            var sb = new StringBuilder();

            while (Pos < Statement.Length && Statement[Pos] != '"')
            {
                // basic escape support for \" and \\ (optional)
                if (Statement[Pos] == '\\' && Pos + 1 < Statement.Length)
                {
                    char next = Statement[Pos + 1];
                    if (next == '"' || next == '\\')
                    {
                        sb.Append(next);
                        Pos += 2;
                        continue;
                    }
                }
                sb.Append(Statement[Pos++]);
            }

            if (Pos >= Statement.Length || Statement[Pos] != '"')
                throw new Exception($"Unterminated string literal at position {start}.");

            Pos++; // closing quote
            return new Token(TokenType.String, sb.ToString(), start);
        }

        // Symbolised Operator
        // Must start with a symbol or punctuation character, and can contain letters/digits/symbols/punctuation/underscore
        if (char.IsSymbol(c) || char.IsPunctuation(c))
        {
            int start = Pos;
            var sb = new StringBuilder();
            sb.Append(Statement[Pos++]);

            while (Pos < Statement.Length &&
                (
                char.IsLetter(Statement[Pos]) ||
                char.IsLetterOrDigit(Statement[Pos]) ||
                char.IsSymbol(Statement[Pos]) ||
                char.IsPunctuation(Statement[Pos]) ||
                Statement[Pos] == '_')
                )
                sb.Append(Statement[Pos++]);

            var value = sb.ToString();

            // External operator names
            if (IsLogicOperator(value))
                return new Token(TokenType.Operator, value, start);

            throw new FormatException($"Unexpected '{sb}' at position {Pos}.");
        }
        throw new FormatException($"Unexpected character '{c}' at position {Pos}.");
    }

    private bool IsLogicOperator(string OpName)
    {
        var _operatorNames = _operatorService.OperatorNames;
        if (_operatorNames == null || _operatorNames.Count == 0)
        {
            return false;
        }
        return _operatorNames.Contains(OpName, StringComparer.Ordinal);
    }

    private void SkipWhitespace()
    {
        while (Pos < Statement.Length && char.IsWhiteSpace(Statement[Pos]))
            Pos++;
    }
}
