using RightRule.Models;
using RightRule.RuleModels;
using RightRule.StatementProcessor.Parser;
using RightRule.StatementProcessor.Tokenizer;
using RightRule.System.Common.Exceptions;

namespace RightRule.RuleRegistration;

internal class RuleRegistrationService
{
    internal static List<ParsedRule> RegisterRules(IEnumerable<RuleDefinition> rules, HashSet<string> OperatorNames, HashSet<string> ObjectNames, HashSet<string> DatasetNames)
    {
        return ParseRulesAndRegister(rules, OperatorNames, ObjectNames, DatasetNames);
    }

    internal static ParsedRule RegisterRule(RuleDefinition rule, HashSet<string> OperatorNames, HashSet<string> ObjectNames, HashSet<string> DatasetNames)
    {
        return ParseRuleAndRegister(rule, OperatorNames, ObjectNames, DatasetNames);
    }

    private static List<ParsedRule> ParseRulesAndRegister(IEnumerable<RuleDefinition> rules, HashSet<string> OperatorNames, HashSet<string> ObjectNames, HashSet<string> DatasetNames)
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

    private static ParsedRule ParseRuleAndRegister(RuleDefinition rule, HashSet<string> OperatorNames, HashSet<string> ObjectNames, HashSet<string> DatasetNames)
    {
        var parsedRule = CreateRule(rule, OperatorNames, ObjectNames, DatasetNames);
        return parsedRule;
    }

    private static ParsedRule CreateRule(RuleDefinition ruleDefinition, HashSet<string> OperatorNames, HashSet<string> ObjectNames, HashSet<string> DatasetNames)
    {
        var statement = ruleDefinition.Statement;
        var dataCheck = ruleDefinition.DataCheck ?? false;

        try
        {
            Node node = GenerateTree(statement, OperatorNames, ObjectNames, DatasetNames, dataCheck);
            ParsedRule rule = ParsedRule.SuccessResult(ruleDefinition, node, message: "Parsing succeeded");
            return rule;
        }
        catch (Exception ex)
        {
            return ParsedRule.FailResult(ruleDefinition, message: ex.Message);
        }
    }

    private static Node GenerateTree(string Statement, HashSet<string> OperatorNames, HashSet<string> ObjectNames, HashSet<string> DatasetNames, bool DataCheck)
    {
        var tokenizer = new Tokenizer(Statement, OperatorNames, ObjectNames, DatasetNames, DataCheck);
        var parser = new Parser(tokenizer);

        Node node = parser.Parse();
        return node;
    }
}
