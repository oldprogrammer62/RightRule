using RightRule.DataServices.DataLoader;

namespace RightRule.Factories.DataLoaderFactory;

internal interface IDataLoaderFactory
{
    IDataLoader? BuildDataLoader(string LoaderType);
}