using Microsoft.Extensions.Logging;
using RightRule.RuleServices.RuleModels;

namespace RightRule.RuleServices.DefaultRuleRegistration;

internal class DefaultRuleRegister : IDefaultRuleRegister
{
    private readonly ILogger<DefaultRuleRegister> _logger;

    private DefaultRule DefaultRule { get; }
    private List<DefaultRulePerContext> DefaultRules {  get; }

    public DefaultRuleRegister(ILogger<DefaultRuleRegister> logger)
    {
        DefaultRule = DefaultRule.DefaultResult();
        DefaultRules = [];
        _logger = logger;
    }

    public List<DefaultRulePerContext>? GetDefaultRules()
    {
        return DefaultRules;
    }

    public DefaultRule GetDefaultRule(string Context)
    {
        if (string.IsNullOrWhiteSpace(Context))
        {
            return DefaultRule;
        }

        var defaultRuleFound = GetRegisteredDefaultRule(Context);
        if (defaultRuleFound != null)
        {
            return defaultRuleFound;
        }
        return DefaultRule;
    }

    public bool SetDefaultRule(DefaultRule defaultRule)
    {
        if (defaultRule == null)
        {
            return false;
        }
        RemoveDefaultRule(defaultRule.Context);
        AddDefaultRule(defaultRule);
        _logger.LogInformation("Default Rule added for Context: {0}", defaultRule.Context);
        return true;
    }

    private void RemoveDefaultRule(string Context)
    {
        if (string.IsNullOrWhiteSpace(Context))
        {
            return;
        }
        var ruleFound = GetRegisteredDefaultRulePerContext(Context);
        if (ruleFound != null)
        {
            _logger.LogInformation($"Default Rule removed for Context: {0}", Context);
            DefaultRules.Remove(ruleFound);
        }
    }

    private DefaultRule GetRegisteredDefaultRule(string Context)
    {
        if (string.IsNullOrWhiteSpace(Context))
        {
            return DefaultRule;
        }
        var result = GetRegisteredDefaultRulePerContext(Context);
        if (result == null)
        {
            return DefaultRule;
        }
        return result.DefaultRule;
    }

    private DefaultRulePerContext? GetRegisteredDefaultRulePerContext(string Context)
    {
        if (string.IsNullOrWhiteSpace(Context))
        {
            return null;
        }
        return DefaultRules.Where
            (x => x.Context.Equals(Context, StringComparison.CurrentCultureIgnoreCase))
            .FirstOrDefault();
    }

    private void AddDefaultRule(DefaultRule defaultRule)
    {
        if (defaultRule == null)
        {
            return;
        }
        DefaultRulePerContext drpc = new()
        {
            Context = defaultRule.Context,
            DefaultRule = defaultRule
        };
        DefaultRules.Add(drpc);
    }
}
