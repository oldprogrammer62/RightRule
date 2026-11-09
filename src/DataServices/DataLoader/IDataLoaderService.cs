using RightRule.DataServices.Data;

namespace RightRule.DataServices.DataLoader
{
    internal interface IDataLoaderService
    {
        List<Dataset>? LoadData(string JsonData);
    }
}