namespace RightRule.RegistrationServices.DataRegistration.DataValidator;

internal interface IDataValidatorService
{
    bool IsValidDataIdentifier(IDataRegistrationService DataService, string DataIdentifier);
}
