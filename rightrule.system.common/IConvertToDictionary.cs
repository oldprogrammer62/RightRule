namespace RightRule.System.Common;

public interface IConvertToDictionary
{
        Dictionary<string, object> ConvertToDictionary<T>(T data);
}
