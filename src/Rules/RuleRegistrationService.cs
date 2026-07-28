using RightRule.StatementProcessor;
using RightRule.System.Common.Exceptions;

namespace RightRule.Rules;

internal class RuleRegistrationService
{
    internal async static Task<List<ParsedRule>> ParseRulesAsync(IEnumerable<RuleDefinition> rules, HashSet<string> OperatorNames)
    {
        var parsedRules = new List<ParsedRule>();
        foreach (var rule in rules)
        {
            var registeredRule = await ParseRuleAsync(rule, OperatorNames).ConfigureAwait(false);
            if (registeredRule != null)
            {
                parsedRules.Add(registeredRule);
            }
        }
        return parsedRules;
    }

    internal async static Task<ParsedRule> ParseRuleAsync(RuleDefinition rule, HashSet<string> OperatorNames)
    {
        var parsedRule = await CreateRuleAsync(rule, OperatorNames).ConfigureAwait(false);
        return parsedRule;
    }

    private async static Task<ParsedRule> CreateRuleAsync(RuleDefinition ruleDefinition, HashSet<string> OperatorNames)
    {
        var statement = ruleDefinition.Statement;

        Node node = await GenerateTreeAsync(statement, OperatorNames).ConfigureAwait(false);
        ParsedRule rule = new ParsedRule(ruleDefinition, node);
        return rule;
    }

    private async static Task<Node> GenerateTreeAsync(string Statement, HashSet<string> OperatorNames)
    {
        var tokenizer = new Tokenizer(Statement, OperatorNames);
        var parser = new Parser(tokenizer);

        try
        {
            Node node = await parser.ParseAsync().ConfigureAwait(false);
            return node;
        }
        catch (Exception ex)
        {
            throw new FormatException($"The Statement string is invalid - {Statement}", ex);
        }
    }
}
