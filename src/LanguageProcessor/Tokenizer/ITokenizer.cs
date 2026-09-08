namespace RightRule.StatementProcessor.Tokenizer;

public interface ITokenizer
{
    Token NextToken(bool DataCheck);
}