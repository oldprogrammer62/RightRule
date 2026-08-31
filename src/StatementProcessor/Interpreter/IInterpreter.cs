using RightRule.RuleModels;
using RightRule.RuleModels.BaseModels;
using RightRule.RuleRegistration;
using RightRule.System.Common;

namespace RightRule.StatementProcessor.Interpreter
{
    public interface IInterpreter
    {
        Task<IRule> EvaluateFirstAsync(IRegisteredRules rules);
        Task<List<IRule>> EvaluateAllAsync(IRegisteredRules rules, RulesToReturn ReturnRules);
    }
}