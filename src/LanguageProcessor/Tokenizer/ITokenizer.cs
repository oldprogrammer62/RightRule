namespace RightRule.LanguageProcessor.Tokenizer;

public interface ITokenizer
{
    Token NextToken(bool DataCheck);
}