namespace RightRule.System.Common.Exceptions;

public class ConvertDataException : Exception
{
    public string DatasetName { get; }

    public ConvertDataException() : base()
    {
        DatasetName = string.Empty;
    }

    public ConvertDataException(string message) : base(message)
    {
        DatasetName = string.Empty;
    }

    public ConvertDataException(string datasetName, string message) : base(message)
    {
        DatasetName = datasetName;
    }

    public ConvertDataException(string datasetName, string message, Exception innerException) : base(message, innerException)
    {
        DatasetName = datasetName;
    }
}
