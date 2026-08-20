using Microsoft.Extensions.DependencyInjection;
using RightRule.StatementProcessor.Evaluator;
using RightRule.StatementProcessor.Interpreter;
using RightRule.System.Common;

namespace RightRule.Factories.RightFactory;

public class RightRuleFactory : IRightRuleFactory
{
    public IEvaluator CreateEvaluator(IServiceProvider serviceProvider, Dictionary<string, Operator> Operators)
    {
        return ActivatorUtilities.CreateInstance<Evaluator>(serviceProvider, Operators);
    }

    public IInterpreter CreateInterpreter(IServiceProvider serviceProvider)
    {
        return ActivatorUtilities.CreateInstance<Interpreter>(serviceProvider);
    }
}
