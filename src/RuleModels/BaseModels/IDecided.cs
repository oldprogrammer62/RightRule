namespace RightRule.RuleModels.BaseModels;

public interface IDecided
{
    bool IsSuccess { get; }
    bool Result { get; }

    string StatementApplied { get; }

    string Message {  get; }
}