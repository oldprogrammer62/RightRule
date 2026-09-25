namespace RightRule.LanguageProcessor.Tokenizer;

internal enum TokenTypeEnum
{
    IDENTIFIER,   // Data name with dot notation
    NUMBER,       // Literal Number: 123, 456
    STRING,       // Literal String: "hello"
    BOOLEAN,      // true, false
    VARIABLE,     // Variable name
    OPERATOR,     // equals, contains, notequals (from registered Operators)
    AND,          // AND
    OR,           // OR
    NOT,          // NOT
    LPAREN,       // (
    RPAREN,       // )
    END           // End of Statement
}
