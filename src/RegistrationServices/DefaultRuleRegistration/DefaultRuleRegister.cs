using RightRule.RuleModels.RuleModels;

namespace RightRule.RegistrationServices.DefaultRuleRegistration;

internal class DefaultRuleRegister : IDefaultRuleRegister
{
    private DefaultRule DefaultRule { get; }
    private List<DefaultRulePerContext> DefaultRules {  get; }

    public DefaultRuleRegister()
    {
        DefaultRule = DefaultRule.DefaultResult();
        DefaultRules = [];
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

    internal class DefaultRulePerContext
    {
        public required DefaultRule DefaultRule { get; init;}
        public required string Context { get; init; }
    }
}
