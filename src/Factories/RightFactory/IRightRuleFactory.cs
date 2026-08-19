using RightRule.StatementProcessor.Evaluator;
using RightRule.System.Common;

namespace RightRule.Factories.RightFactory
{
    public interface IRightRuleFactory
    {
        IEvaluator CreateEvaluator(IServiceProvider serviceProvider, Dictionary<string, Operator> Operators);
    }
}