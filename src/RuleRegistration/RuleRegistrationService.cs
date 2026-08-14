using RightRule.Models;
using RightRule.RuleModels;
using RightRule.StatementProcessor.Parser;
using RightRule.StatementProcessor.Tokenizer;
using RightRule.System.Common.Exceptions;

namespace RightRule.RuleRegistration;

internal class RuleRegistrationService
{
    internal static List<ParsedRule> RegisterRules(IEnumerable<RuleDefinition> rules, HashSet<string> OperatorNames, HashSet<string> ObjectNames, HashSet<string> DatasetNames, RightRuleConfiguration rightRuleConfiguration)
    {
        return ParseRulesAndRegister(rules, OperatorNames, ObjectNames, DatasetNames, rightRuleConfiguration);
    }

    internal static ParsedRule RegisterRule(RuleDefinition rule, HashSet<string> OperatorNames, HashSet<string> ObjectNames, HashSet<string> DatasetNames, RightRuleConfiguration rightRuleConfiguration)
    {
        return ParseRuleAndRegister(rule, OperatorNames, ObjectNames, DatasetNames, rightRuleConfiguration);
    }

    private static List<ParsedRule> ParseRulesAndRegister(IEnumerable<RuleDefinition> rules, HashSet<string> OperatorNames, HashSet<string> ObjectNames, HashSet<string> DatasetNames, RightRuleConfiguration rightRuleConfiguration)
    {
        var parsedRules = new List<ParsedRule>();
        foreach (var rule in rules)
        {
            var registeredRule = RegisterRule(rule, OperatorNames, ObjectNames, DatasetNames, rightRuleConfiguration);
            if (registeredRule != null)
            {
                parsedRules.Add(registeredRule);
            }
        }
        return parsedRules;
    }

    private static ParsedRule ParseRuleAndRegister(RuleDefinition rule, HashSet<string> OperatorNames, HashSet<string> ObjectNames, HashSet<string> DatasetNames, RightRuleConfiguration rightRuleConfiguration)
    {
        var parsedRule = CreateRule(rule, OperatorNames, ObjectNames, DatasetNames, rightRuleConfiguration);
        return parsedRule;
    }

    private static ParsedRule CreateRule(RuleDefinition ruleDefinition, HashSet<string> OperatorNames, HashSet<string> ObjectNames, HashSet<string> DatasetNames, RightRuleConfiguration rightRuleConfiguration)
    {
        var statement = ruleDefinition.Statement;

        try
        {
            Node node = GenerateTree(statement, OperatorNames, ObjectNames, DatasetNames, rightRuleConfiguration);
            ParsedRule rule = ParsedRule.SuccessResult(ruleDefinition, node, message: "Parsing succeeded");
            return rule;
        }
        catch (Exception ex)
        {
            return ParsedRule.FailResult(ruleDefinition, message: ex.Message);
        }
    }

    private static Node GenerateTree(string Statement, HashSet<string> OperatorNames, HashSet<string> ObjectNames, HashSet<string> DatasetNames, RightRuleConfiguration rightRuleConfiguration)
    {
        var tokenizer = new Tokenizer(Statement, OperatorNames, ObjectNames, DatasetNames, rightRuleConfiguration);
        var parser = new Parser(tokenizer);

        Node node = parser.Parse();
        return node;
    }
}
