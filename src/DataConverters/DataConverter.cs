using RightRule.HelperFunctions;
using RightRule.System.Common.Exceptions;

namespace RightRule.DataConverters;

public class DataConverter
{
    /// <summary>
    /// Converts the provided data to a dictionary representation. If the data is already a dictionary, it will be returned as is. If the data is an enumerable, it will be converted to a dictionary using the ListConverter. If the data is an object, it will be converted to a dictionary using the ObjectConverter.
    /// </summary>
    /// <typeparam name="T">The type of the data to convert.</typeparam>
    /// <param name="DatasetName">The name of the dataset.</param>
    /// <param name="Data">The data to convert.</param>
    /// <returns>A dictionary representation of the data.</returns>
    /// <exception cref="ConvertDataException">Thrown when an error occurs during data conversion.</exception>
    public static Dictionary<string, object> ConvertToDictionary<T>(string DatasetName, T Data)
    {
        try
        {
            return ConvertDataToDictionary(Data);
        }
        catch (Exception ex)
        {
            throw new ConvertDataException(DatasetName, $"Error converting data to dictionary: {ex.Message}", ex);
        }
    }

    private static Dictionary<string, object> ConvertDataToDictionary<T>(T Data)
    {
        if (Data == null)
        {
            return new Dictionary<string, object>();
        }

        if (IsTypeOfDictionary(typeof(T)))
        {
            return (Dictionary<string, object>)(object)Data;
        }

        if (IsTypeOfEnumerable(typeof(T)))
        {
            var listConverter = new ListConverter();
            return listConverter.ConvertToDictionary(Data);
        }
        
        var objectConverter = new ObjectConverter();
        return objectConverter.ConvertToDictionary(Data);
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
