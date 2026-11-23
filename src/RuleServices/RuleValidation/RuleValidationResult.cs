using RightRule.Models;

namespace RightRule.RuleServices.RuleValidation;

internal class RuleValidationResult
{
    public RightRuleResultEnum Valid { get; set; }
    public string Message { get; set; }

    public RuleValidationResult()
    {
        Message = string.Empty;
        Valid = RightRuleResultEnum.NOTAPPLICABLE;
    }
}
    