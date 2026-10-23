namespace RightRule.DataTransformer;

internal sealed class InterpolatorService : IInterpolatorService
{
    private readonly IInterpolate _stringInterpolator;

    public InterpolatorService(IInterpolate stringInterpolator)
    {
        _stringInterpolator = stringInterpolator;
    }

    public object? Interpolate(object? ObjectToProcess)
    {
        if (ObjectToProcess is null)
        {
            return null;
        }
        if (ObjectToProcess is string stringToProcess)
        {
            return _stringInterpolator.Interpolate(stringToProcess);
        }
        return ObjectToProcess;
    }
}
