using RightRule.HelperFunctions;

namespace RightRule.DataConverters;

public class DataConverter
{
    public static Dictionary<string, object> ConvertToDictionary<T>(T data)
    {
        if (IsTEnumerable(typeof(T)))
        {
            var listConverter = new ListConverter();
            return listConverter.ConvertToDictionary(data);
        }
        
        var objectConverter = new ObjectConverter();
        return objectConverter.ConvertToDictionary(data);
    }

    private static bool IsTEnumerable(Type type)
    {
        return Helpers.IsTEnumerable(type, out _);
    }
}
