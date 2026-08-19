using Microsoft.Extensions.DependencyInjection;
using RightRule.StatementProcessor.Evaluator;
using RightRule.System.Common;

namespace RightRule.Factories.Evaluator;

public class EvaluatorFactory : IEvaluatorFactory
{
    private readonly IServiceProvider _serviceProvider;

    public EvaluatorFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public IEvaluator Create(Dictionary<string, Operator> Operators)
    {
        // ActivatorUtilities resolves DI dependencies while passing runtime values manually
        return ActivatorUtilities.CreateInstance<IEvaluator>(_serviceProvider, Operators);
    }
}
