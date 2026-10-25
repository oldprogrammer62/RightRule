namespace RightRule.DataServices.DataValidator;

internal interface IDataValidatorService
{
    bool IsDataIdentifier(IDataService DataService, string DataIdentifier);
}
