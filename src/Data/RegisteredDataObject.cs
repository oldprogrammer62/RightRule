using System.Diagnostics.CodeAnalysis;

namespace RightRule.Data;

public class RegisteredDataObject
{
    public required string Name { get; init; }
    public required object Value { get; init; }

    [SetsRequiredMembers]
    internal RegisteredDataObject(string DataName, object DataValue)
    {
        this.Name = DataName;
        this.Value = DataValue;
    }
}
