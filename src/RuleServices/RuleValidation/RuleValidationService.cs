using RightRule.RuleServices.RuleModels;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RightRule.RuleServices.RuleValidation;

internal class RuleValidationService : IRuleValidationService
{
    public RuleValidationResult ValidateRule(DefinedRule rule)
    {
        return ValidateDefinedRule(rule);
    }

    private RuleValidationResult ValidateDefinedRule(DefinedRule rule)
    {
        if (rule is null)
        {
            return new RuleValidationResult()
            {
                Valid = Models.RightRuleStatusEnum.ERROR,
                Message = "Rule not defined"
            };
        }

        /* Rule Validation - Mandatory Fields
         * Number - Must be a number
         * Name - Must be a string
         * Context - Must be a string
         * Statement - Must be a string
         * TrueDecision - Must be a string
         * FalseDecision - Must be a string
         */

        var number = rule.Number;
        var name = rule.Name;
        var context = rule.Context;
        var statement = rule.Statement;
        var trueDecision = rule.TrueDecision;
        var falseDecision = rule.FalseDecision;

        var validNumber = number >= 1;
        var validName = !string.IsNullOrWhiteSpace(name);
        var validContext = !string.IsNullOrWhiteSpace(context);
        var validStatement = !string.IsNullOrWhiteSpace(statement);
        var validTrueDecision = !string.IsNullOrWhiteSpace(trueDecision);
        var validFalseDecision = !string.IsNullOrWhiteSpace(falseDecision);

        StringBuilder sb = new StringBuilder();
        sb.Append(!validNumber ? "Rule Number," : "");
        sb.Append(!validName ? "Rule Name," : "");
        sb.Append(!validContext ? "Context," : "");
        sb.Append(!validStatement ? "Statement," : "");
        sb.Append(!validTrueDecision ? "TrueDecision," : "");
        sb.Append(!validFalseDecision ? "FalseDecision," : "");

        if (AllOk(validNumber, validName, validContext, validStatement, validTrueDecision, validFalseDecision))
        {
            return new RuleValidationResult()
            {
                Valid = Models.RightRuleStatusEnum.SUCCESS,
                Message = string.Empty
            };
        }

        sb.Insert(0, "Missing or Invalid fields: ");
        sb.Remove(sb.Length - 1, 1);
        return new RuleValidationResult()
        {
            Valid = Models.RightRuleStatusEnum.ERROR,
            Message = sb.ToString()
        };
    }

    private static bool AllOk(
        bool number,
        bool name,
        bool context,
        bool statement,
        bool trueDecision,
        bool falseDecision)
    {
        if (!number) return false;
        if (!name) return false;
        if (!context) return false;
        if (!statement) return false;
        if (!trueDecision) return false;
        if (!falseDecision) return false;
        return true;
    }
}
