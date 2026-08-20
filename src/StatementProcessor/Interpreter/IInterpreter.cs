using RightRule.RuleModels;
using RightRule.RuleRegistration;
using RightRule.System.Common;

namespace RightRule.StatementProcessor.Interpreter
{
    public interface IInterpreter
    {
        Task<List<AppliedRule>> ProcessRuleAsync(IRegisteredRules rules);
        Task<List<AppliedRule>> ProcessRulesAsync(IRegisteredRules rules, RulesToReturn ReturnRules);
    }
}