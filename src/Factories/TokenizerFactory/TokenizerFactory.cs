using RightRule.LanguageProcessor.Tokenizer;
using RightRule.OperatorServices;
using RightRule.RegistrationServices.DataRegistration;

namespace RightRule.Factories.TokenizerFactory;

internal class TokenizerFactory : ITokenizerFactory
{
    private readonly IOperatorService _operatorService;
    private readonly IDataService _dataService;

    public TokenizerFactory(IOperatorService operatorService, IDataService dataService)
    {
        _operatorService = operatorService;
        _dataService = dataService;
    }

    public ITokenizer BuildTokenizer(string statement, int pos, bool dataCheck)
    {
        return new Tokenizer(_operatorService, _dataService, statement, pos, dataCheck);
    }
}
