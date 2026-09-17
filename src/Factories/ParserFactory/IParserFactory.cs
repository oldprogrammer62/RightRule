using RightRule.LanguageProcessor.Parser;
using RightRule.Models.BaseModels;

namespace RightRule.Factories.ParserFactory;

internal interface IParserFactory
{
    IParser BuildParser(RuleDefinition ruleDefinition);
    IParser BuildParser(string Statement, bool DataCheck);
}