using System.Diagnostics.CodeAnalysis;

namespace RightRule.DataRegistration;

public class RegisteredDataObject
{
    public required string Name { get; init; }
    public required object Value { get; init; }

    [SetsRequiredMembers]
    internal RegisteredDataObject(string DataName, object DataValue)
    {
        Name = DataName;
        Value = DataValue;
    }
}
