using RightRule.DataServices.Data;
using RightRule.Factories.DataLoaderFactory;

namespace RightRule.DataServices.DataLoader;

internal class DataLoaderService : IDataLoaderService
{
    private readonly IDataLoaderFactory _dataLoaderFactory;

    public DataLoaderService(IDataLoaderFactory dataLoaderFactory)
    {
        _dataLoaderFactory = dataLoaderFactory;
    }

    public List<Dataset>? LoadData(string JsonData)
    {
        string loaderType = "JSON";
        var loader = GetLoader(loaderType);
        if (loader == null)
        {
            return [];
        }
        var rules = loader.Deserialize(JsonData);
        return rules;
    }

    private IDataLoader? GetLoader(string LoaderType)
    {
        var loader = _dataLoaderFactory.BuildDataLoader(LoaderType);
        return loader;
    }
}
