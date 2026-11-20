using RightRule.DataServices.Data;

namespace RightRule.DataServices.DataValidator;

internal interface IDataValidatorService
{
    bool IsDataIdentifier(IDataService dataService, string DataIdentifier);
    bool ValidateData(IDataService dataService, List<Dataset> datasets);
}
