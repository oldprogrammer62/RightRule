namespace RightRule.RuleServices.RuleModels.BaseModels;

public interface IDecided
{
    bool Success { get; }
    bool Result { get; }

    string StatementApplied { get; }

    string Message {  get; }
}