namespace RightRule.Data;

public class RegisteredDataObject
{
    public string Name { get; init; }
    public object Data { get; init; }

    internal RegisteredDataObject(string Name, object Data)
    {
        this.Name = Name;
        this.Data = Data;
    }
}
