namespace RightRule.DataTransformer
{
    public interface IInterpolate
    {
        object? Interpolate<T>(T DataToInterpolate);
    }
}
