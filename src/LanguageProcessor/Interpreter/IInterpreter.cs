using RightRule.RegistrationServices.Rules;
using RightRule.RuleApplication;
using RightRule.RuleModels.BaseModels;

namespace RightRule.LanguageProcessor.Interpreter
{
    public interface IInterpreter
    {
        Task<IProcessedRule> EvaluateFirstAsync(IRegisteredRules rules, string Context);
        Task<List<IProcessedRule>> EvaluateAllAsync(IRegisteredRules rules, string Context, RulesToReturn ReturnRules);
    }
}