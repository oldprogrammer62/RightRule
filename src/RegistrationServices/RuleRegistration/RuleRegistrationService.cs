using RightRule.LanguageProcessor.Parser;
using RightRule.Models.RuleModels;

namespace RightRule.RegistrationServices.RuleRegistration;

internal class RuleRegistrationService : IRuleRegistrationService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IParser _parser;

    public RuleRegistrationService(IServiceProvider ServiceProvider, IParser parser)
    {
        _serviceProvider = ServiceProvider;
        _parser = parser;
    }

    public List<ParsedRule> RegisterRules(IEnumerable<DefinedRule> rules, HashSet<string> OperatorNames)
    {
        return ParseRulesAndRegister(rules, OperatorNames);
    }

    public ParsedRule? RegisterRule(DefinedRule rule, HashSet<string> OperatorNames)
    {
        return ParseRuleAndRegister(rule, OperatorNames);
    }

    private List<ParsedRule> ParseRulesAndRegister(IEnumerable<DefinedRule> rules, HashSet<string> OperatorNames)
    {
        var parsedRules = new List<ParsedRule>();
        foreach (var rule in rules)
        {
            var registeredRule = RegisterRule(rule, OperatorNames);
            if (registeredRule != null)
            {
                parsedRules.Add(registeredRule);
            }
        }
        return parsedRules;
    }

    private ParsedRule? ParseRuleAndRegister(DefinedRule rule, HashSet<string> OperatorNames)
    {
        var parsedRule = CreateRule(rule, OperatorNames);
        return parsedRule;
    }

    private ParsedRule? CreateRule(DefinedRule? ruleDefinition, HashSet<string> OperatorNames)
    {
        if (_parser == null)
        {
            return default;
        }
        if (ruleDefinition == null)
        {
            return default;
        }

        try
        {
            _parser.Statement = ruleDefinition.Statement;

            Node node = GenerateTree(_parser);
            ParsedRule? rule = ParsedRule.SuccessResult(ruleDefinition, node, message: "Parsing succeeded");
            return rule;
        }
        catch (Exception ex)
        {
            return ParsedRule.ErrorResult(ruleDefinition, message: ex.Message);
        }
    }

    private static Node GenerateTree(IParser parser)
    {
        Node node = parser.Parse();
        return node;
    }
}
