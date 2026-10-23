using System.Runtime.Loader;

namespace RightRule.OperatorServices.OperatorLoader.CustomOperators;

internal sealed class ExternalOperatorLoader : AssemblyLoadContext
{
    private readonly AssemblyDependencyResolver _resolver;

    internal ExternalOperatorLoader(string FunctionPath, bool IsCollectible)
        : base(name: Path.GetFileName(FunctionPath), IsCollectible)
    {
        if (string.IsNullOrWhiteSpace(FunctionPath) || !File.Exists(FunctionPath))
            throw new FileNotFoundException("Function assembly path not found", FunctionPath);

        _resolver = new AssemblyDependencyResolver(FunctionPath);
    }
}
