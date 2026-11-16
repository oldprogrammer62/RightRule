using Microsoft.Extensions.Logging;
using RightRule.Models;
using RightRule.RuleServices.RuleModels;
using RightRule.RuleServices.RuleRegistration;
using RightRule.RuleServices.RuleValidation;

namespace RightRule.RuleServices;

internal sealed partial class RuleService : IRuleService
{
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

    private RuleValidationResult ValidateRule(DefinedRule Rule)
    {
        return _ruleValidationService.ValidateRule(Rule);
    }

    private ParsedRule? ParseRule(DefinedRule Rule)
    {
        var diagnostics = _options.Diagnostics;
        return _parseRuleService.ParseRule(Rule, diagnostics);
    }
}
