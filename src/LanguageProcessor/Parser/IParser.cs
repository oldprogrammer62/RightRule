namespace RightRule.LanguageProcessor.Parser;

public interface IParser
{
    bool DataCheck { get; set; }
    string Statement { get; set; }
    Node Parse();
}