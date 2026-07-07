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
            if (registeredRule != null)
            {
                parsedRules.Add(registeredRule);
            }
        }
        return parsedRules;
    }

    internal static ParsedRule? ParseRule(RuleDefinition rule, HashSet<string> OperatorNames)
    {
        var parsedRule = CreateRule(rule, OperatorNames);
        return parsedRule;
    }

    private static ParsedRule? CreateRule(RuleDefinition ruleDefinition, HashSet<string> OperatorNames)
    {
        var statement = ruleDefinition.Statement;

        Node? node = GenerateTree(statement, OperatorNames);
        if (node == null)
        {
            return default;
        }
        ParsedRule rule = new ParsedRule(ruleDefinition, node);
        return rule;
    }

    private static Node? GenerateTree(string Statement, HashSet<string> OperatorNames)
    {
        var tokenizer = new Tokenizer(Statement, OperatorNames);
        var parser = new Parser(tokenizer);

        try
        {
            Node node = parser.Parse();
            return node;
        }
        catch
        {
            return default;
        }
    }
}
