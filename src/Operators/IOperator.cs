namespace RightRule.Operators;

public interface xxxxIOperator
{
    /// <summary>
    /// The textual name used in the DSL, e.g., "equals", "notequals", "contains".
    /// </summary>
    List<string> Names { get; }

    /// <summary>
    /// Process the condition with resolved left/right operands.
    /// Return true/false.
    /// </summary>
    bool Process(object left, object right);
}
