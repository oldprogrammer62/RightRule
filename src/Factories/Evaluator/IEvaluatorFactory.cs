using RightRule.StatementProcessor.Evaluator;
using RightRule.System.Common;

namespace RightRule.Factories.Evaluator
{
    public interface IEvaluatorFactory
    {
        IEvaluator Create(Dictionary<string, Operator> Operators);
    }
}