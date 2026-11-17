using Microsoft.Extensions.Logging;
using RightRule.DataAdaptors;
using RightRule.DataServices.Data;
using RightRule.DataServices.DataLoader;
using RightRule.DataServices.DataSerializer;
using RightRule.DataServices.DataValidator;

namespace RightRule.DataServices;

internal sealed partial class DataService : IDataService
{
    private List<Dataset> _datasets;
    public List<Dataset> RegisteredDatasets => _datasets;

    private readonly ILogger<DataService> _logger;
    private readonly IDataValidatorService _dataValidator;
    private readonly IDataSerializerService _dataSerializerService;
    private readonly IDataLoaderService _dataLoaderService;
    private readonly IDataAdaptorService _dataAdaptorService;

    public DataService(IDataValidatorService dataValidator, IDataSerializerService dataSerializerService, IDataLoaderService dataLoaderService, IDataAdaptorService dataAdaptorService, ILogger<DataService> logger)
    {
        _logger = logger;

        _datasets = new List<Dataset>();
        _dataValidator = dataValidator;
        _dataSerializerService = dataSerializerService;
        _dataLoaderService = dataLoaderService;
        _dataAdaptorService = dataAdaptorService;
    }

    internal bool IsValidDataIdentifier(string DataIdentifier)
    {
        return _dataValidator.IsDataIdentifier(this, DataIdentifier);
    }
}
