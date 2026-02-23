using System.Diagnostics.CodeAnalysis;

namespace System.Common;

public class DecisionResult
{
    public bool Decision { get; init; }
    public string KeyWord { get; init; }

    [SetsRequiredMembers]
    public DecisionResult()
    {
        Decision = false;
        KeyWord = string.Empty;        
    }
}
