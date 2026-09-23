using RightRule.DataServices.Data;
using RightRule.DataServices.DataRegistration;
using System.Data;

namespace BeSoft;

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

    /// <summary>
    /// Registers a data set with the RightRule engine for use in rule evaluation.
    /// </summary>
    /// <param name="DatasetName">The name of the dataset.</param>
    /// <param name="DataItems">The data to register.</param>
    /// <returns>True if the data was successfully registered; otherwise, false.</returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="ArgumentNullException"></exception>
    public DataRegistrationResult RegisterData(string DatasetName, Dictionary<string, object> DataItems)
    {
        ArgumentNullException.ThrowIfNull(DataItems, nameof(DataItems));
        ArgumentException.ThrowIfNullOrWhiteSpace(DatasetName, nameof(DatasetName));
        try
        {
            return RightRuleService.RegisterData(DatasetName, DataItems);
        }
        catch
        {
            throw;
        }
    }

    /// <summary>
    /// Registers a data set with the RightRule engine for use in rule evaluation.
    /// </summary>
    /// <param name="DatasetName">The name of the dataset.</param>
    /// <param name="DataItem">The data to register.</param>
    /// <returns>True if the data was successfully registered; otherwise, false.</returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="ArgumentNullException"></exception>
    public DataRegistrationResult RegisterData(string DatasetName, ValueTuple<string, object> DataItem)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(DatasetName, nameof(DatasetName));
        try
        {
            return RightRuleService.RegisterData(DatasetName, DataItem);
        }
        catch
        {
            throw;
        }
    }

    public DataRegistrationResult RegisterData(string DataItemName, object DataItemValue)
    {
        try
        {
            return RightRuleService.RegisterData(DataItemName, DataItemName, DataItemValue);
        }
        catch
        {
            throw;
        }
    }

    public DataRegistrationResult RegisterData(string DatasetName, string DataItemName, bool DataItemValue)
    {
        try
        {
            return RightRuleService.RegisterData(DatasetName, DataItemName, DataItemValue);
        }
        catch
        {
            throw;
        }
    }

    public DataRegistrationResult RegisterData(string DatasetName, string DataItemName, string DataItemValue)
    {
        try
        {
            return RightRuleService.RegisterData(DatasetName, DataItemName, DataItemValue);
        }
        catch
        {
            throw;
        }
    }

    public DataRegistrationResult RegisterData(string DatasetName, string DataItemName, int DataItemValue)
    {
        try
        {
            return RightRuleService.RegisterData(DatasetName, DataItemName, DataItemValue);
        }
        catch
        {
            throw;
        }
    }

    public DataRegistrationResult RegisterData(string DatasetName, string DataItemName, long DataItemValue)
    {
        try
        {
            return RightRuleService.RegisterData(DatasetName, DataItemName, DataItemValue);
        }
        catch
        {
            throw;
        }
    }

    public DataRegistrationResult RegisterData(string DatasetName, string DataItemName, double DataItemValue)
    {
        try
        {
            return RightRuleService.RegisterData(DatasetName, DataItemName, DataItemValue);
        }
        catch
        {
            throw;
        }
    }

    public DataRegistrationResult RegisterData(string DatasetName, string DataItemName, decimal DataItemValue)
    {
        try
        {
            return RightRuleService.RegisterData(DatasetName, DataItemName, DataItemValue);
        }
        catch
        {
            throw;
        }
    }

    public DataRegistrationResult RegisterData(string DatasetName, string DataItemName, DateTime DataItemValue)
    {
        try
        {
            return RightRuleService.RegisterData(DatasetName, DataItemName, DataItemValue);
        }
        catch
        {
            throw;
        }
    }

    public DataRegistrationResult RegisterData(string DatasetName, string DataItemName, DateOnly DataItemValue)
    {
        try
        {
            return RightRuleService.RegisterData(DatasetName, DataItemName, DataItemValue);
        }
        catch
        {
            throw;
        }
    }

    public DataRegistrationResult RegisterData(string DatasetName, string DataItemName, TimeOnly DataItemValue)
    {
        try
        {
            return RightRuleService.RegisterData(DatasetName, DataItemName, DataItemValue);
        }
        catch
        {
            throw;
        }
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
