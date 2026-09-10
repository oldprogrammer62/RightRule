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

    public List<ParsedRule> RegisterRules(IEnumerable<DefinedRule> rules, HashSet<string> OperatorNames, HashSet<string>? ObjectNames, HashSet<string>? DatasetNames)
    {
        return ParseRulesAndRegister(rules, OperatorNames, ObjectNames, DatasetNames);
    }

    public ParsedRule? RegisterRule(DefinedRule rule, HashSet<string> OperatorNames, HashSet<string>? ObjectNames, HashSet<string>? DatasetNames)
    {
        return ParseRuleAndRegister(rule, OperatorNames, ObjectNames, DatasetNames);
    }

    private List<ParsedRule> ParseRulesAndRegister(IEnumerable<DefinedRule> rules, HashSet<string> OperatorNames, HashSet<string>? ObjectNames, HashSet<string>? DatasetNames)
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

    private ParsedRule? ParseRuleAndRegister(DefinedRule rule, HashSet<string> OperatorNames, HashSet<string>? ObjectNames, HashSet<string>? DatasetNames)
    {
        var parsedRule = CreateRule(rule, OperatorNames, ObjectNames, DatasetNames);
        return parsedRule;
    }

    private ParsedRule? CreateRule(DefinedRule? ruleDefinition, HashSet<string> OperatorNames, HashSet<string>? ObjectNames, HashSet<string>? DatasetNames)
    {
        IParser? parser = CreateParser(ruleDefinition, OperatorNames, ObjectNames, DatasetNames);
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

    private IParser? CreateParser(DefinedRule? ruleDefinition, HashSet<string>? OperatorNames, HashSet<string>? DataObjectNames, HashSet<string>? DatasetNames)
    {
        string statement = string.Empty;
        bool dataCheck = false;
        if (ruleDefinition == null)
        {
            return default;
        }

        statement = ruleDefinition.Statement;
        dataCheck = ruleDefinition.RuleConfiguration?.DataCheck ?? false;
        return _factory.CreateParser(_serviceProvider, statement, OperatorNames!, DataObjectNames!, DatasetNames!, dataCheck);
    }
}
