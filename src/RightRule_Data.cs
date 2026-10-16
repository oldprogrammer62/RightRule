using RightRule.DataAdaptors;
using RightRule.DataServices.Data;
using RightRule.DataServices.DataRegistration;
using RightRule.System.Common.DataAdaptor;

namespace RightRule;

/// <summary>
/// Main RightRule class
/// Provides all public entry points to the RightRule system
/// </summary>
public sealed partial class RightRule : IRightRule, IDisposable
{
    public List<Dataset>? GetRegisteredData()
    {
        return _rightRuleService.GetDataset();
    }

    public Dataset? GetRegisteredData(string DatasetName)
    {
        return _rightRuleService.GetDataset(DatasetName);
    }

    public DataAdaptorRegistrationResult RegisterDataAdaptor(IDataAdaptor DataAdaptor)
    {
        return _rightRuleService.RegisterDataAdaptor(DataAdaptor);
    }

    public DataRegistrationResult RegisterData<T>(string DatasetName, T Data) where T : notnull
    {
        return _rightRuleService.RegisterData(DatasetName, Data);
    }

    /// <summary>
    /// DeRegisters all data from the RightRule engine.
    /// </summary>
    public bool DeRegisterData()
    {
        try
        {
            return _rightRuleService.DeRegisterData();
        }
        catch
        {
            throw;
        }
    }

    /// <summary>
    /// DeRegisters the specified data from the RightRule engine.
    /// </summary>
    /// <param name="DatasetName">The name of the dataset.</param>
    /// <exception cref="ArgumentException"></exception>
    public bool DeRegisterData(string DatasetName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(DatasetName, nameof(DatasetName));

        try
        {
            return _rightRuleService.DeRegisterData(DatasetName);
        }
        catch
        {
            throw;
        }
    }
}
