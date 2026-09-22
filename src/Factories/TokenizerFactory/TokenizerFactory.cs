using RightRule.LanguageProcessor.Tokenizer;
using RightRule.OperatorServices;
using RightRule.RegistrationServices.DataRegistration;
using RightRule.RegistrationServices.DataRegistration.DataValidator;

namespace RightRule.Factories.TokenizerFactory;

internal class TokenizerFactory : ITokenizerFactory
{
    private readonly IOperatorService _operatorService;
    private readonly IDataRegistrationService _dataService;
    private readonly IDataValidatorService _dataValidator;

    public TokenizerFactory(IOperatorService operatorService, IDataRegistrationService dataService, IDataValidatorService dataValidator)
    {
        _operatorService = operatorService;
        _dataService = dataService;
        _dataValidator = dataValidator;
    }

    public ITokenizer BuildTokenizer(string statement, int pos, bool dataCheck)
    {
        return new Tokenizer(_operatorService, _dataService, _dataValidator, statement, pos, dataCheck);
    }
}
