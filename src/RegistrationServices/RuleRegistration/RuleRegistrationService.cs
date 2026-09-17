using RightRule.Factories.ParserFactory;
using RightRule.LanguageProcessor.Parser;
using RightRule.Models.BaseModels;
using RightRule.Models.RuleModels;

namespace RightRule.RegistrationServices.RuleRegistration;

internal class RuleRegistrationService : IRuleRegistrationService
{
    private readonly IParserFactory _parserFactory;

    private IParser? _parser = default;

    public RuleRegistrationService(IParserFactory parserFactory)
    {
        _parserFactory = parserFactory;
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

    private ParsedRule? ParseRuleAndRegister(DefinedRule ruleDefinition, HashSet<string> OperatorNames)
    {
        _parser = BuildParser(ruleDefinition);

        var parsedRule = CreateRule(ruleDefinition);
        return parsedRule;
    }

    private ParsedRule? CreateRule(DefinedRule? ruleDefinition)
    {
        if (ruleDefinition == null)
        {
            return default;
        }
        if (_parser == null)
        {
            return default;
        }

        try
        {
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

    private IParser BuildParser(RuleDefinition ruleDefinition)
    {
        return _parserFactory.BuildParser(ruleDefinition);
    }
}
