using Microsoft.Extensions.DependencyInjection;
using RightRule.Factories.RightRuleLanguageFactory;
using RightRule.RegistrationServices.DataRegistration;
using RightRule.StatementProcessor.Evaluator;
using RightRule.StatementProcessor.Interpreter;
using RightRule.StatementProcessor.Parser;
using RightRule.StatementProcessor.Tokenizer;
using RightRule.System.Common;

namespace RightRule.Factories.LanguageFactory;

internal class LanguageFactory : ILanguageFactory
{
    public IInterpreter CreateInterpreter(IServiceProvider serviceProvider, Dictionary<string, Operator> Operators, IRegisteredDatasets registeredDatasets, IRegisteredDataObjects registeredDataObjects, Dictionary<string, Operator> operators)
    {
        var evaluator = CreateEvaluator(serviceProvider, Operators);
        return CreateInterpreter(serviceProvider, evaluator, registeredDatasets, registeredDataObjects, operators);
    }

    public IInterpreter CreateInterpreter(IServiceProvider serviceProvider, IEvaluator evaluator, IRegisteredDatasets registeredDatasets, IRegisteredDataObjects registeredDataObjects, Dictionary<string, Operator> operators)
    {
        return ActivatorUtilities.CreateInstance<Interpreter>(serviceProvider, evaluator, registeredDatasets, registeredDataObjects, operators);
    }

    public IEvaluator CreateEvaluator(IServiceProvider serviceProvider, Dictionary<string, Operator> Operators)
    {
        return ActivatorUtilities.CreateInstance<Evaluator>(serviceProvider, Operators);
    }

    public IParser CreateParser(IServiceProvider serviceProvider, string Statement, HashSet<string> OperatorNames, HashSet<string> ObjectNames, HashSet<string> DatasetNames, bool DataCheck)
    {
        var tokenizer = CreateTokenizer(serviceProvider, Statement, OperatorNames, ObjectNames, DatasetNames);
        return CreateParser(serviceProvider, tokenizer, DataCheck);
    }

    public IParser CreateParser(IServiceProvider serviceProvider, ITokenizer tokenizer, bool DataCheck)
    {
        return ActivatorUtilities.CreateInstance<Parser>(serviceProvider, tokenizer, DataCheck);
    }

    public ITokenizer CreateTokenizer(IServiceProvider serviceProvider, string Statement, HashSet<string> OperatorNames, HashSet<string> ObjectNames, HashSet<string> DatasetNames)
    {
        return ActivatorUtilities.CreateInstance<Tokenizer>(serviceProvider, Statement, OperatorNames, ObjectNames, DatasetNames);
    }
}
