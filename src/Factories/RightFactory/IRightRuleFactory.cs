using RightRule.Data;
using RightRule.StatementProcessor.Evaluator;
using RightRule.StatementProcessor.Interpreter;
using RightRule.System.Common;

namespace RightRule.Factories.RightFactory
{
    public interface IRightRuleFactory
    {
        IEvaluator CreateEvaluator(IServiceProvider serviceProvider, Dictionary<string, Operator> Operators);

        IInterpreter CreateInterpreter(IServiceProvider serviceProvider, IEvaluator evaluator, IRegisteredDatasets registeredDatasets, IRegisteredDataObjects registeredDataObjects, Dictionary<string, Operator> operators);
    }
}