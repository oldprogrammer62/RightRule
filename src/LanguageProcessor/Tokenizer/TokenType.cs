namespace RightRule.LanguageProcessor.Tokenizer;

internal enum TokenType
{
    Identifier,   // Data name with dot notation
    Number,       // Literal Number: 123, 456
    String,       // Literal String: "hello"
    Boolean,      // true, false
    Variable,     // Variable name
    Operator,     // equals, contains, notequals (from registered Operators)
    And,          // AND
    Or,           // OR
    Not,          // NOT
    LParen,       // (
    RParen,       // )
    End           // End of Statement
}
