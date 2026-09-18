namespace RightRule.RegistrationServices.DataRegistration.DataValidator;

internal interface IDataValidator
{
    bool IsValidData(IDataService dataService, string name);
}
