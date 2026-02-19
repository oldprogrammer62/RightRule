using RightRule.HelperFunctions;
using RightRule.Interpreter;
using RightRule.Operators;
using RightRule.Rules;

namespace RightRule;

public class RightRule
{
    private readonly List<IOperator> _operators;
    private readonly Rules.Rules _rules;

    public RightRule()
    {
        _rules = new Rules.Rules();
        _operators = GetOperators();
    }

    public Rule RegisterRule(RawRule rawRule)
    {
        var rule = CreateRule(rawRule);
        _rules.AddRule(rule);
        return rule;
    }

    public Rule ApplyRules(string Context, Dictionary<string, object> Data)
    {
        var rulesToApply = Helpers.FilterRules(Context, _rules.RightRules);

        var evaluator = new Evaluator(Data, _operators);
        return evaluator.Evaluate(rulesToApply);
    }

    private Rule CreateRule(RawRule rawRule)
    {
        var operatorNames = _operators.Select(o => o.Name);
        var statement = rawRule.Statement;

        Node node = GenerateTree(statement, operatorNames);
        Rule rule = new Rule()
        {
            Number = rawRule.Number,
            Name = rawRule.Name,
            Context = rawRule.Context,
            Node = node,
            Decision= rawRule.Decision,
            DecisionData = rawRule.DecisionData
        };
        return rule;
    }

    private Node GenerateTree(string Statement, IEnumerable<string> OperatorNames)
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
