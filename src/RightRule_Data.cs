using RightRule.DataProvider;
using RightRule.DataServices.Data;
using RightRule.DataServices.DataRegistration;
using RightRule.System.Common.DataProvider;

namespace RightRule;

/// <summary>
/// Main RightRule class
/// Provides all public entry points to the RightRule system
/// </summary>
public sealed partial class RightRule : IRightRule, IDisposable
{
    public List<Dataset>? GetRegisteredData()
    {
        return RightRuleService.GetDataset();
    }

    public Dataset? GetRegisteredData(string DatasetName)
    {
        return RightRuleService.GetDataset(DatasetName);
    }

    public DataProviderRegistrationResult RegisterDataProvider(IDataProvider DataProvider)
    {
        return RightRuleService.RegisterDataProvider(DataProvider);
    }

    public DataRegistrationResult RegisterData<T>(string DatasetName, T Data) where T : notnull
    {
        return RightRuleService.RegisterData(DatasetName, Data);
    }

    /// <summary>
    /// DeRegisters all data from the RightRule engine.
    /// </summary>
    public bool DeRegisterData()
    {
        try
        {
            return RightRuleService.DeRegisterData();
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
            return RightRuleService.DeRegisterData(DatasetName);
        }
        catch
        {
            throw;
        }
    }
}
