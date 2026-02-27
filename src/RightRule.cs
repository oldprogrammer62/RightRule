using RightRule.HelperFunctions;
using RightRule.Interpreter;
using RightRule.Rules;
using RightRule.System.Common;

namespace RightRule;

public class RightRule
{
    private readonly List<IOperator> _operators;
    private readonly Rules.ParsedRules _rules;

    public RightRule()
    {
        _rules = new Rules.ParsedRules();
        _operators = GetOperators();
    }

    /// <summary>
    /// Takes a Rule Definition and registers it for use.
    /// </summary>
    /// <remarks>
    /// The process of registering rule includes parsing the rule statement
    /// </remarks>
    /// <param name="rule"></param>
    /// <returns>A RegisteredRule object</returns>
    public RegisteredRule RegisterRule(RuleDefinition rule)
    {
        ArgumentNullException.ThrowIfNull(rule);

        var pRule = CreateRule(rule);
        _rules.AddRule(pRule);

        return new RegisteredRule(pRule);
    }

    /// <summary>
    /// Applies the default set of rules to the specified context and data, and returns the result of the first matching
    /// rule.
    /// </summary>
    /// <param name="Context">The context identifier used to determine which rules to apply. Cannot be null or empty.</param>
    /// <param name="Data">A dictionary containing the input data to be evaluated against the rules. Cannot be null.</param>
    /// <returns>An AppliedRule object representing the result of the first rule that matches the provided context and data.</returns>
    public AppliedRule ApplyRules(string Context, Dictionary<string, object> Data)
    {
        return ApplyRules(Context, Data, RulesToApply.First);
    }

    /// <summary>
    /// Applies the specified set of rules to the provided data within the given context and returns the result of the
    /// rule evaluation.
    /// </summary>
    /// <param name="Context">The context identifier used to filter and select applicable rules. Cannot be null or empty.</param>
    /// <param name="Data">A dictionary containing the input data against which the rules are evaluated. Keys represent data field names;
    /// values are the corresponding data objects. Cannot be null.</param>
    /// <param name="RulesToApply">Specifies which rules to apply during evaluation. Determines the subset or type of rules to be executed.</param>
    /// <returns>An AppliedRule object representing the outcome of the rule evaluation, including details about which rules were
    /// applied and their results.</returns>
    public AppliedRule ApplyRules(string Context, Dictionary<string, object> Data, RulesToApply RulesToApply)
    {
        var rules = Helpers.FilterRules(Context, _rules.RightRules);

        var evaluator = new Evaluator(Data, _operators);
        return evaluator.Evaluate(rules, RulesToApply);
    }

    private ParsedRule CreateRule(Rule rule)
    {
        var operatorNames = _operators.Select(o => o.Identifiers);
        var statement = rule.Statement;

        Node node = GenerateTree(statement, operatorNames);
        ParsedRule pRule = new ParsedRule()
        {
            Number = rule.Number,
            Name = rule.Name,
            Statement = rule.Statement,
            Context = rule.Context,
            Node = node,
            Decision= rule.Decision,
            DecisionData = rule.DecisionData
        };
        return pRule;
    }

    private Node GenerateTree(string Statement, IEnumerable<List<string>> OperatorNames)
    {
        var tokenizer = new Tokenizer(Statement, OperatorNames);
        var parser = new Parser(tokenizer);
        Node node = parser.Parse();
        return node;
    }

    private static List<IOperator> GetOperators()
    {
        return Operators.Operators.RegisterOperators();
    }
}
