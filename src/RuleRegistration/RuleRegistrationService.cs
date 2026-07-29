using RightRule.RuleModels;
using RightRule.StatementProcessor;
using RightRule.System.Common.Exceptions;

namespace RightRule.RuleRegistration;

internal class RuleRegistrationService
{
    internal static List<ParsedRule> RegisterRules(IEnumerable<RuleDefinition> rules, HashSet<string> OperatorNames)
    {
        return ParseRulesAndRegister(rules, OperatorNames);
    }

    internal static ParsedRule RegisterRule(RuleDefinition rule, HashSet<string> OperatorNames)
    {
        return ParseRuleAndRegister(rule, OperatorNames);
    }

    private static List<ParsedRule> ParseRulesAndRegister(IEnumerable<RuleDefinition> rules, HashSet<string> OperatorNames)
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

    private static ParsedRule ParseRuleAndRegister(RuleDefinition rule, HashSet<string> OperatorNames)
    {
        var parsedRule = CreateRule(rule, OperatorNames);
        return parsedRule;
    }

    private static ParsedRule CreateRule(RuleDefinition ruleDefinition, HashSet<string> OperatorNames)
    {
        var statement = ruleDefinition.Statement;

        Node node = GenerateTree(statement, OperatorNames);
        ParsedRule rule = new ParsedRule(ruleDefinition, node);
        return rule;
    }

    private static Node GenerateTree(string Statement, HashSet<string> OperatorNames)
    {
        var tokenizer = new Tokenizer(Statement, OperatorNames);
        var parser = new Parser(tokenizer);

        try
        {
            Node node = parser.Parse();
            return node;
        }
        catch (Exception ex)
        {
            throw new FormatException($"The Statement string is invalid - {Statement}", ex);
        }
    }
}
