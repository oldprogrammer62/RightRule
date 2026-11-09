using RightRule.DataServices.Data;

namespace RightRule.DataServices.DataSerializer;

internal class DataSerializerService : IDataSerializerService
{
    public DataSerializerService()
    {
    }

    public string SerializeData(List<Dataset> Datasets)
    {
        return Newtonsoft.Json.JsonConvert.SerializeObject(Datasets);
    }

    public string SerializeData(Dataset Dataset)
    {
        return Newtonsoft.Json.JsonConvert.SerializeObject(Dataset);
    }
}
