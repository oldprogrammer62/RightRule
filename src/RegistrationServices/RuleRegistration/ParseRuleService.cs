using RightRule.Factories.ParserFactory;
using RightRule.LanguageProcessor.Parser;
using RightRule.Models.BaseModels;
using RightRule.Models.RuleModels;

namespace RightRule.RegistrationServices.RuleRegistration;

internal class ParseRuleService : IParseRuleService
{
    private readonly IParserFactory _parserFactory;

    private IParser? _parser = default;

    public ParseRuleService(IParserFactory ParserFactory)
    {
        _parserFactory = ParserFactory;
    }

    public List<ParsedRule> ParseRules(IEnumerable<DefinedRule> Rules)
    {
        return CreateParsedRules(Rules);
    }

    public ParsedRule? ParseRule(DefinedRule Rule)
    {
        return CreateParsedRule(Rule);
    }

    private List<ParsedRule> CreateParsedRules(IEnumerable<DefinedRule> Rules)
    {
        var parsedRules = new List<ParsedRule>();
        foreach (var rule in Rules)
        {
            var registeredRule = CreateParsedRule(rule);
            if (registeredRule != null)
            {
                parsedRules.Add(registeredRule);
            }
        }
        return parsedRules;
    }

    private ParsedRule? CreateParsedRule(DefinedRule RuleDefinition)
    {
        _parser = BuildParser(RuleDefinition);

        var parsedRule = ProcessRule(RuleDefinition);
        return parsedRule;
    }

    private ParsedRule? ProcessRule(DefinedRule? RuleDefinition)
    {
        if (RuleDefinition == null)
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
            ParsedRule? rule = ParsedRule.SuccessResult(RuleDefinition, node, message: "Parsing succeeded");
            return rule;
        }
        catch (Exception ex)
        {
            return ParsedRule.ErrorResult(RuleDefinition, message: ex.Message);
        }
    }

    private static Node GenerateTree(IParser Parser)
    {
        Node node = Parser.Parse();
        return node;
    }

    private IParser BuildParser(RuleDefinition RuleDefinition)
    {
        return _parserFactory.BuildParser(RuleDefinition);
    }
}
