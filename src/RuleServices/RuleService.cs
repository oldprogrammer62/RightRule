using Microsoft.Extensions.Logging;
using RightRule.DataTransformer;
using RightRule.LanguageProcessor.Evaluator;
using RightRule.Models;
using RightRule.RightRuleService;
using RightRule.RuleServices.DefaultRuleRegistration;
using RightRule.RuleServices.RuleApplication;
using RightRule.RuleServices.RuleBuilders.DefaultRuleBuilder;
using RightRule.RuleServices.RuleLoader;
using RightRule.RuleServices.RuleModels;
using RightRule.RuleServices.RuleModels.BaseModels;
using RightRule.RuleServices.RuleParsingService;
using RightRule.RuleServices.RuleRegistration;
using RightRule.RuleServices.Rules;
using RightRule.RuleServices.RuleSerializer;
using RightRule.RuleServices.RuleValidation;
using System.ComponentModel;
using System.Data;

namespace RightRule.RuleServices;

internal sealed class RuleService : IRuleService
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

    public List<RightRuleResult<IProcessedRule>> ApplyRules(string Context)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(Context, nameof(Context));

        try
        {
            return ApplyRulesAsync(Context, EvaluationTypeEnum.FIRST).GetAwaiter().GetResult();
        }
        catch (Exception ex)
        {
            return [RightRuleResult<IProcessedRule>.ErrorResult(ex, ErrorRule.ErrorResult(ex.Message))];
        }
    }

    /// <summary>
    /// Apply all Registered Rules for the given Context to the registered data set(s).<br/>
    /// The first Rule to return a positive (true) result will be returned.<br/>
    /// If no rule returns a positive result, a default rule will be returned.
    /// </summary>
    /// <param name="Context">Used to identify the set of rules to apply</param>
    /// <returns>
    /// A single Applied Rule is returned.<br />
    /// If a rule returns true, that rule is returned.<br />
    /// If no rule returns true, a default rule is returned.<br />
    /// </returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="KeyNotFoundException"></exception>
    /// <exception cref="Exception"></exception>
    public RightRuleResult<IProcessedRule> ApplyRule(string Context)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(Context, nameof(Context));

        try
        {
            return ApplyRuleAsync(Context).GetAwaiter().GetResult();
        }
        catch (Exception ex)
        {
            return RightRuleResult<IProcessedRule>.ErrorResult(ex, ErrorRule.ErrorResult(ex.Message));
        }
    }

    /// <summary>
    /// Apply all Registered Rules for the given Context to the registered data set(s).<br/>
    /// The first Rule to return a positive (true) result will be returned.<br/>
    /// If no rule returns a positive result, a default rule will be returned.
    /// </summary>
    /// <param name="Context">Used to identify the set of rules to apply</param>
    /// <returns>
    /// A single Applied Rule is returned.<br />
    /// If a rule returns true, that rule is returned.<br />
    /// If no rule returns true, a default rule is returned.<br />
    /// </returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="KeyNotFoundException"></exception>
    /// <exception cref="Exception"></exception>
    public async Task<RightRuleResult<IProcessedRule>> ApplyRuleAsync(string Context)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(Context, nameof(Context));

        try
        {
            var svcResult = await ApplyRulesAsync_SVC(Context).ConfigureAwait(false);
            if (svcResult == null)
            {
                return RightRuleResult<IProcessedRule>.DefaultResult(GetDefaultRule(Context));
            }
            return svcResult;
        }
        catch (Exception ex)
        {
            return RightRuleResult<IProcessedRule>.ErrorResult(ex, ErrorRule.ErrorResult(ex.Message));
        }
    }

    /// <summary>
    /// Apply all Registered Rules for the given Context to the registered data set(s).<br/>
    /// The first Rule to return a positive (true) result will be returned.<br/>
    /// If no rule returns a positive result, a default rule will be returned.
    /// </summary>
    /// <param name="Context">Used to identify the set of rules to apply</param>
    /// <returns>
    /// A single Applied Rule is returned.<br />
    /// If a rule returns true, that rule is returned.<br />
    /// If no rule returns true, a default rule is returned.<br />
    /// </returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="KeyNotFoundException"></exception>
    /// <exception cref="Exception"></exception>
    //public List<RightRuleResult<IProcessedRule>> ApplyRules(string Context)
    //{
    //    ArgumentException.ThrowIfNullOrWhiteSpace(Context, nameof(Context));

    //    try
    //    {
    //        return ApplyRulesAsync(Context, EvaluationTypeEnum.FIRST).GetAwaiter().GetResult();
    //    }
    //    catch (Exception ex)
    //    {
    //        return [RightRuleResult<IProcessedRule>.ErrorResult(ex, ErrorRule.ErrorResult(ex.Message))];
    //    }
    //}

    /// <summary>
    /// Apply all Registered Rules for the given Context to the registered data set(s).<br/>
    /// The first Rule to return a positive (true) result will be returned.<br/>
    /// If no rule returns a positive result, a default rule will be returned.
    /// </summary>
    /// <param name="Context">Used to identify the set of rules to apply</param>
    /// <returns>
    /// A single Applied Rule is returned.<br />
    /// If a rule returns true, that rule is returned.<br />
    /// If no rule returns true, a default rule is returned.<br />
    /// </returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="KeyNotFoundException"></exception>
    /// <exception cref="Exception"></exception>
    public async Task<List<RightRuleResult<IProcessedRule>>> ApplyRulesAsync(string Context)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(Context, nameof(Context));

        try
        {
            var svcResult = await ApplyRulesAsync_SVC(Context, EvaluationTypeEnum.FIRST).ConfigureAwait(false);
            if (svcResult == null)
            {
                return [RightRuleResult<IProcessedRule>.DefaultResult(GetDefaultRule(Context))];
            }
            return svcResult;
        }
        catch (Exception ex)
        {
            return [RightRuleResult<IProcessedRule>.ErrorResult(ex, ErrorRule.ErrorResult(ex.Message))];
        }
    }

    /// <summary>
    /// Apply all Registered Rules for the given Context to the registered data set(s).<br/>
    /// The first Rule to return a positive (true) result will be returned.<br/>
    /// If no rule returns a positive result, a default rule will be returned.
    /// </summary>
    /// <param name="Context">Used to identify the set of rules to apply</param>
    /// <param name="EvaluationType"></param>
    /// <returns>
    /// A single Applied Rule is returned.<br />
    /// If a rule returns true, that rule is returned.<br />
    /// If no rule returns true, a default rule is returned.<br />
    /// </returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="KeyNotFoundException"></exception>
    /// <exception cref="Exception"></exception>
    public List<RightRuleResult<IProcessedRule>> ApplyRules(string Context, EvaluationTypeEnum EvaluationType)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(Context, nameof(Context));

        try
        {
            return ApplyRulesAsync(Context, EvaluationType).GetAwaiter().GetResult();
        }
        catch (Exception ex)
        {
            return [RightRuleResult<IProcessedRule>.ErrorResult(ex, ErrorRule.ErrorResult(ex.Message))];
        }
    }

    /// <summary>
    /// Apply all Registered Rules for the given Context to the registered data set(s).<br/>
    /// The first Rule to return a positive (true) result will be returned.<br/>
    /// If no rule returns a positive result, a default rule will be returned.
    /// </summary>
    /// <param name="Context">Used to identify the set of rules to apply</param>
    /// <param name="EvaluationType"></param>
    /// <returns>
    /// A single Applied Rule is returned.<br />
    /// If a rule returns true, that rule is returned.<br />
    /// If no rule returns true, a default rule is returned.<br />
    /// </returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="KeyNotFoundException"></exception>
    /// <exception cref="Exception"></exception>
    public async Task<List<RightRuleResult<IProcessedRule>>> ApplyRulesAsync(string Context, EvaluationTypeEnum EvaluationType)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(Context, nameof(Context));

        try
        {
            var svcResult = await ApplyRulesAsync_SVC(Context, EvaluationType).ConfigureAwait(false);
            if (svcResult == null)
            {
                return [RightRuleResult<IProcessedRule>.DefaultResult(GetDefaultRule(Context))];
            }
            return svcResult;
        }
        catch (Exception ex)
        {
            return [RightRuleResult<IProcessedRule>.ErrorResult(ex, ErrorRule.ErrorResult(ex.Message))];
        }
    }

    private async Task<RightRuleResult<IProcessedRule>> ApplyRulesAsync_SVC(string Context)
    {
        try
        {
            var rulesApplied = await ApplyRules_SingleResultAsync(Context, EvaluationTypeEnum.FIRST);
            return BuildResult(Context, rulesApplied);
        }
        catch (Exception ex)
        {
            return RightRuleResult<IProcessedRule>.ErrorResult(ex, ErrorRule.ErrorResult(ex.Message));
        }
    }

    private async Task<List<RightRuleResult<IProcessedRule>>> ApplyRulesAsync_SVC(string Context, EvaluationTypeEnum EvaluationType)
    {
        try
        {
            var rulesApplied = await ApplyRules_MultipleResultAsync(Context, EvaluationType);
            return BuildResults(Context, rulesApplied);
        }
        catch (Exception ex)
        {
            return [RightRuleResult<IProcessedRule>.ErrorResult(ex, ErrorRule.ErrorResult(ex.Message))];
        }
    }

    public async Task<RightRuleServiceResult<IProcessedRule>> ApplyRules_SingleResultAsync(string Context, EvaluationTypeEnum EvaluationType)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(Context, nameof(Context));

        var rulesApplied = await ApplyRules_MultipleResultAsync(Context, EvaluationType).ConfigureAwait(false);
        if (rulesApplied == null || rulesApplied.Count == 0)
        {
            return BuildDefaultResult(Context);
        }
        var result = rulesApplied.FirstOrDefault();
        if (result != null)
        {
            return result;
        }
        return BuildDefaultResult(Context);
    }

    public async Task<List<RightRuleServiceResult<IProcessedRule>>> ApplyRules_MultipleResultAsync(string Context, EvaluationTypeEnum EvaluationType)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(Context, nameof(Context));

        var rulesApplied = await _ruleApplicationService.ApplyRulesAsync(Context, EvaluationType).ConfigureAwait(false);
        if (rulesApplied == null)
        {
            return [BuildDefaultResult(Context)];
        }
        return BuildResult(rulesApplied);
    }

    public IDefaultNameStep BuildDefaultRule()
    {
        return DefaultRuleBuilder.Create();
    }

    public DefaultRuleRegistrationResult RegisterDefaultRule(DefaultRule Rule)
    {
        if (Rule == null)
        {
            return DefaultRuleRegistrationResult.Failed("Default Rule registration failed");
        }
        var result = _defaultRuleRegister.SetDefaultRule(Rule);
        if (!result)
        {
            return DefaultRuleRegistrationResult.Failed("Default Rule registration failed", Rule);
        }
        return DefaultRuleRegistrationResult.Success("Default Rule registration success", Rule);
    }

    public DefaultRule GetDefaultRule(string Context)
    {
        return _defaultRuleRegister.GetDefaultRule(Context);
    }

    public string SerializeRules()
    {
        var rules = GetRegisteredRules();
        if (rules == null || rules.Count == 0)
        {
            return string.Empty;
        }
        return _ruleSerializerService.SerializeRules(rules);
    }

    public string SerializeRules(string Context)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(Context, nameof(Context));

        var rules = GetRegisteredRules(Context);
        if (rules == null || rules.Count == 0)
        {
            return string.Empty;
        }
        return _ruleSerializerService.SerializeRules(rules);
    }

    public List<RuleRegistrationResult> LoadRules(string JsonData)
    {
        ArgumentNullException.ThrowIfNullOrWhiteSpace(JsonData, nameof(JsonData));

        var ruleDef = _ruleLoaderService.LoadRules(JsonData);
        if (ruleDef == null)
        {
            return [];
        }
        return RegisterRules(ruleDef);
    }

    public ParsedRule? GetRegisteredRule(string Context, string RuleName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(Context, nameof(Context));
        ArgumentException.ThrowIfNullOrWhiteSpace(RuleName, nameof(RuleName));

        return _registeredRules.Rules.FirstOrDefault(r =>
        r.Context.Equals(Context, StringComparison.OrdinalIgnoreCase)
        &&
        string.Equals(r.Name, RuleName, StringComparison.OrdinalIgnoreCase));
    }

    public ParsedRule? GetRegisteredRule(string Context, int RuleNumber)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(Context, nameof(Context));

        return _registeredRules.Rules.FirstOrDefault(r => 
        r.Context.Equals(Context, StringComparison.OrdinalIgnoreCase)
        &&
        r.Number == RuleNumber);
    }

    public List<ParsedRule> GetRegisteredRules()
    {
        return _registeredRules.Rules;
    }

    public List<ParsedRule> GetRegisteredRules(string Context)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(Context, nameof(Context));

        return _registeredRules.Rules.Where(r => r.Context.Equals(Context, StringComparison.OrdinalIgnoreCase)).ToList();
    }

    public List<RuleRegistrationResult> RegisterRules(IEnumerable<DefinedRule> Rules)
    {
        ArgumentNullException.ThrowIfNull(Rules, nameof(Rules));

        List<RuleRegistrationResult> registeredRules = [];
        foreach (var rule in Rules)
        {
            var ruleRegistration = RegisterRule(rule);
            if (ruleRegistration.Status == RightRuleStatusEnum.SUCCESS)
            {
                registeredRules.Add(ruleRegistration);
                continue;
            }
            registeredRules.Add(ruleRegistration);
        }
        return registeredRules;
    }

    public RuleRegistrationResult RegisterRule(DefinedRule Rule)
    {
        ArgumentNullException.ThrowIfNull(Rule, nameof(Rule));

        var ruleValidationResult = ValidateRule(Rule);
        if (ruleValidationResult.Valid == RightRuleStatusEnum.ERROR)
        {
            if (_logger.IsEnabled(LogLevel.Error))
            {
                _logger.LogError("Rule: {number} - {name} not registered. {message}", Rule.Number, Rule.Name, ruleValidationResult.Message);
            }
            return new RuleRegistrationResult()
            {
                RegisteredRule = null,
                Message = ruleValidationResult.Message,
                Status = ruleValidationResult.Valid
            };
        }
        var parsedRule = ParseRule(Rule);
        if (parsedRule == null)
        {
            return new RuleRegistrationResult()
            {
                RegisteredRule = null,
                Message = "Error: Rule Failed to be Parsed",
                Status = RightRuleStatusEnum.ERROR
            };
        }
        if (parsedRule.Status == RightRuleStatusEnum.SUCCESS)
        {
            return RegisteredRules.AddRule(parsedRule);
        }
        if (parsedRule.Status == RightRuleStatusEnum.DEFAULT)
        {
            return RegisteredRules.AddRule(parsedRule);
        }
        if (parsedRule.Status == RightRuleStatusEnum.FAILURE)
        {
            return new RuleRegistrationResult()
            {
                RegisteredRule = parsedRule,
                Message = parsedRule.Message ?? "Registration failed, reason unknown!",
                Status = parsedRule.Status
            };
        }
        if (parsedRule.Status == RightRuleStatusEnum.ERROR)
        {
            return new RuleRegistrationResult()
            {
                RegisteredRule = parsedRule,
                Message = parsedRule.Message ?? "Error: Registration failed, reason unknown!",
                Status = parsedRule.Status
            };
        }
        return new RuleRegistrationResult()
        {
            RegisteredRule = null,
            Message = "Rule Failed to register",
            Status = RightRuleStatusEnum.FAILURE
        };
    }

    public bool DeRegisterRules(string Context)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(Context, nameof(Context));
        return RegisteredRules.RemoveRules(Context);
    }

    public bool DeRegisterRules()
    {
        return RegisteredRules.RemoveRules();
    }

    public bool DeRegisterRule(string Context, string RuleName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(Context, nameof(Context));
        ArgumentException.ThrowIfNullOrWhiteSpace(RuleName, nameof(RuleName));

        return RegisteredRules.RemoveRule(Context, RuleName);
    }

    public bool DeRegisterRule(string Context, int RuleNumber)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(Context, nameof(Context));
        return RegisteredRules.RemoveRule(Context, RuleNumber);
    }

    public List<string>? GetRuleContexts()
    {
        return RegisteredRules.GetRuleContexts();
    }

    public List<(int Number, string Name)>? GetRuleNumbers(string Context)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(Context, nameof(Context));
        return RegisteredRules.GetRuleNumbers(Context);
    }

    public List<string>? GetRuleNames(string Context)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(Context, nameof(Context));
        return RegisteredRules.GetRuleNames(Context);
    }

    private RuleValidationResult ValidateRule(DefinedRule Rule)
    {
        return _ruleValidationService.ValidateRule(Rule);
    }

    private ParsedRule? ParseRule(DefinedRule Rule)
    {
        var diagnostics = _options.Diagnostics;
        return _parseRuleService.ParseRule(Rule, diagnostics);
    }

    private RightRuleServiceResult<IProcessedRule> BuildDefaultResult(string Context)
    {
        var rule = RightRuleServiceResult<IProcessedRule>.DefaultResult(GetDefaultRule(Context));

        var decisionData = _interpolatorService.Interpolate(rule.DecisionData);
        rule.DecisionData = decisionData;
        return rule;
    }

    private List<RightRuleResult<IProcessedRule>> BuildResults(string Context, List<RightRuleServiceResult<IProcessedRule>> ServiceResults)
    {
        if (ServiceResults == null)
        {
            return [RightRuleResult<IProcessedRule>.DefaultResult(GetDefaultRule(Context))];
        }
        if (ServiceResults.Count == 0)
        {
            return [RightRuleResult<IProcessedRule>.DefaultResult(GetDefaultRule(Context))];
        }
        List<RightRuleResult<IProcessedRule>> rightRuleResults = new List<RightRuleResult<IProcessedRule>>();

        foreach (var serviceResult in ServiceResults)
        {
            rightRuleResults.Add(BuildResult(Context, serviceResult));
        }
        return rightRuleResults;
    }

    private RightRuleResult<IProcessedRule> BuildResult(string Context, RightRuleServiceResult<IProcessedRule> ServiceResult)
    {
        var status = ServiceResult?.Status ?? RightRuleStatusEnum.DEFAULT;

        var payload = ServiceResult?.Payload ?? null;
        var message = ServiceResult?.Message ?? string.Empty;
        var statementApplied = ServiceResult?.Payload?.StatementApplied ?? string.Empty;
        var decisionData = ServiceResult?.DecisionData ?? string.Empty;

        return status switch
        {
            RightRuleStatusEnum.SUCCESS when ServiceResult?.Payload != null =>
                RightRuleResult<IProcessedRule>.SuccessResult(payload, statementApplied, decisionData),

            RightRuleStatusEnum.FAILURE =>
                RightRuleResult<IProcessedRule>.FailureResult(payload, statementApplied, decisionData, message),

            RightRuleStatusEnum.DEFAULT =>
                RightRuleResult<IProcessedRule>.DefaultResult(GetDefaultRule(Context), message),

            RightRuleStatusEnum.ERROR =>
                RightRuleResult<IProcessedRule>.ErrorResult(ServiceResult?.Exception ?? new Exception(message), payload != null ? payload : ErrorRule.ErrorResult(message)),

            _ => RightRuleResult<IProcessedRule>.DefaultResult(GetDefaultRule(Context))
        };
    }

    private List<RightRuleServiceResult<IProcessedRule>> BuildResult(List<IProcessedRule> RulesApplied)
    {
        List<RightRuleServiceResult<IProcessedRule>> results = new List<RightRuleServiceResult<IProcessedRule>>();
        foreach (var ruleApplied in RulesApplied)
        {
            var decisionData = MakeDecisionData(ruleApplied);
            results.Add(BuildResultObject(ruleApplied, decisionData));
        }
        return results;
    }

    private static RightRuleServiceResult<IProcessedRule> BuildResultObject(IProcessedRule processedRule, object? DecisionData)
    {
        var result = processedRule.Result switch
        {
            true => RightRuleServiceResult<IProcessedRule>.SuccessResult(processedRule, DecisionData),
            false => RightRuleServiceResult<IProcessedRule>.FailureResult(processedRule, DecisionData)
        };
        return result;
    }

    private object? MakeDecisionData(IProcessedRule rule)
    {
        var decisionData = rule.Result switch
        {
            true => _interpolatorService.Interpolate(rule.TrueDecisionData),
            false => _interpolatorService.Interpolate(rule.FalseDecisionData)
        };
        return decisionData;
    }
}
