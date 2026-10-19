using Microsoft.Extensions.Logging.Abstractions;
using RightRule.DataAdaptors;
using RightRule.DataAdaptors.DataAdaptorLibrary;
using RightRule.Models;
using RightRule.System.Common.DataAdaptor;

namespace RightRuleTests;

public class DataAdaptorLibraryManagerTests
{
    private readonly DataAdaptorLibraryManager _manager =
        new(NullLogger<DataAdaptorLibraryManager>.Instance);

    [Fact]
    public void Constructor_RegistersDefaultAdaptors()
    {
        Assert.Equal(5, _manager.RegisteredDataAdaptors.Count());
    }

    [Fact]
    public void RegisterDataAdaptor_NullAdaptor_ReturnsFailure()
    {
        var result = _manager.RegisterDataAdaptor(null!);

        Assert.Equal(RightRuleStatusEnum.FAILURE, result.Status);
        Assert.Equal("Adaptor is null", result.Message);
    }

    [Fact]
    public void RegisterDataAdaptor_AdaptorWithoutProviderInterface_ReturnsFailure()
    {
        var result = _manager.RegisterDataAdaptor(new NonProviderAdaptor());

        Assert.Equal(RightRuleStatusEnum.FAILURE, result.Status);
        Assert.Contains("does not implement IDataAdaptor<TInput>", result.Message);
    }

    [Fact]
    public void RegisterDataAdaptor_Provider_ReturnsSuccessAndRegistersInputType()
    {
        var result = _manager.RegisterDataAdaptor(new TestAdaptor<int>());

        Assert.Equal(RightRuleStatusEnum.SUCCESS, result.Status);
        Assert.Contains(typeof(int), _manager.RegisteredDataAdaptors);
    }

    [Fact]
    public void ProcessIncomingData_UsesAdaptorRegisteredForInputType()
    {
        _manager.RegisterDataAdaptor(new TestAdaptor<int>("int-adaptor"));

        var result = _manager.ProcessIncomingData("numbers", 42);

        Assert.Equal("numbers", result.DatasetName);
        Assert.Equal("int-adaptor", result.Data["adaptor"]);
        Assert.Equal(42, result.Data["value"]);
    }

    [Fact]
    public void RegisterDataAdaptor_SameInputType_ReplacesPreviouslyRegisteredAdaptor()
    {
        _manager.RegisterDataAdaptor(new TestAdaptor<int>("first"));
        _manager.RegisterDataAdaptor(new TestAdaptor<int>("second"));

        var result = _manager.ProcessIncomingData("numbers", 42);

        Assert.Equal("second", result.Data["adaptor"]);
    }

    [Fact]
    public void ProcessIncomingData_UsesObjectAdaptorWhenInputTypeIsNotRegistered()
    {
        _manager.RegisterDataAdaptor(new TestAdaptor<object>("object-adaptor"));

        var result = _manager.ProcessIncomingData("unmapped", new UnregisteredInput());

        Assert.Equal("unmapped", result.DatasetName);
        Assert.Equal("object-adaptor", result.Data["adaptor"]);
        Assert.IsType<UnregisteredInput>(result.Data["value"]);
    }

    [Fact]
    public void ProcessIncomingData_UnregisteredType_UsesDefaultObjectAdaptor()
    {
        var input = new UnregisteredInput();

        var result = _manager.ProcessIncomingData("unmapped", input);

        Assert.Equal("unmapped", result.DatasetName);
        Assert.Same(input, result.Data["unmapped"]);
    }

    [Fact]
    public void ProcessIncomingData_NullInput_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(
            () => _manager.ProcessIncomingData<string>("strings", null!));
    }

    [Fact]
    public void ProcessIncomingData_WhenAdaptorThrows_ReturnsEmptyStructure()
    {
        _manager.RegisterDataAdaptor(new ThrowingAdaptor<int>());

        var result = _manager.ProcessIncomingData("numbers", 42);

        Assert.Equal(string.Empty, result.DatasetName);
        Assert.Empty(result.Data);
    }

    private sealed class UnregisteredInput;

    private sealed class NonProviderAdaptor : IDataAdaptor
    {
        public string AdaptorName { get; init; } = nameof(NonProviderAdaptor);

        public AdaptorDataStructure Process(string DatasetName, object Data) =>
            new() { DatasetName = DatasetName };
    }

    private sealed class TestAdaptor<T>(string name = "test-adaptor") : IDataProvider<T>
    {
        public string AdaptorName { get; init; } = name;

        public AdaptorDataStructure Process(string DatasetName, T Data) =>
            new()
            {
                DatasetName = DatasetName,
                Data = new Dictionary<string, object>
                {
                    ["adaptor"] = AdaptorName,
                    ["value"] = Data!
                }
            };

        public AdaptorDataStructure Process(string DatasetName, object Data) =>
            Process(DatasetName, (T)Data);
    }

    private sealed class ThrowingAdaptor<T> : IDataProvider<T>
    {
        public string AdaptorName { get; init; } = nameof(ThrowingAdaptor<T>);

        public AdaptorDataStructure Process(string DatasetName, T Data) =>
            throw new InvalidOperationException("Test exception");

        public AdaptorDataStructure Process(string DatasetName, object Data) =>
            Process(DatasetName, (T)Data);
    }
}