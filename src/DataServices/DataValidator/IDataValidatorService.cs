namespace RightRule.DataServices.DataValidator;

internal interface IDataValidatorService
{
    bool IsValidDataIdentifier(IDataService DataService, string DataIdentifier);
}
