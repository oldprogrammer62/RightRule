using RightRule.LanguageProcessor.Evaluator;
using RightRule.LanguageProcessor.Interpreter;
using RightRule.LanguageProcessor.Parser;
using RightRule.LanguageProcessor.Tokenizer;

namespace RightRule.Factories.RightRuleLanguageFactory;

internal interface ILanguageFactory
{
    IInterpreter CreateInterpreter(IServiceProvider serviceProvider);

    IInterpreter CreateLanguageInterpreter(IServiceProvider serviceProvider);

    IEvaluator CreateLanguageEvaluator(IServiceProvider serviceProvider);

    IParser CreateParser(IServiceProvider serviceProvider, string Statement, HashSet<string> OperatorNames, bool DataCheck);

    IParser CreateParser(IServiceProvider serviceProvider, ITokenizer tokenizer, bool DataCheck);

    ITokenizer CreateTokenizer(IServiceProvider serviceProvider, string Statement, HashSet<string> OperatorNames);
}