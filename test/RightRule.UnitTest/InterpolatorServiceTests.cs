using RightRule.DataTransformer;

namespace RightRuleTests;

public class InterpolatorServiceTests
{
    [Fact]
    public void Interpolate_NullInput_ReturnsNullWithoutCallingInterpolator()
    {
        var interpolator = new RecordingInterpolator();
        var service = new InterpolatorService(interpolator);

        var result = service.Interpolate(null);

        Assert.Null(result);
        Assert.Equal(0, interpolator.CallCount);
    }

    [Fact]
    public void Interpolate_StringInput_DelegatesToStringInterpolator()
    {
        const string input = "Hello, $name!";
        var expected = new object();
        var interpolator = new RecordingInterpolator { Result = expected };
        var service = new InterpolatorService(interpolator);

        var result = service.Interpolate(input);

        Assert.Same(expected, result);
        Assert.Equal(1, interpolator.CallCount);
        Assert.Equal(input, interpolator.LastInput);
    }

    [Fact]
    public void Interpolate_NonStringInput_ReturnsOriginalObjectWithoutCallingInterpolator()
    {
        var input = new object();
        var interpolator = new RecordingInterpolator();
        var service = new InterpolatorService(interpolator);

        var result = service.Interpolate(input);

        Assert.Same(input, result);
        Assert.Equal(0, interpolator.CallCount);
    }

    private sealed class RecordingInterpolator : IInterpolate
    {
        public int CallCount { get; private set; }

        public object? LastInput { get; private set; }

        public object? Result { get; init; }

        public object? Interpolate<T>(T DataToInterpolate)
        {
            CallCount++;
            LastInput = DataToInterpolate;
            return Result;
        }
    }
}