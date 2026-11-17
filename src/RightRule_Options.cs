using RightRule.Models;

namespace RightRule;

/// <summary>
/// Main RightRule class
/// Provides all public entry points to the RightRule system
/// </summary>
public sealed partial class RightRule
{
    private static bool CheckOptions(RightRuleOptions options)
    {
        if (string.IsNullOrWhiteSpace(options.CustomOperatorLibraryPath))
        {
            options.CustomOperatorLibraryPath = RightRuleLibraryDefaults.CustomOperatorLibraryPath;
        }

        // If LoadCustomOperators is true, then CustomOperatorLibraryPath must be set to a non-empty value
        if (options.LoadCustomOperators 
            && 
            string.IsNullOrWhiteSpace(options.CustomOperatorLibraryPath))
        {
            return false;
        }
        return true;
    }
}
