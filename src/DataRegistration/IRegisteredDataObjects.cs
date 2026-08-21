using RightRule.Models;

namespace RightRule.DataRegistration;

public interface IRegisteredDataObjects
{
    List<RegisteredDataObject> Objects { get; }
    int Count { get; }
    RegisteredDataObject? this[int index] { get; }
    HashSet<string> ObjectNames { get; }
    DataRegistrationResult AddData(RegisteredDataObject data);
    bool RemoveData();
    bool RemoveData(string name);
    RegisteredDataObject? GetData(string name);
}
