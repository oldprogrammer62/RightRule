using RightRule.Factories.RightRuleLanguageFactory;
using RightRule.LanguageProcessor.Parser;
using RightRule.Models.RuleModels;

namespace RightRule.RegistrationServices.RuleRegistration;

internal class RuleRegistrationService : IRuleRegistrationService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILanguageFactory _factory;

    public RuleRegistrationService(IServiceProvider ServiceProvider, ILanguageFactory factory)
    {
        _serviceProvider = ServiceProvider;
        _factory = factory;
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
        IParser? parser = CreateParser(ruleDefinition, OperatorNames);
        if (parser == null)
        {
            return default;
        }

        try
        {
            Node node = GenerateTree(parser);
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

    private IParser? CreateParser(DefinedRule? ruleDefinition, HashSet<string>? operatorNames)
    {
        string statement = string.Empty;
        bool dataCheck = false;
        if (ruleDefinition == null)
        {
            return default;
        }

        statement = ruleDefinition.Statement;
        dataCheck = ruleDefinition.RuleConfiguration?.DataCheck ?? false;
        return _factory.CreateParser(_serviceProvider, statement, operatorNames, dataCheck);
    }
}
