using System.Runtime.Loader;

namespace RightRule.Loaders.OperatorLoader;

public class OperatorLoader : AssemblyLoadContext
{
    private readonly AssemblyDependencyResolver _resolver;

    internal OperatorLoader(string functionPath, bool collectible)
        : base(name: Path.GetFileName(functionPath), collectible)
    {
        if (string.IsNullOrWhiteSpace(functionPath) || !File.Exists(functionPath))
            throw new FileNotFoundException("Function assembly path not found", functionPath);

        _resolver = new AssemblyDependencyResolver(functionPath);
    }
}
