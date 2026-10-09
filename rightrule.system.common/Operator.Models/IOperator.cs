namespace RightRule.System.Common.Operator.Models
{
    public interface IOperator
    {
        List<string> Aliases { get; }
        bool IsCustom { get; }
        string Name { get; }

        void Dispose();
        Task<OperatorResult> ProcessAsync(object Left, object Right, object? OptionalData);
    }
}