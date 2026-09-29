using RightRule.DataServices.DataLoaders.Json;
using RightRule.RuleServices.RuleLoader;

namespace RightRule.Factories.RuleLoaderFactory;

public class RuleLoaderFactory
{
    private readonly IRightRule _rightRule;

    public RuleLoaderFactory(IRightRule rightRule)
    {
        _rightRule = rightRule;
    }

    public IRuleLoader BuildLoader(string LoaderType)
    {
        switch (LoaderType)
        {
            case "JSON":
                return new JsonLoader(_rightRule);
        }
        throw new NotImplementedException($"Not Implemented: {LoaderType}");
    }
}
