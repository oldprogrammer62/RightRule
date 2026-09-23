using RightRule.LanguageProcessor.Parser;
using RightRule.RuleServices.RuleModels.BaseModels;

namespace RightRule.Factories.ParserFactory;

internal interface IParserFactory
{
    IParser BuildParser(RuleDefinition ruleDefinition);
    IParser BuildParser(string Statement, bool DataCheck);
}