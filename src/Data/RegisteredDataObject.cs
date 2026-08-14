using System.Diagnostics.CodeAnalysis;

namespace RightRule.Data;

public class RegisteredDataObject
{
    public required string DataName { get; init; }
    public required object DataValue { get; init; }

    [SetsRequiredMembers]
    internal RegisteredDataObject(string DataName, object DataValue)
    {
        this.DataName = DataName;
        this.DataValue = DataValue;
    }
}
