using RightRule.Interpreter;

namespace RightRule.Rules;

public class ParsedRule : Rule
{
    internal ParsedRule()
    {
    }

    public required Node? Node { get; init; }
}
