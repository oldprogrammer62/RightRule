using RightRule.Data;
using RightRule.Rules;
using RightRule.System.Common;

namespace RightRule.StatementProcessor.Interpreter;

public sealed class Interpreter
{
    private readonly RegisteredDatasets _registeredDatasets;
    private readonly Dictionary<string, IOperator> _operators;
    private readonly Evaluator.Evaluator _evaluator;

    public Interpreter(RegisteredDatasets data, Dictionary<string, IOperator> operators)
    {
        _registeredDatasets = data ?? new RegisteredDatasets();
        _operators = operators ?? new Dictionary<string, IOperator> { };
        _evaluator = new Evaluator.Evaluator(_operators);
    }

    internal AppliedRule ProcessRule(ParsedRules rules)
    {
        return Evaluate(rules);
    }

    private AppliedRule Evaluate(ParsedRules rules)
    {
        return ProcessFirstRule.EvaluateFirst(_evaluator, rules.RightRules, _registeredDatasets);
    }

    internal AppliedRule ProcessRule(ParsedRules rules, string datasetName)
    {
        return Evaluate(rules, datasetName);
    }

    private AppliedRule Evaluate(ParsedRules rules, string datasetName)
    {
        return ProcessFirstRule.EvaluateFirst(_evaluator, rules.RightRules, _registeredDatasets, datasetName);
    }

    internal List<AppliedRule> ProcessRules(ParsedRules rules)
    {
        return Evaluate(rules.RightRules, RulesToReturn.All);
    }

    internal List<AppliedRule> ProcessRules(ParsedRules rules, RulesToReturn rulesToReturn)
    {
        return Evaluate(rules.RightRules, rulesToReturn);
    }

    private List<AppliedRule> Evaluate(List<ParsedRule> rules, RulesToReturn rulesToReturn)
    {
        switch (rulesToReturn)
        {
            case RulesToReturn.First:
                return [ProcessFirstRule.EvaluateFirst(_evaluator, rules, _registeredDatasets)];

            case RulesToReturn.All:
                return ProcessAllRules.EvaluateAll(_evaluator, rules, _registeredDatasets);

            default:
                return [AppliedRule.DefaultRule($"Unknown RulesToReturn value: {rulesToReturn}")];
        }
    }

    internal List<AppliedRule> ProcessRules(ParsedRules rules, string datasetName)
    {
        return Evaluate(rules.RightRules, datasetName, RulesToReturn.All);
    }

    internal List<AppliedRule> ProcessRules(ParsedRules rules, string datasetName, RulesToReturn rulesToReturn)
    {
        return Evaluate(rules.RightRules, datasetName, rulesToReturn);
    }

    private List<AppliedRule> Evaluate(List<ParsedRule> rules, string datasetName, RulesToReturn rulesToReturn)
    {
        switch (rulesToReturn)
        {
            case RulesToReturn.First:
                return [ProcessFirstRule.EvaluateFirst(_evaluator, rules, _registeredDatasets, datasetName)];

            case RulesToReturn.All:
                return ProcessAllRules.EvaluateAll(_evaluator, rules, _registeredDatasets, datasetName);

            default:
                return [AppliedRule.DefaultRule($"Unknown RulesToReturn value: {rulesToReturn}")];
        }
    }


    //private AppliedRule EvaluateFirst(List<ParsedRule> rules)
    //{
    //    bool result = false;

    //    AppliedRule appliedRule = AppliedRule.DefaultRule();
    //    foreach (var rule in rules)
    //    {
    //        for (var i = 0; i < _registeredDatasets.Count; i++)
    //        {
    //            var data = _registeredDatasets[i];
    //            if (data == null)
    //            {
    //                continue;
    //            }

    //            result = Evaluate(rule, data);
    //            appliedRule = CreateAppliedRule(result, rule, data);
    //            break;
    //        }
    //        if (result)
    //        {
    //            break;
    //        }
    //    }
    //    return appliedRule;
    //}

    //private AppliedRule EvaluateFirst(List<ParsedRule> rules, string datasetName)
    //{
    //    bool result = false;

    //    var data = _registeredDatasets.GetDataset(datasetName);
    //    if (data == null)
    //    {
    //        return AppliedRule.DefaultRule($"Dataset '{datasetName}' not found.");
    //    }

    //    AppliedRule appliedRule = AppliedRule.DefaultRule();
    //    foreach (var rule in rules)
    //    {
    //        result = Evaluate(rule, data);
    //        appliedRule = CreateAppliedRule(result, rule, data);

    //        if (result)
    //        {
    //            break;
    //        }
    //    }
    //    return appliedRule;
    //}

    //private List<AppliedRule> EvaluateAll(List<ParsedRule> rules)
    //{
    //    AppliedRule resultRule = AppliedRule.DefaultRule();
    //    List<AppliedRule> appliedRules = [];

    //    foreach (var rule in rules)
    //    {
    //        for (var i = 0; i < _registeredDatasets.Count; i++)
    //        {
    //            var data = _registeredDatasets[i];
    //            if (data == null)
    //            {
    //                continue;
    //            }

    //            var result = Evaluate(rule, data);
    //            var appliedRule = CreateAppliedRule(result, rule, data);
    //            appliedRules.Add(appliedRule);
    //        }
    //    }
    //    return appliedRules;
    //}

    //private List<AppliedRule> EvaluateAll(List<ParsedRule> rules, string datasetName)
    //{
    //    var data = _registeredDatasets.GetDataset(datasetName);
    //    if (data == null)
    //    {
    //        return [AppliedRule.DefaultRule($"Dataset '{datasetName}' not found.")];
    //    }

    //    AppliedRule resultRule = AppliedRule.DefaultRule();
    //    List<AppliedRule> appliedRules = [];

    //    foreach (var rule in rules)
    //    {
    //        var result = Evaluate(rule, data);
    //        var appliedRule = CreateAppliedRule(result, rule, data);
    //        appliedRules.Add(appliedRule);
    //    }
    //    return appliedRules;
    //}

    //private bool Evaluate(ParsedRule rule, RegisteredDataset data)
    //{
    //    return _evaluator.Evaluate(data, rule.Node, rule.Arguments);
    //}

    //private static AppliedRule CreateAppliedRule(bool result, ParsedRule rule, RegisteredDataset data)
    //{
    //    var statement = Helper.ResolveStatement(rule.Statement, rule.Arguments);
    //    return new AppliedRule(rule, data.Name, statement, result);
    //}
}
