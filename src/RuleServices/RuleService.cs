using Microsoft.Extensions.Logging;
using RightRule.DataTransformer;
using RightRule.Models;
using RightRule.RuleServices.DefaultRuleRegistration;
using RightRule.RuleServices.RuleApplication;
using RightRule.RuleServices.RuleLoader;
using RightRule.RuleServices.RuleParsingService;
using RightRule.RuleServices.Rules;
using RightRule.RuleServices.RuleSerializer;
using RightRule.RuleServices.RuleValidation;

namespace RightRule.RuleServices;

internal sealed partial class RuleService : IRuleService
{
    private readonly IRegisteredRules _registeredRules;
    public IRegisteredRules RegisteredRules => _registeredRules;

    private readonly RightRuleOptions _options;
    private readonly IRuleApplicationService _ruleApplicationService;
    private readonly IRuleValidationService _ruleValidationService;
    private readonly IParseRuleService _parseRuleService;
    private readonly IRuleLoaderService _ruleLoaderService;
    private readonly IRuleSerializerService _ruleSerializerService;
    private readonly IDefaultRuleRegister _defaultRuleRegister;
    private readonly IInterpolatorService _interpolatorService;
    private readonly ILogger<RuleService> _logger;

    public RuleService(RightRuleOptions options, IRuleApplicationService ruleApplicationService, IRuleValidationService ruleValidationService, IParseRuleService parseRuleService, IRegisteredRules registeredRules, IRuleLoaderService ruleLoaderService, IRuleSerializerService ruleSerializerService, IDefaultRuleRegister defaultRuleRegister, IInterpolatorService interpolatorService, ILogger<RuleService> logger)
    {
        _logger = logger;

        _options = options;
        _ruleApplicationService = ruleApplicationService;
        _registeredRules = registeredRules;
        _ruleLoaderService = ruleLoaderService;
        _ruleSerializerService = ruleSerializerService;
        _defaultRuleRegister = defaultRuleRegister;
        _interpolatorService = interpolatorService;
        _ruleValidationService = ruleValidationService;
        _parseRuleService = parseRuleService;
    }
}
