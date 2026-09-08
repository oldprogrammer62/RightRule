using RightRule.LanguageProcessor.Evaluator;
using RightRule.LanguageProcessor.Interpreter;
using RightRule.LanguageProcessor.Parser;
using RightRule.LanguageProcessor.Tokenizer;
using RightRule.RegistrationServices.DataRegistration;
using RightRule.System.Common;

namespace RightRule.Factories.RightRuleLanguageFactory;

internal interface ILanguageFactory
{
    IInterpreter CreateInterpreter(IServiceProvider serviceProvider, Dictionary<string, Operator> Operators, IRegisteredDatasets registeredDatasets, IRegisteredDataObjects registeredDataObjects, Dictionary<string, Operator> operators);

    IInterpreter CreateInterpreter(IServiceProvider serviceProvider, IEvaluator evaluator, IRegisteredDatasets registeredDatasets, IRegisteredDataObjects registeredDataObjects, Dictionary<string, Operator> operators);

    IEvaluator CreateEvaluator(IServiceProvider serviceProvider, Dictionary<string, Operator> Operators);

    IParser CreateParser(IServiceProvider serviceProvider, string Statement, HashSet<string> OperatorNames, HashSet<string> ObjectNames, HashSet<string> DatasetNames, bool DataCheck);

    IParser CreateParser(IServiceProvider serviceProvider, ITokenizer tokenizer, bool DataCheck);

    ITokenizer CreateTokenizer(IServiceProvider serviceProvider, string Statement, HashSet<string> OperatorNames, HashSet<string> ObjectNames, HashSet<string> DatasetNames);
}