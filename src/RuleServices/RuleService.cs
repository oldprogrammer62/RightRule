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
using System.Diagnostics;

namespace RightRule.RuleServices;

internal sealed partial class RuleService : IRuleService
{
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private readonly IRegisteredRules _registeredRules;
    public IRegisteredRules RegisteredRules => _registeredRules;

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private readonly RightRuleOptions _options;

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private readonly IRuleApplicationService _ruleApplicationService;

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private readonly IRuleValidationService _ruleValidationService;

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private readonly IParseRuleService _parseRuleService;

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private readonly IRuleLoaderService _ruleLoaderService;

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private readonly IRuleSerializerService _ruleSerializerService;

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private readonly IDefaultRuleRegister _defaultRuleRegister;
    public List<DefaultRulePerContext> DefaultRules => _defaultRuleRegister.GetDefaultRules() ?? new List<DefaultRulePerContext>();

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private readonly IInterpolatorService _interpolatorService;

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
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
