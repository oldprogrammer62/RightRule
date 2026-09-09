namespace RightRule.Models.BaseModels;

public interface IProcessedRule : IRule, IDecided, IDecideTrue, IDecideFalse
{
    string? ToString();
}
