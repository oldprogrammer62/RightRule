namespace RightRule.RuleServices.RuleModels.BaseModels;

public interface IProcessedRule : IRule, IDecided, IDecideTrue, IDecideFalse
{
    string? ToString();
}
