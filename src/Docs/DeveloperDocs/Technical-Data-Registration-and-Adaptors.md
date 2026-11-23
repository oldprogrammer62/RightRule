# Data registration and adaptors

## Data model and registration

Data is registered into named datasets through `session.DataService.RegisterData<T>(datasetName, data)`. A statement uses the dataset name and, for structured datasets, item names (for example `Customer.age`). Data and rules are scoped to the session; create/register them in the same session that applies the rules.

The built-in adaptor set handles general objects, strings, dictionaries, `(string, int)` tuples, and lists of those tuples. The object and string adaptors expose the input under the dataset name as a value. The dictionary adaptor exposes entries as named items and can use an `Id` entry to set the resulting dataset name. Tuple adaptors expose tuple names and values. Registering a type is not a promise that every property is flattened: inspect the adapted shape and write rule paths against that shape.

```csharp
var customer = new Dictionary<string, object>
{
    ["age"] = 25,
    ["country"] = "GB"
};

var result = session.DataService.RegisterData("Customer", customer);
if (result.Status != RegistrationStatusEnum.SUCCESS)
    throw new InvalidOperationException(result.Message);
```

Use `RegisteredDatasets`, `GetRegisteredData`, `GetDataSetNames`, and `GetDataItemNames` to inspect registered values. `SerializeData` and `LoadData` support persistence/interchange. Check the registration result before evaluating rules.

## Custom data adaptors

When application input has a shape that the built-in adaptors do not expose suitably, implement `IDataAdaptor` (commonly by deriving from `DataAdaptorBase<TInput>`). Its processing method receives the requested dataset name and typed input, and returns an `AdaptorDataStructure` with the dataset name and named values. Register the adaptor on the session before calling `RegisterData` for that input type.

```csharp
public sealed class CustomerAdaptor : DataAdaptorBase<CustomerInput>
{
    public override string AdaptorName { get; init; } = "CustomerAdaptor";

    public override AdaptorDataStructure Process(string datasetName, CustomerInput input)
    {
        var adapted = new AdaptorDataStructure { DatasetName = datasetName };
        adapted.Data.Add("age", input.Age);
        adapted.Data.Add("country", input.Country);
        return adapted;
    }
}

session.DataAdaptorService.RegisterDataAdaptor(new CustomerAdaptor());
var result = session.DataService.RegisterData("Customer", customerInput);
```

The adaptor output determines the paths that rules can reference. Keep names stable, avoid duplicate keys, and preserve values in types that operators can compare. Inspect `DataAdaptorRegistrationResult` and `DataRegistrationResult` for failures. `ProcessIncomingData` is also available for explicitly obtaining the adaptor output without registering it.

## Available value types

`RegisterData<T>` accepts any non-null input type for which a registered adaptor can produce the RightRule data structure. The built-ins cover the input categories above; custom adaptors can support application-specific classes and other types. The resulting named values may be primitive or structured objects, but rule access depends on the adaptor output and supported statement paths. RightRule's documented intrinsic checks include primitive values, `string`, `decimal`, `DateTime`, `DateOnly`, and `TimeOnly`. Ensure JSON-loaded values and custom adaptor values have types compatible with the operators and expressions that use them.
