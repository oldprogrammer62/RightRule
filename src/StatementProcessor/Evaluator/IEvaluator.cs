using RightRule.Data;
using RightRule.Models;
using RightRule.RuleModels;

namespace RightRule.StatementProcessor.Evaluator;

public interface IEvaluator
{
    Task<RuleResult> EvaluateAsync(ParsedRule ParsedRule, IRegisteredDatasets? Datasets, IRegisteredDataObjects? DataObjects);
}