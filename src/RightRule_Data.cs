using RightRule.DataRegistration;
using RightRule.Models.DataRegistration;

namespace BeSoft;

/// <summary>
/// Main RightRule class
/// Provides all public entry points to the RightRule system
/// </summary>
public sealed partial class RightRule : IRightRule, IDisposable
{
    /// <summary>
    /// Returns the specified Dataset.
    /// </summary>
    /// <param name="DataName">The name of the dataset.</param>
    /// <returns>RegisteredDataset object</returns>
    /// <exception cref="ArgumentException"></exception>
    public RegisteredDataset? RegisteredDataset(string DataName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(DataName, nameof(DataName));
        try
        {
            return _rightRuleService.GetDataSet(DataName);
        }
        catch
        {
            throw;
        }
    }

    /// <summary>
    /// Returns the specified DataObject
    /// </summary>
    /// <param name="DataName">The name of the dataobject.</param>
    /// <returns>RegisteredDataObject</returns>
    /// <exception cref="ArgumentException"></exception>
    public RegisteredDataObject? RegisteredDataObject(string DataName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(DataName, nameof(DataName));
        try
        {
            return _rightRuleService.GetDataObject(DataName);
        }
        catch
        {
            throw;
        }
    }

    /// <summary>
    /// Returns the set of Registered Datasets.
    /// </summary>
    /// <returns>RegisteredDatasets object</returns>
    public IRegisteredDatasets RegisteredDatasets()
    {
        try
        {
            return _rightRuleService.RegisteredDatasets;
        }
        catch
        {
            throw;
        }
    }

    /// <summary>
    /// Returns the set of Registered DataObjects
    /// </summary>
    /// <returns>RegisteredDataObjects object</returns>
    public IRegisteredDataObjects RegisteredDataObjects()
    {
        try
        {
            return _rightRuleService.RegisteredDataObjects;
        }
        catch
        {
            throw;
        }
    }

    /// <summary>
    /// Registers a data set with the RightRule engine for use in rule evaluation.
    /// </summary>
    /// <param name="DataName">The name of the dataset.</param>
    /// <param name="Data">The data to register.</param>
    /// <returns>True if the data was successfully registered; otherwise, false.</returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="ArgumentNullException"></exception>
    public DataRegistrationResult RegisterData(string DataName, Dictionary<string, object> Data)
    {
        ArgumentNullException.ThrowIfNull(Data, nameof(Data));
        ArgumentException.ThrowIfNullOrWhiteSpace(DataName, nameof(DataName));
        try
        {
            return _rightRuleService.RegisterData(DataName, Data);
        }
        catch
        {
            throw;
        }
    }

    /// <summary>
    /// Registers a data set with the RightRule engine for use in rule evaluation.
    /// </summary>
    /// <param name="DataName">The name of the dataset.</param>
    /// <param name="DataReference">The data to register.</param>
    /// <returns>True if the data was successfully registered; otherwise, false.</returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="ArgumentNullException"></exception>
    public DataRegistrationResult RegisterData(string DataName, object DataReference)
    {
        ArgumentNullException.ThrowIfNull(DataReference, nameof(DataReference));
        ArgumentException.ThrowIfNullOrWhiteSpace(DataName, nameof(DataName));
        try
        {
            return _rightRuleService.RegisterData(DataName, DataReference);
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
    /// <param name="DataName">The name of the dataset.</param>
    /// <exception cref="ArgumentException"></exception>
    public bool DeRegisterData(string DataName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(DataName, nameof(DataName));

        try
        {
            return _rightRuleService.DeRegisterData(DataName);
        }
        catch
        {
            throw;
        }
    }
}
