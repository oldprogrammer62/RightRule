# Data registration and adaptors

## Register data

`session.DataService.RegisterData<T>(datasetName, data)` converts the supplied value through the adaptor registered for its compile-time type, then stores the returned named values in the requested dataset. Statements address an item as `DatasetName.ItemName`. Check `DataRegistrationResult.Status` and `Message` before applying rules. Data and adaptors are scoped to the RightRule instance/session.

Built-in adaptors cover these input types:

| Input type | Adapted shape |
|---|---|
| `Dictionary<string, object>` | Each dictionary entry becomes a named dataset item. |
| `(string Name, int Value)` | One item using `Name` as its key and `Value` as its value. |
| `List<(string Name, int Value)>` | One item for each tuple. |
| `string` | One item whose key is the dataset name and whose value is the string. |
| `object` | One item whose key is the dataset name and whose value is the object. |

The general `object` adaptor is a fallback for input types without a more specific adaptor. It stores the object as a single value; it does not automatically flatten arbitrary object properties. A dictionary item's key `Id` does not change the dataset name used by `RegisterData`.

```csharp
var customer = new Dictionary<string, object>
{
    ["age"] = 25,
    ["country"] = "GB"
};

var registration = session.DataService.RegisterData("Customer", customer);
if (registration.Status != RegistrationStatusEnum.SUCCESS)
    throw new InvalidOperationException(registration.Message);
// The rule can now refer to Customer.age and Customer.country.
```

You can register a simple scalar such as a number or boolean through the object adaptor. Its item name is the same as the dataset name, so the statement reference is `age.age` when the dataset name is `age`. Use `GetDataSetNames`, `GetDataItemNames`, `GetRegisteredData`, and `GetDataItemValue` to inspect registered data. JSON loading and serialization are also available through the data service.

## Develop and register a custom adaptor

Use a custom adaptor when an application type needs a more useful named shape. Implement `IDataAdaptor<TInput>` (often by deriving from `DataAdaptorBase<TInput>` in the adaptor support library). Its `Process` method receives the requested dataset name and input and returns an `AdaptorDataStructure` containing named values.

```csharp
public sealed class CustomerAdaptor : DataAdaptorBase<CustomerInput>
{
    public override string AdaptorName { get; init; } = "CustomerAdaptor";

    public override AdaptorDataStructure Process(
        string datasetName, CustomerInput input)
    {
        var adapted = new AdaptorDataStructure { DatasetName = datasetName };
        adapted.Data.Add("age", input.Age);
        adapted.Data.Add("country", input.Country);
        return adapted;
    }
}

var adaptorRegistration = session.DataAdaptorService
    .RegisterDataAdaptor(new CustomerAdaptor());
var dataRegistration = session.DataService
    .RegisterData("Customer", customerInput);
```

Register the adaptor before registering an instance of its input type. The adaptor manager selects by the generic input type; when no exact match exists it uses the `object` fallback. Keep emitted keys stable and unique, and preserve types that the operators in your statements can compare. Check both adaptor and data registration results. `ProcessIncomingData` lets you obtain the adapted structure without storing it in a dataset.

## Value types and statement access

`RegisterData<T>` accepts any non-null input for which an adaptor can produce data. Built-in adaptors cover the categories above; custom adaptors can map application-specific types. The adapted shape, rather than the original CLR object's property layout, determines which paths statements can use. Intrinsic typed-value support includes primitive types, `string`, `decimal`, `DateTime`, `DateOnly`, and `TimeOnly`; operators still determine whether a particular comparison is valid. Ensure JSON-loaded or custom-adaptor values have suitable types for their comparisons.
