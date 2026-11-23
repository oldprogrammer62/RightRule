namespace RightRule.Models;

public enum RightRuleResultEnum
{
    SUCCESS = 1,
    FAILURE = 2,
    ERROR = 3,
    DEFAULT = 4
}

public enum RegistrationStatusEnum
{
    SUCCESS = 1,
    FAILURE = 2,
    ERROR = 3
}

public static class StatusExtensions
{
    public static RegistrationStatusEnum ParserToRegistrationStatus(
        this RightRuleResultEnum status) => status switch
    { 
        RightRuleResultEnum.SUCCESS => RegistrationStatusEnum.SUCCESS,
        RightRuleResultEnum.FAILURE => RegistrationStatusEnum.FAILURE,
        RightRuleResultEnum.ERROR => RegistrationStatusEnum.ERROR,
        RightRuleResultEnum.DEFAULT => RegistrationStatusEnum.SUCCESS,
        _ => throw new ArgumentOutOfRangeException(nameof(status), status, "Unmapped status value")
    };
}
