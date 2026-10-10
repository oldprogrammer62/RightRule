using RightRule.Models;
using RightRule.OperatorServices;
using RightRule.System.Common.Operator.Models;

namespace RightRule;

/// <summary>
/// Main RightRule class
/// Provides all public entry points to the RightRule system
/// </summary>
public sealed partial class RightRule : IRightRule, IDisposable
{
    private static RightRuleOptions CheckOptions(RightRuleOptions options)
    {
        if (string.IsNullOrWhiteSpace(options.CustomOperatorLibraryPath))
        {
            options.CustomOperatorLibraryPath =
                RightRuleLibraryDefaults.CustomOperatorLibraryPath;
        }
        return options;
    }
}
