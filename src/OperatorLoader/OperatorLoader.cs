using RightRule.System.Common;
using System.Reflection;
using System.Runtime.Loader;

namespace RightRule.OperatorLoader;

public class OperatorLoader : AssemblyLoadContext
{
    readonly AssemblyDependencyResolver _resolver;

    internal OperatorLoader(string functionPath, bool collectible)
        : base(name: Path.GetFileName(functionPath), collectible)
    {
        if (string.IsNullOrWhiteSpace(functionPath) || !File.Exists(functionPath))
            throw new FileNotFoundException("Function assembly path not found", functionPath);

        _resolver = new AssemblyDependencyResolver(functionPath);
    }

    protected override Assembly? Load(AssemblyName assemblyName)
    {
        if (assemblyName is null)
        {
            return null;
        }
        if (assemblyName.Name == typeof(IOperator).Assembly.GetName().Name)
        {
            return null;
        }

        string target = _resolver.ResolveAssemblyToPath(assemblyName)!;
        if (!string.IsNullOrEmpty(target) && File.Exists(target))
        {
            return LoadFromAssemblyPath(target);
        }
        return null;
    }

    protected override nint LoadUnmanagedDll(string unmanagedDllName)
    {
        string path = _resolver.ResolveUnmanagedDllToPath(unmanagedDllName)!;

        return string.IsNullOrEmpty(path) || !File.Exists(path)
            ? nint.Zero
            : LoadUnmanagedDllFromPath(path);
    }
}
