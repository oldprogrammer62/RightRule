using RightRule.DataAdaptors;
using RightRule.DataAdaptors.DataAdaptorLibrary;
using RightRule.System.Common.DataAdaptor;
using Xunit;

namespace RightRuleTests;

public class DataAdaptorServiceTests
{
    [Fact]
    public void RegisteredDataAdaptors_ReturnsManagerAdaptors()
    {
        var adaptors = new[] { typeof(string), typeof(int) };
        var manager = new FakeDataAdaptorLibraryManager
        {
            Adaptors = adaptors
        };
        var service = new DataAdaptorService(manager);

        Assert.Same(adaptors, service.RegisteredDataAdaptors);
    }

    [Fact]
    public void RegisterDataAdaptor_ForwardsAdaptorAndReturnsManagerResult()
    {
        var adaptor = new TestAdaptor();
        var result = new DataAdaptorRegistrationResult
        {
            Status = RightRule.Models.RightRuleStatusEnum.SUCCESS,
            Message = "registered"
        };
        var manager = new FakeDataAdaptorLibraryManager
        {
            RegistrationResult = result
        };
        var service = new DataAdaptorService(manager);

        var actual = service.RegisterDataAdaptor(adaptor);

        Assert.Same(adaptor, manager.RegisteredAdaptor);
        Assert.Same(result, actual);
    }

    [Fact]
    public void ProcessIncomingData_ForwardsArgumentsAndReturnsManagerResult()
    {
        const string datasetName = "test-dataset";
        const int data = 42;
        var result = new AdaptorDataStructure { DatasetName = datasetName };
        var manager = new FakeDataAdaptorLibraryManager
        {
            ProcessResult = result
        };
        var service = new DataAdaptorService(manager);

        var actual = service.ProcessIncomingData(datasetName, data);

        Assert.Equal(datasetName, manager.ProcessedDatasetName);
        Assert.Equal(data, manager.ProcessedData);
        Assert.Same(result, actual);
    }

    private sealed class FakeDataAdaptorLibraryManager : IDataAdaptorLibraryManager
    {
        public IEnumerable<Type> Adaptors { get; set; } = [];
        public DataAdaptorRegistrationResult RegistrationResult { get; set; } = null!;
        public AdaptorDataStructure ProcessResult { get; set; } = null!;

        public IDataAdaptor? RegisteredAdaptor { get; private set; }
        public string? ProcessedDatasetName { get; private set; }
        public object? ProcessedData { get; private set; }

        public IEnumerable<Type> RegisteredDataAdaptors => Adaptors;

        public DataAdaptorRegistrationResult RegisterDataAdaptor(IDataAdaptor DataAdaptor)
        {
            RegisteredAdaptor = DataAdaptor;
            return RegistrationResult;
        }

        public AdaptorDataStructure ProcessIncomingData<TInput>(string DatasetName, TInput Data)
            where TInput : notnull
        {
            ProcessedDatasetName = DatasetName;
            ProcessedData = Data;
            return ProcessResult;
        }
    }

    private sealed class TestAdaptor : IDataAdaptor
    {
        public string AdaptorName { get; init; } = nameof(TestAdaptor);

        public AdaptorDataStructure Process(string DatasetName, object Data) =>
            new() { DatasetName = DatasetName };
    }
}
