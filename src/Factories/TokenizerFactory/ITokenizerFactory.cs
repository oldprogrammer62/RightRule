using RightRule.LanguageProcessor.Tokenizer;

namespace RightRule.Factories.TokenizerFactory;

internal interface ITokenizerFactory
{
    ITokenizer BuildTokenizer(string statement, int pos, bool dataCheck);
}