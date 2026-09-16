using Microsoft.Extensions.DependencyInjection;
using RightRule.Data;
using RightRule.Factories.RightRuleLanguageFactory;
using RightRule.LanguageProcessor.Evaluator;
using RightRule.LanguageProcessor.Interpreter;
using RightRule.LanguageProcessor.Parser;
using RightRule.LanguageProcessor.Tokenizer;

namespace RightRule.Factories.LanguageFactory;

internal class LanguageFactory : ILanguageFactory
{
    private readonly IDataService dataService;

    public LanguageFactory(IDataService DataService)
    {
        dataService = DataService;
    }

    public IParser CreateParser(IServiceProvider serviceProvider, string Statement, HashSet<string>? OperatorNames, bool DataCheck)
    {
        var tokenizer = CreateTokenizer(serviceProvider, Statement, OperatorNames);
        return CreateParser(serviceProvider, tokenizer, DataCheck);
    }

    public IParser CreateParser(IServiceProvider serviceProvider, ITokenizer tokenizer, bool DataCheck)
    {
        return ActivatorUtilities.CreateInstance<Parser>(serviceProvider, tokenizer, DataCheck);
    }

    public ITokenizer CreateTokenizer(IServiceProvider serviceProvider, string Statement, HashSet<string>? OperatorNames)
    {
        return ActivatorUtilities.CreateInstance<Tokenizer>(serviceProvider, Statement, OperatorNames);
    }
}
