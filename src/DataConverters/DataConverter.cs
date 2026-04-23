namespace RightRule.DataConverters;

internal class DataConverter
{
    public static Dictionary<string, object> ConvertToDictionary<T>(T data)
    {
        var listConverter = new ListConverter();
        return listConverter.ConvertToDictionary(data);
    }
}
