using Microsoft.Extensions.DependencyInjection;
using RightRule.StatementProcessor.Evaluator;
using RightRule.System.Common;

namespace RightRule.Factories.RightFactory;

public class RightRuleFactory : IRightRuleFactory
{
    public IEvaluator CreateEvaluator(IServiceProvider serviceProvider, Dictionary<string, Operator> Operators)
    {
        // ActivatorUtilities resolves DI dependencies while passing runtime values manually
        return ActivatorUtilities.CreateInstance<Evaluator>(serviceProvider, Operators);
    }
}
