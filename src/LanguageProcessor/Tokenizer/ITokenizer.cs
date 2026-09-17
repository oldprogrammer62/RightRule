namespace RightRule.LanguageProcessor.Tokenizer;

public interface ITokenizer
{
    int Pos { get; set; }
    string Statement { get; set; }
    Token NextToken(bool DataCheck);
}