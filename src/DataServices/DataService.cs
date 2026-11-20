using Microsoft.Extensions.Logging;
using RightRule.DataAdaptors;
using RightRule.DataServices.Data;
using RightRule.DataServices.DataLoader;
using RightRule.DataServices.DataSerializer;
using RightRule.DataServices.DataValidator;
using System.Diagnostics;

namespace RightRule.DataServices;

internal sealed partial class DataService : IDataService
{
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private List<Dataset> _datasets;
    public List<Dataset> RegisteredDatasets => _datasets;

    public List<string> DataSetNames => GetDataSetNames();

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private readonly ILogger<DataService> _logger;

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private readonly IDataValidatorService _dataValidatorService;

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private readonly IDataSerializerService _dataSerializerService;

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private readonly IDataLoaderService _dataLoaderService;

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private readonly IDataAdaptorService _dataAdaptorService;

    public DataService(IDataValidatorService dataValidator, IDataSerializerService dataSerializerService, IDataLoaderService dataLoaderService, IDataAdaptorService dataAdaptorService, ILogger<DataService> logger)
    {
        _logger = logger;

        _datasets = new List<Dataset>();
        _dataValidatorService = dataValidator;
        _dataSerializerService = dataSerializerService;
        _dataLoaderService = dataLoaderService;
        _dataAdaptorService = dataAdaptorService;
    }

    internal bool IsValidDataIdentifier(string DataIdentifier)
    {
        return _dataValidatorService.IsDataIdentifier(this, DataIdentifier);
    }
}
