
using RightRule.DataServices.Data;

namespace RightRule.Factories.DataLoaderFactory;

internal interface IDataLoader
{
    List<Dataset>? Load(string JsonData);
}
