using RightRule.Factories.ParserFactory;
using RightRule.LanguageProcessor.Parser;
using RightRule.Models;
using RightRule.RuleServices.RuleModels;
using RightRule.RuleServices.RuleModels.BaseModels;

namespace RightRule.RuleServices.RuleParsingServices;

internal sealed class ParseRuleService : IParseRuleService
{
    private readonly IParserFactory _parserFactory;
    private readonly RightRuleOptions _options;
    private IParser? _parser = default;

    public ParseRuleService(IParserFactory ParserFactory, RightRuleOptions options)
    {
        _parserFactory = ParserFactory;
        _options = options;
    }

    public List<ParsedRule> ParseRules(IEnumerable<DefinedRule> Rules, bool Diagnostics)
    {
        return CreateParsedRules(Rules, Diagnostics);
    }

    public ParsedRule? ParseRule(DefinedRule Rule, bool Diagnostics)
    {
        return CreateParsedRule(Rule, Diagnostics);
    }

    private List<ParsedRule> CreateParsedRules(IEnumerable<DefinedRule> Rules, bool Diagnostics)
    {
        var parsedRules = new List<ParsedRule>();
        foreach (var rule in Rules)
        {
            var registeredRule = CreateParsedRule(rule, Diagnostics);
            if (registeredRule != null)
            {
                parsedRules.Add(registeredRule);
            }
        }
        return parsedRules;
    }

    private ParsedRule? CreateParsedRule(DefinedRule RuleDefinition, bool Diagnostics)
    {
        _parser = BuildParser(RuleDefinition);

        var parsedRule = ProcessRule(RuleDefinition, Diagnostics);
        return parsedRule;
    }

    private ParsedRule? ProcessRule(DefinedRule? RuleDefinition, bool Diagnostics)
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
            var diagnostics = _options.Diagnostics;
            return ParsedRule.SuccessResult(RuleDefinition, node, message: "Parsing succeeded", diagnostics);
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
