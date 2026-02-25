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

    public RegisteredRule RegisterRule(Rule rule)
    {
        var pRule = CreateRule(rule);
        _rules.AddRule(pRule);

        return new RegisteredRule(pRule);
    }

    public AppliedRule ApplyRules(string Context, Dictionary<string, object> Data)
    {
        return ApplyRules(Context, Data, RulesToApply.First);
    }

    public AppliedRule ApplyRules(string Context, Dictionary<string, object> Data, RulesToApply rulesToApply)
    {
        var rules = Helpers.FilterRules(Context, _rules.RightRules);

        var evaluator = new Evaluator(Data, _operators);
        return evaluator.Evaluate(rules, rulesToApply);
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

    private List<IOperator> GetOperators()
    {
        return Operators.Operators.GetOperators();
    }
}
