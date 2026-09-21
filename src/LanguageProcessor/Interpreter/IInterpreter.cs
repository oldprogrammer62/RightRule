using RightRule.LanguageProcessor.Evaluator;
using RightRule.Models.BaseModels;
using RightRule.RegistrationServices.RuleRegistration.Rules;

namespace RightRule.LanguageProcessor.Interpreter
{
    public interface IInterpreter
    {
        Task<IProcessedRule> EvaluateFirstAsync(IRegisteredRules Rules, string Context);
        Task<List<IProcessedRule>> EvaluateAnyAsync(IRegisteredRules Rules, string Context, EvaluationTypeEnum EvaluationType);
        Task<List<IProcessedRule>> EvaluateAllAsync(IRegisteredRules Rules, string Context, EvaluationTypeEnum EvaluationType);
    }
}