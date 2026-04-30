using RightRule.HelperFunctions;

namespace RightRule.DataConverters;

public class DataConverter
{
    public static Dictionary<string, object> ConvertToDictionary<T>(T data)
    {
        if (data == null)
        {
            return new Dictionary<string, object>();
        }

        if (IsTypeOfDictionary(typeof(T)))
        {
            return (Dictionary<string, object>)(object)data;
        }

        if (IsTypeOfEnumerable(typeof(T)))
        {
            var listConverter = new ListConverter();
            return listConverter.ConvertToDictionary(data);
        }
        
        var objectConverter = new ObjectConverter();
        return objectConverter.ConvertToDictionary(data);
    }

    private static bool IsTypeOfEnumerable(Type type)
    {
        return Helpers.IsTEnumerable(type, out _);
    }

    private static bool IsTypeOfDictionary(Type type)
    {
        return Helpers.IsDictionary(type, out _, out _);
    }
}
