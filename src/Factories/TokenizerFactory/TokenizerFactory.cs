using RightRule.DataServices;
using RightRule.DataServices.DataValidator;
using RightRule.LanguageProcessor.Tokenizer;
using RightRule.Models;
using RightRule.OperatorServices;

namespace RightRule.Factories.TokenizerFactory;

internal class TokenizerFactory : ITokenizerFactory
{
    private readonly IOperatorService _operatorService;
    private readonly IDataService _dataService;
    private readonly IDataValidatorService _dataValidator;
    private readonly RightRuleOptions _options;

    public TokenizerFactory(IOperatorService operatorService, IDataService dataService, IDataValidatorService dataValidator, RightRuleOptions options)
    {
        _operatorService = operatorService;
        _dataService = dataService;
        _dataValidator = dataValidator;
        _options = options;
    }

    public ITokenizer BuildTokenizer(string statement, int pos)
    {
        bool dataCheck = _options.DataCheck;

        return new Tokenizer(_operatorService, _dataService, _dataValidator, statement, pos, dataCheck);
    }
}
