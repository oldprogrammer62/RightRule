using RightRule.DataServices.DataRegistration;

namespace RightRule.DataServices;

internal sealed partial class DataService : IDataService
{
    public List<DataRegistrationResult> LoadData(string JsonData)
    {
        ArgumentNullException.ThrowIfNullOrWhiteSpace(JsonData, nameof(JsonData));

        var dataDef = _dataLoaderService.LoadData(JsonData);
        if (dataDef == null)
        {
            return [];
        }
        return AddData(dataDef);
    }
}
