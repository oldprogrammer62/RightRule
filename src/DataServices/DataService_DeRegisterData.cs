using Microsoft.Extensions.Logging;
using RightRule.DataServices.Data;

namespace RightRule.DataServices;

internal sealed partial class DataService : IDataService
{
    public bool DeRegisterData()
    {
        if (_datasets == null || _datasets.Count == 0)
        {
            return true;
        }
        _datasets = new List<Dataset>();

        if (_logger.IsEnabled(LogLevel.Information))
        {
            _logger.LogInformation("Datasets Cleared");
        }
        return true;
    }

    public bool DeRegisterData(string DatasetName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(DatasetName, nameof(DatasetName));

        if (_datasets == null || _datasets.Count == 0)
        {
            return true;
        }
        _datasets.Remove(_datasets.First(x => x.DatasetName.Equals(DatasetName, StringComparison.OrdinalIgnoreCase)));

        if (_logger.IsEnabled(LogLevel.Debug))
        {
            _logger.LogDebug("{DatasetName} deregistered", DatasetName);
        }
        return true;
    }
}
