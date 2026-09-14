using RightRule.Models.BaseModels;
using RightRule.RegistrationServices.DataRegistration;
using RightRule.RegistrationServices.RuleRegistration;
using RightRule.RuleApplication;

namespace RightRule.LanguageProcessor.Interpreter
{
    public interface IInterpreter
    {
        Task<IProcessedRule> EvaluateFirstAsync(IRegisteredRules rules, string Context);
        Task<List<IProcessedRule>> EvaluateAllAsync(IRegisteredRules rules, string Context, RulesToReturn ReturnRules);
    }
}