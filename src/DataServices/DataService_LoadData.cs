using RightRule.DataServices.Data;
using RightRule.DataServices.DataRegistration;

namespace RightRule.DataServices;

internal sealed partial class DataService : IDataService
{
    public List<DataRegistrationResult> LoadData(string JsonData)
    {
        ArgumentNullException.ThrowIfNullOrWhiteSpace(JsonData, nameof(JsonData));

        var datasets = _dataLoaderService.LoadData(JsonData);
        if (datasets == null)
        {
            return [];
        }
        if (ValidateData(datasets))
        {
            return AddData(datasets);
        }
        return [];
    }

    private bool ValidateData(List<Dataset> datasets)
    {
        return _dataValidatorService.ValidateData(this, datasets);
    }
}
