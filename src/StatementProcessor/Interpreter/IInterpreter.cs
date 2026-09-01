using RightRule.RuleModels.BaseModels;
using RightRule.RuleRegistration;
using RightRule.System.Common;

namespace RightRule.StatementProcessor.Interpreter
{
    public interface IInterpreter
    {
        Task<IProcessedRule> EvaluateFirstAsync(IRegisteredRules rules);
        Task<List<IProcessedRule>> EvaluateAllAsync(IRegisteredRules rules, RulesToReturn ReturnRules);
    }
}