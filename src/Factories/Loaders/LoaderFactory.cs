using BeSoft;
using RightRule.RuleLoaders;
using RightRule.RuleLoaders.Json;

namespace RightRule.Factories.Loaders;

public class LoaderFactory
{
    private readonly IRightRule _rightRule;

    public LoaderFactory(IRightRule rightRule)
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
