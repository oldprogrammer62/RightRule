using RightRule.LanguageProcessor.Evaluator;
using RightRule.Models.BaseModels;
using RightRule.RegistrationServices.RuleRegistration.Rules;

namespace RightRule.LanguageProcessor.Interpreter
{
    public interface IInterpreter
    {
        Task<IProcessedRule> EvaluateFirstAsync(IRegisteredRules rules, string Context);
        Task<List<IProcessedRule>> EvaluateAnyAsync(IRegisteredRules rules, string Context, EvaluationTypeEnum EvaluationType);
        Task<List<IProcessedRule>> EvaluateAllAsync(IRegisteredRules rules, string Context, EvaluationTypeEnum EvaluationType);
    }
}