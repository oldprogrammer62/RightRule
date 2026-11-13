using Microsoft.Extensions.Logging.Testing;
using RightRule.DataAdaptors;
using RightRule.DataAdaptors.DataAdaptorLibrary;
using RightRule.DataServices;
using RightRule.DataServices.DataLoader;
using RightRule.DataServices.DataSerializer;
using RightRule.DataServices.DataValidator;
using RightRule.Factories.DataLoaderFactory;

namespace RightRuleTests;

public class DataServiceTests
{
    private static DataService CreateService() =>
        new
        (
            new DataValidatorService(), 
            new DataSerializerService(), 
            new DataLoaderService(new DataLoaderFactory(new FakeLogger<DataLoaderFactory>())), 
            new DataAdaptorService(new DataAdaptorLibraryManager(new FakeLogger<DataAdaptorLibraryManager>())),
            new FakeLogger<DataService>()
        );

    [Fact]
    public void CreateDataSet_AddsAndReturnsDataset()
    {
        var service = CreateService();

        var dataset = service.CreateDataSet("Customer");

        Assert.Same(dataset, service.GetRegisteredData("Customer"));
        Assert.Single(service.GetRegisteredData()!);
    }

    [Fact]
    public void GetDataset_IsCaseInsensitive()
    {
        var service = CreateService();
        var created = service.CreateDataSet("Customer");

        Assert.Same(created, service.GetRegisteredData("CUSTOMER"));
    }

    [Fact]
    public void AddData_OverloadsAddItemsToTheSameDataset()
    {
        var service = CreateService();

        var dataset = service.AddData(
            "Customer",
            new Dictionary<string, object> { ["Name"] = "Ada" });

        Assert.Same(dataset, service.AddData("customer", ("Age", (object)37)));
        Assert.Same(dataset, service.AddData(
            "CUSTOMER",
            new List<(string, object)> { ("Active", true) }));
        Assert.Same(dataset, service.AddData("Customer", "City", "London"));

        Assert.Equal(4, dataset.DataItems.Count);
        Assert.Equal("Ada", service.GetDataItemValue("Customer", "Name"));
        Assert.Equal(37, service.GetDataItemValue("Customer", "Age"));
        Assert.Equal(true, service.GetDataItemValue("Customer", "Active"));
        Assert.Equal("London", service.GetDataItemValue("Customer", "City"));
    }

    [Fact]
    public void GetDataItem_IsCaseInsensitiveForItemName()
    {
        var service = CreateService();
        service.AddData("Customer", "Name", "Ada");

        var item = service.GetDataItem("Customer", "NAME");

        Assert.NotNull(item);
        Assert.Equal("Name", item.Key);
        Assert.Equal("Ada", item.Value);
    }

    [Fact]
    public void GetDataItemAndValue_ReturnNullWhenDatasetOrItemIsMissing()
    {
        var service = CreateService();
        service.AddData("Customer", "Name", "Ada");

        Assert.Null(service.GetDataItem("Missing", "Name"));
        Assert.Null(service.GetDataItem("Customer", "Missing"));
        Assert.Null(service.GetDataItemValue("Missing", "Name"));
        Assert.Null(service.GetDataItemValue("Customer", "Missing"));
    }

    [Fact]
    public void GetDataItemNames_ReturnsNamesOrAnEmptyListWhenDatasetIsMissing()
    {
        var service = CreateService();
        var dataset = service.AddData(
            "Customer",
            new Dictionary<string, object>
            {
                ["Name"] = "Ada",
                ["Age"] = 37
            });

        Assert.Equal(new[] { "Name", "Age" }, service.GetDataItemNames("Customer"));
        Assert.Equal(new[] { "Name", "Age" }, service.GetDataItemNames(dataset));
        Assert.Empty(service.GetDataItemNames("Missing")!);
    }

    [Fact]
    public void GetDataSetNames_ReturnsDatasetNames()
    {
        var service = CreateService();
        service.CreateDataSet("Customers");
        service.CreateDataSet("Orders");

        Assert.Equal(new[] { "Customers", "Orders" }, service.GetDataSetNames());
    }

    [Fact]
    public void DeregisterDataset_RemovesMatchingDatasetCaseInsensitively()
    {
        var service = CreateService();
        service.CreateDataSet("Customer");
        service.CreateDataSet("Order");

        service.DeRegisterData("CUSTOMER");

        Assert.Null(service.GetRegisteredData("Customer"));
        Assert.NotNull(service.GetRegisteredData("Order"));
        Assert.Single(service.GetRegisteredData()!);
    }

    [Fact]
    public void Clear_RemovesAllDatasetsAndCanBeCalledWhenEmpty()
    {
        var service = CreateService();
        service.AddData("Customer", "Name", "Ada");

        service.DeRegisterData();

        Assert.Empty(service.GetRegisteredData()!);
        Assert.Empty(service.GetDataSetNames());

        service.DeRegisterData();

        Assert.Empty(service.GetRegisteredData()!);
    }

    [Fact]
    public void IsValidDataIdentifier_ValidatesDottedDatasetAndItemNames()
    {
        var service = CreateService();
        service.AddData("Customer", "Name", "Ada");

        Assert.True(service.IsValidDataIdentifier("Customer.Name"));
        Assert.False(service.IsValidDataIdentifier("Customer.Missing"));
        Assert.False(service.IsValidDataIdentifier("Missing.Name"));
        Assert.False(service.IsValidDataIdentifier("Customer.Name.Extra"));
    }
}