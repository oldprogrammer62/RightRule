using RightRule.System.Common;

namespace RightRule.DataConverters;

public class ObjectConverter : IConvertToDictionary
{
    public Dictionary<string, object> ConvertToDictionary<T>(T data)
    {
        if (data == null)
        {
            return new Dictionary<string, object>();
        }
        
        var result = new Dictionary<string, object>();
        var properties = typeof(T).GetProperties();
        foreach (var property in properties)
        {
            var value = property.GetValue(data);
            result[property.Name] = value ?? DBNull.Value;
        }
        return result;
    }
}
