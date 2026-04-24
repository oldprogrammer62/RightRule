using RightRule.Interpreter;

namespace RightRule.Rules;

internal class RuleRegistrationService
{
    internal static List<ParsedRule> ParseRules(IEnumerable<RuleDefinition> rules, IEnumerable<List<string>> operators)
    {
        var parsedRules = new List<ParsedRule>();
        foreach (var rule in rules)
        {
            var registeredRule = ParseRule(rule, operators);
            parsedRules.Add(registeredRule);
        }
        return parsedRules;
    }

    internal static ParsedRule ParseRule(RuleDefinition rule, IEnumerable<List<string>> operators)
    {
        var parsedRule = CreateRule(rule, operators);
        return parsedRule;
    }

    private static ParsedRule CreateRule(RuleDefinition rule, IEnumerable<List<string>> operators)
    {
        var statement = rule.Statement;

        Node node = GenerateTree(statement, operators);
        ParsedRule pRule = new ParsedRule(rule, node);
        return pRule;
    }

    private static Node GenerateTree(string Statement, IEnumerable<List<string>> OperatorNames)
    {
        var tokenizer = new Tokenizer(Statement, OperatorNames);
        var parser = new Parser(tokenizer);
        Node node = parser.Parse();
        return node;
    }
}
