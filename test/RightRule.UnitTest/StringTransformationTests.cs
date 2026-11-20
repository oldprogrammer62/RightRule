using Microsoft.Extensions.Logging.Testing;
using RightRule.DataAdaptors;
using RightRule.DataAdaptors.DataAdaptorLibrary;
using RightRule.DataServices;
using RightRule.DataServices.DataLoader;
using RightRule.DataServices.DataSerializer;
using RightRule.DataServices.DataValidator;
using RightRule.DataTransformer;
using RightRule.Factories.DataLoaderFactory;
using System.Reflection;

namespace RightRuleTests;

public class StringTransformationTests
{
    private static DataService CreateDataService()
    {
        IDataService? service = null;
        var proxy = DispatchProxy.Create<IDataService, ForwardingDataServiceProxy>();

        ((ForwardingDataServiceProxy)(object)proxy).Target = () => service!;

        service = new DataService(
            new DataValidatorService(),
            new DataSerializerService(),
            new DataLoaderService(new DataLoaderFactory(new FakeLogger<DataLoaderFactory>())),
            new DataAdaptorService(new DataAdaptorLibraryManager(new FakeLogger<DataAdaptorLibraryManager>())),
            new FakeLogger<DataService>());

        return (DataService)service;
    }

    [Fact]
    public void TransformSimpleString_NoChange()
    {
        DataService dataservice = CreateDataService();

        IInterpolate sInt = new StringInterpolator(dataservice);
        var trans = new InterpolatorService(sInt);

        var input = "Hello World";
        var output = trans.Interpolate(input).ToString();

        var expected = input;

        Assert.Equal(expected, output);
    }

    [Fact]
    public void TransformSimpleString_NoChangeNoData()
    {
        DataService dataservice = CreateDataService();

        IInterpolate sInt = new StringInterpolator(dataservice);
        var trans = new InterpolatorService(sInt);

        var input = "Hello World from $name thanks";
        var output = trans.Interpolate(input).ToString();

        var expected = input;

        Assert.Equal(expected, output);
    }

    [Fact]
    public void TransformSimpleString_NoChangeNoData2()
    {
        DataService dataservice = CreateDataService();

        IInterpolate sInt = new StringInterpolator(dataservice);
        var trans = new InterpolatorService(sInt);

        var input = "Hello World from $name $what.ever thanks";
        var output = trans.Interpolate(input).ToString();

        var expected = input;

        Assert.Equal(expected, output);
    }

    [Fact]
    public void TransformSimpleString_DottedDataInMiddle()
    {
        DataService dataservice = CreateDataService();

        IInterpolate sInt = new StringInterpolator(dataservice);
        var trans = new InterpolatorService(sInt);

        dataservice.AddData("a", "a", "FoundA");

        var input = "Hello World from $a.a thanks";
        var output = trans.Interpolate(input).ToString();

        var expected = "Hello World from FoundA thanks";

        Assert.Equal(expected, output);
    }

    [Fact]
    public void TransformSimpleString_NoDotDataInMiddle()
    {
        DataService dataservice = CreateDataService();

        IInterpolate sInt = new StringInterpolator(dataservice);
        var trans = new InterpolatorService(sInt);

        dataservice.AddData("a", "a", "FoundA");

        var input = "Hello World from $a thanks";
        var output = trans.Interpolate(input).ToString();

        var expected = "Hello World from FoundA thanks";

        Assert.Equal(expected, output);
    }

    [Fact]
    public void TransformSimpleString_NoDotDataStart()
    {
        DataService dataservice = CreateDataService();

        IInterpolate sInt = new StringInterpolator(dataservice);
        var trans = new InterpolatorService(sInt);

        dataservice.AddData("a", "a", "FoundA");

        var input = "$a Hello World from thanks";
        var output = trans.Interpolate(input).ToString();

        var expected = "FoundA Hello World from thanks";

        Assert.Equal(expected, output);
    }


    [Fact]
    public void TransformSimpleString_DottedDataStart()
    {
        DataService dataservice = CreateDataService();

        IInterpolate sInt = new StringInterpolator(dataservice);
        var trans = new InterpolatorService(sInt);

        dataservice.AddData("a", "a", "FoundA");

        var input = "$a.a Hello World from thanks";
        var output = trans.Interpolate(input).ToString();

        var expected = "FoundA Hello World from thanks";

        Assert.Equal(expected, output);
    }

    [Fact]
    public void TransformSimpleString_NoDotDataEnd()
    {
        DataService dataservice = CreateDataService();

        IInterpolate sInt = new StringInterpolator(dataservice);
        var trans = new InterpolatorService(sInt);

        dataservice.AddData("a", "a", "FoundA");

        var input = "Hello World from thanks $a";
        var output = trans.Interpolate(input).ToString();

        var expected = "Hello World from thanks FoundA";

        Assert.Equal(expected, output);
    }

    [Fact]
    public void TransformSimpleString_DottedDataEnd()
    {
        DataService dataservice = CreateDataService();

        IInterpolate sInt = new StringInterpolator(dataservice);
        var trans = new InterpolatorService(sInt);

        dataservice.AddData("a", "a", "FoundA");

        var input = "Hello World from thanks $a.a";
        var output = trans.Interpolate(input).ToString();

        var expected = "Hello World from thanks FoundA";

        Assert.Equal(expected, output);
    }

    [Fact]
    public void TransformComplexString()
    {
        DataService dataservice = CreateDataService();

        IInterpolate sInt = new StringInterpolator(dataservice);
        var trans = new InterpolatorService(sInt);

        dataservice.AddData("a", "a", "FoundA");
        dataservice.AddData("b", "b", "FoundB");
        dataservice.AddData("c", "c", "FoundC");
        dataservice.AddData("d", "d", "FoundD");

        var input = "Hello World from thanks $a.a $b.b $c $d";
        var output = trans.Interpolate(input).ToString();

        var expected = "Hello World from thanks FoundA FoundB FoundC FoundD";

        Assert.Equal(expected, output);
    }

    [Fact]
    public void TransformJsonString_NoDots()
    {
        var json = "{\"firstname\": \"$firstname\",\"surname\": \"$surname\",\"postcode\": \"$postcode\"}";

        DataService dataservice = CreateDataService();

        IInterpolate sInt = new StringInterpolator(dataservice);
        var trans = new InterpolatorService(sInt);

        dataservice.AddData("firstname", "firstname", "John");
        dataservice.AddData("surname", "surname", "Doe");
        dataservice.AddData("postcode", "postcode", "IP6 0NL");

        var input = json;
        var output = trans.Interpolate(input).ToString();

        var expected = "{\"firstname\": \"John\",\"surname\": \"Doe\",\"postcode\": \"IP6 0NL\"}";

        Assert.Equal(expected, output);
    }

    [Fact]
    public void TransformJsonString_Dotted()
    {
        var json = "{\"firstname\": \"$person.firstname\",\"surname\": \"$person.surname\",\"postcode\": \"$person.postcode\"}";

        DataService dataservice = CreateDataService();

        IInterpolate sInt = new StringInterpolator(dataservice);
        var trans = new InterpolatorService(sInt);

        dataservice.AddData("person", "firstname", "John");
        dataservice.AddData("person", "surname", "Doe");
        dataservice.AddData("person", "postcode", "IP6 0NL");

        var input = json;
        var output = trans.Interpolate(input).ToString();

        var expected = "{\"firstname\": \"John\",\"surname\": \"Doe\",\"postcode\": \"IP6 0NL\"}";

        Assert.Equal(expected, output);
    }

    [Fact]
    public void TransformJsonStringAndNumbers_Dotted()
    {
        var json = "{\"firstname\": \"$person.firstname\",\"surname\": \"$person.surname\",\"postcode\": \"$person.postcode\",\"discount\": $order.discount}";

        DataService dataservice = CreateDataService();

        IInterpolate sInt = new StringInterpolator(dataservice);
        var trans = new InterpolatorService(sInt);

        dataservice.AddData("person", "firstname", "John");
        dataservice.AddData("person", "surname", "Doe");
        dataservice.AddData("person", "postcode", "IP6 0NL");
        dataservice.AddData("order", "discount", 15);

        var input = json;
        var output = trans.Interpolate(input).ToString();

        var expected = "{\"firstname\": \"John\",\"surname\": \"Doe\",\"postcode\": \"IP6 0NL\",\"discount\": 15}";

        Assert.Equal(expected, output);
    }

    private class ForwardingDataServiceProxy : DispatchProxy
    {
        public Func<IDataService>? Target { get; set; }

        protected override object? Invoke(
            System.Reflection.MethodInfo? targetMethod,
            object?[]? args)
        {
            return targetMethod!.Invoke(Target!(), args);
        }
    }
}
