namespace RightRule.LanguageProcessor.Tokenizer;

internal enum TokenTypeEnum
{
    IDENTIFIER,   // Data name with dot notation
    NUMBER,       // Literal Number: 123, 456
    STRING,       // Literal String: "hello"
    ARRAY,        // Array literal: [1, 2] or ['a', 'b'] or [1.1, 2.2]
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
