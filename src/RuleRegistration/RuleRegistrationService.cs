using RightRule.Factories.RightRuleLanguageFactory;
using RightRule.RuleModels;
using RightRule.StatementProcessor.Parser;

namespace RightRule.RuleRegistration;

internal class RuleRegistrationService : IRuleRegistrationService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILanguageFactory _factory;

    public RuleRegistrationService(IServiceProvider ServiceProvider, ILanguageFactory factory)
    {
        _serviceProvider = ServiceProvider;
        _factory = factory;
    }

    public List<ParsedRule> RegisterRules(IEnumerable<RuleDefinition> rules, HashSet<string> OperatorNames, HashSet<string>? ObjectNames, HashSet<string>? DatasetNames)
    {
        return ParseRulesAndRegister(rules, OperatorNames, ObjectNames, DatasetNames);
    }

    public ParsedRule? RegisterRule(RuleDefinition rule, HashSet<string> OperatorNames, HashSet<string>? ObjectNames, HashSet<string>? DatasetNames)
    {
        return ParseRuleAndRegister(rule, OperatorNames, ObjectNames, DatasetNames);
    }

    private List<ParsedRule> ParseRulesAndRegister(IEnumerable<RuleDefinition> rules, HashSet<string> OperatorNames, HashSet<string>? ObjectNames, HashSet<string>? DatasetNames)
    {
        var parsedRules = new List<ParsedRule>();
        foreach (var rule in rules)
        {
            var registeredRule = RegisterRule(rule, OperatorNames, ObjectNames, DatasetNames);
            if (registeredRule != null)
            {
                parsedRules.Add(registeredRule);
            }
        }
        return parsedRules;
    }

    private ParsedRule? ParseRuleAndRegister(RuleDefinition rule, HashSet<string> OperatorNames, HashSet<string>? ObjectNames, HashSet<string>? DatasetNames)
    {
        var parsedRule = CreateRule(rule, OperatorNames, ObjectNames, DatasetNames);
        return parsedRule;
    }

    private ParsedRule? CreateRule(RuleDefinition? ruleDefinition, HashSet<string> OperatorNames, HashSet<string>? ObjectNames, HashSet<string>? DatasetNames)
    {
        IParser parser = CreateParser(ruleDefinition, OperatorNames, ObjectNames, DatasetNames);
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
            return ParsedRule.FailResult(ruleDefinition, message: ex.Message);
        }
    }

    private static Node GenerateTree(IParser parser)
    {
        Node node = parser.Parse();
        return node;
    }

    private IParser CreateParser(RuleDefinition? ruleDefinition, HashSet<string>? OperatorNames, HashSet<string>? DataObjectNames, HashSet<string>? DatasetNames)
    {
        string statement = string.Empty;
        bool dataCheck = false;
        if (ruleDefinition != null)
        {
            statement = ruleDefinition.Statement;
            dataCheck = ruleDefinition.RuleConfiguration?.DataCheck ?? false;
        }
        return _factory.CreateParser(_serviceProvider, statement, OperatorNames!, DataObjectNames!, DatasetNames!, dataCheck);
    }
}
