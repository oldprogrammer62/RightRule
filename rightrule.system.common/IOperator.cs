namespace RightRule.System.Common;

public interface IOperator
{
    /// <summary>
    /// The textual name used in the DSL, e.g., "equals", "notequals", "contains".
    /// </summary>
    List<string> Identifiers { get; }

    /// <summary>
    /// Process the condition with resolved left/right operands.
    /// Return true/false.
    /// </summary>
    bool Process(object left, object right);

    void Dispose();
}
