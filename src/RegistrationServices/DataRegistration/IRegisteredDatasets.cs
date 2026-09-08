using RightRule.Models.DataRegistration;

namespace RightRule.RegistrationServices.DataRegistration;

public interface IRegisteredDatasets
{
    List<RegisteredDataset> Datasets { get; }
    int Count { get; }
    RegisteredDataset? this[int index] { get; }
    HashSet<string> DatasetNames { get; }
    DataRegistrationResult AddData(RegisteredDataset data);

    bool RemoveData();
    bool RemoveData(string name);
    RegisteredDataset? GetData(string name);
}
