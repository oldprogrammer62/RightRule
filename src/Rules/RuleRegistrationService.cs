using RightRule.StatementProcessor;

namespace RightRule.Rules;

internal class RuleRegistrationService
{
    internal static List<ParsedRule> ParseRules(IEnumerable<RuleDefinition> rules, HashSet<string> OperatorNames)
    {
        var parsedRules = new List<ParsedRule>();
        foreach (var rule in rules)
        {
            var registeredRule = ParseRule(rule, OperatorNames);
            parsedRules.Add(registeredRule);
        }
        return parsedRules;
    }

    internal static ParsedRule ParseRule(RuleDefinition rule, HashSet<string> OperatorNames)
    {
        var parsedRule = CreateRule(rule, OperatorNames);
        return parsedRule;
    }

    private static ParsedRule CreateRule(RuleDefinition rule, HashSet<string> OperatorNames)
    {
        var statement = rule.Statement;

        Node node = GenerateTree(statement, OperatorNames);
        ParsedRule pRule = new ParsedRule(rule, node);
        return pRule;
    }

    private static Node GenerateTree(string Statement, HashSet<string> OperatorNames)
    {
        var tokenizer = new Tokenizer(Statement, OperatorNames);
        var parser = new Parser(tokenizer);
        Node node = parser.Parse();
        return node;
    }
}
