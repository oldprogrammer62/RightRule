using RightRule.DataServices.Data;

namespace RightRule.DataServices.DataLoader;

internal interface IDataLoader
{
    List<Dataset>? Deserialize(string JsonData);
}
