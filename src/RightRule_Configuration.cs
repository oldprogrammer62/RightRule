using RightRule.HelperFunctions;

namespace BeSoft;

/// <summary>
/// Main RightRule class
/// Provides all public entry points to the RightRule system
/// </summary>
public sealed partial class RightRule : IRightRule, IDisposable
{
    public string GetConfigSettings(string key)
    {
        if (string.IsNullOrEmpty(key))
        {
            return string.Empty;
        }
        return ConfigurationHelper.GetSetting(key);
    }
}
