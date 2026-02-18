using RightRule.Interpreter;
using RightRule.Operators;
using RightRule.Rules;

namespace RightRule;

public class Program
{
    public Rule? Main()
    {
        Rules.Rules rules = new();

        var operators = new List<IOperator>
        {
            new EqualsOperator(),
            new NotEqualsOperator(),
            new ContainsOperator()
        };

        RawRule rr1 = new()
        {
            Number = 1,
            Name = "Age Rule",
            Statement = @"age equals 28",
            Context = "AgeContext",
            Decision = "OLDENOUGH",
        };
        rules.AddRule(CreateRule(rr1, operators));

        RawRule rr2 = new()
        {
            Number = 2,
            Name = "Name Rule",
            Statement = @"name equals ""Johnathan""",
            Context = "AgeContext",
            Decision = "OLDENOUGH",
        };
        rules.AddRule(CreateRule(rr2, operators));

        var data = new Dictionary<string, object>(StringComparer.Ordinal)
        {
            { "name", "Johnathan" },
            { "age", 28 },
            { "role", "nuser" }
        };

        var evaluator = new Evaluator(data, operators);
        return evaluator.Evaluate(rules);
    }

    private Rule CreateRule(RawRule rawRule, List<IOperator> operators)
    {
        var operatorNames = operators.Select(o => o.Name);
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
}
