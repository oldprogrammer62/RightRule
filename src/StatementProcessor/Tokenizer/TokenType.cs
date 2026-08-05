namespace RightRule.StatementProcessor.Tokenizer;

internal enum TokenType
{
    Identifier,   // variable names with dot notation
    Number,       // 123, 456
    String,       // "hello"
    Boolean,      // true, false
    Operator,     // equals, contains, notequals (from registry)
    And,          // AND
    Or,           // OR
    Not,          // NOT
    LParen,       // (
    RParen,       // )
    End
}
