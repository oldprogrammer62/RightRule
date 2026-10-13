namespace RightRule.DataTransformer
{
    public interface IInterpolatorService
    {
        object? Interpolate(object? DataToInterpolate);
    }
}