namespace RightRule.RuleServices.RuleModels.BaseModels;

public interface IRule
{
    int Number { get; }
    string Name { get; }
    string Context { get; }
}
