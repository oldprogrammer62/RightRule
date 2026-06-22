namespace RightRule.StatementProcessor;

public enum TokenType
{
    Identifier,   // variable names
    Number,       // 123, 456
    String,       // "hello"
    Boolean,      // true, false
    Operator,     // equals, contains, notequals (from registry)
    And,          // AND
    Or,           // OR
    Not,          // NOT
    LParen,       // (
    RParen,       // )
    PlaceHolder,  // "{" identifier "}"
    End
}
