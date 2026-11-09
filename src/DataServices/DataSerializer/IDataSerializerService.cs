using RightRule.DataServices.Data;

namespace RightRule.DataServices.DataSerializer
{
    internal interface IDataSerializerService
    {
        string SerializeData(List<Dataset> Datasets);
        string SerializeData(Dataset Dataset);
    }
}