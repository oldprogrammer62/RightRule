using RightRule.RuleServices.RuleModels;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RightRule.RuleServices.RuleValidation;

internal sealed class RuleValidationService : IRuleValidationService
{
    public bool ValidateRules(RuleService ruleService, List<DefinedRule> rules)
    {
        if (rules == null)
        {
            return false;
        }
        if (rules.Count == 0)
        {
            return true;
        }
        bool valid = true;
        foreach (var rule in rules)
        {
            var validationResult = ValidateDefinedRule(rule);
            if (!validationResult.Valid.Equals(Models.RightRuleStatusEnum.SUCCESS))
            {
                valid = false;
                break;
            }
        }
        return valid;
    }

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
         * Decision - Must be a string
         */

        var number = rule.Number;
        var name = rule.Name;
        var context = rule.Context;
        var statement = rule.Statement;
        var decision = rule.Decision;

        var validNumber = number >= 1;
        var validName = !string.IsNullOrWhiteSpace(name);
        var validContext = !string.IsNullOrWhiteSpace(context);
        var validStatement = !string.IsNullOrWhiteSpace(statement);
        var validDecision = !string.IsNullOrWhiteSpace(decision);

        StringBuilder sb = new StringBuilder();
        sb.Append(!validNumber ? "Rule Number," : "");
        sb.Append(!validName ? "Rule Name," : "");
        sb.Append(!validContext ? "Context," : "");
        sb.Append(!validStatement ? "Statement," : "");
        sb.Append(!validDecision ? "Decision," : "");

        if (AllOk(validNumber, validName, validContext, validStatement, validDecision))
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
        bool decision)
    {
        if (!number) return false;
        if (!name) return false;
        if (!context) return false;
        if (!statement) return false;
        if (!decision) return false;
        return true;
    }
}
