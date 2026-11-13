using RightRule.DataAdaptors;
using RightRule.System.Common.DataAdaptor;

namespace RightRule;

/// <summary>
/// Main RightRule class
/// Provides all public entry points to the RightRule system
/// </summary>
public sealed partial class RightRule : IRightRule, IDisposable
{
    public DataAdaptorRegistrationResult RegisterDataAdaptor(IDataAdaptor DataAdaptor)
    {
        return _rightRuleService.RegisterDataAdaptor(DataAdaptor);
    }
}
