using RightRule.System.Common;
using System.Collections;

namespace RightRule.DataConverters;

public class ListConverter : IConvertToDictionary
{
    public Dictionary<string, object> ConvertToDictionary<T>(T data)
    {
        if (data is not IEnumerable list || data is string)
        {
            throw new ArgumentException("Input data must be a non-string IEnumerable.", nameof(data));
        }

        var dict = new Dictionary<string, object>(StringComparer.Ordinal);
        int index = 0;
        foreach (var item in list)
        {
            dict[index.ToString()] = item;
            index++;
        }
        return dict;
    }
}
