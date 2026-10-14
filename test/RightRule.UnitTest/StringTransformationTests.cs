using RightRule.DataServices;
using RightRule.DataServices.DataValidator;
using RightRule.DataTransformer;

namespace RightRuleTests;

public class StringTransformationTests
{
    [Fact]
    public void TransformSimpleString_NoChange()
    {
        IDataValidatorService dvs = new DataValidatorService();
        DataService ds = new DataService(dvs);
        IInterpolate sInt = new StringInterpolator(ds);
        var trans = new InterpolatorService(sInt);

        var input = "Hello World";
        var output = trans.Interpolate(input).ToString();

        var expected = input;

        Assert.Equal(expected, output);
    }

    [Fact]
    public void TransformSimpleString_NoChangeNoData()
    {
        IDataValidatorService dvs = new DataValidatorService();
        DataService ds = new DataService(dvs);
        IInterpolate sInt = new StringInterpolator(ds);
        var trans = new InterpolatorService(sInt);

        var input = "Hello World from $name thanks";
        var output = trans.Interpolate(input).ToString();

        var expected = input;

        Assert.Equal(expected, output);
    }

    [Fact]
    public void TransformSimpleString_NoChangeNoData2()
    {
        IDataValidatorService dvs = new DataValidatorService();
        DataService ds = new DataService(dvs);
        IInterpolate sInt = new StringInterpolator(ds);
        var trans = new InterpolatorService(sInt);

        var input = "Hello World from $name $what.ever thanks";
        var output = trans.Interpolate(input).ToString();

        var expected = input;

        Assert.Equal(expected, output);
    }

    [Fact]
    public void TransformSimpleString_DottedDataInMiddle()
    {
        IDataValidatorService dvs = new DataValidatorService();
        DataService ds = new DataService(dvs);
        IInterpolate sInt = new StringInterpolator(ds);
        var trans = new InterpolatorService(sInt);

        ds.AddData("a", "a", "FoundA");

        var input = "Hello World from $a.a thanks";
        var output = trans.Interpolate(input).ToString();

        var expected = "Hello World from FoundA thanks";

        Assert.Equal(expected, output);
    }

    [Fact]
    public void TransformSimpleString_NoDotDataInMiddle()
    {
        IDataValidatorService dvs = new DataValidatorService();
        DataService ds = new DataService(dvs);
        IInterpolate sInt = new StringInterpolator(ds);
        var trans = new InterpolatorService(sInt);

        ds.AddData("a", "a", "FoundA");

        var input = "Hello World from $a thanks";
        var output = trans.Interpolate(input).ToString();

        var expected = "Hello World from FoundA thanks";

        Assert.Equal(expected, output);
    }

    [Fact]
    public void TransformSimpleString_NoDotDataStart()
    {
        IDataValidatorService dvs = new DataValidatorService();
        DataService ds = new DataService(dvs);
        IInterpolate sInt = new StringInterpolator(ds);
        var trans = new InterpolatorService(sInt);

        ds.AddData("a", "a", "FoundA");

        var input = "$a Hello World from thanks";
        var output = trans.Interpolate(input).ToString();

        var expected = "FoundA Hello World from thanks";

        Assert.Equal(expected, output);
    }


    [Fact]
    public void TransformSimpleString_DottedDataStart()
    {
        IDataValidatorService dvs = new DataValidatorService();
        DataService ds = new DataService(dvs);
        IInterpolate sInt = new StringInterpolator(ds);
        var trans = new InterpolatorService(sInt);

        ds.AddData("a", "a", "FoundA");

        var input = "$a.a Hello World from thanks";
        var output = trans.Interpolate(input).ToString();

        var expected = "FoundA Hello World from thanks";

        Assert.Equal(expected, output);
    }

    [Fact]
    public void TransformSimpleString_NoDotDataEnd()
    {
        IDataValidatorService dvs = new DataValidatorService();
        DataService ds = new DataService(dvs);
        IInterpolate sInt = new StringInterpolator(ds);
        var trans = new InterpolatorService(sInt);

        ds.AddData("a", "a", "FoundA");

        var input = "Hello World from thanks $a";
        var output = trans.Interpolate(input).ToString();

        var expected = "Hello World from thanks FoundA";

        Assert.Equal(expected, output);
    }

    [Fact]
    public void TransformSimpleString_DottedDataEnd()
    {
        IDataValidatorService dvs = new DataValidatorService();
        DataService ds = new DataService(dvs);
        IInterpolate sInt = new StringInterpolator(ds);
        var trans = new InterpolatorService(sInt);

        ds.AddData("a", "a", "FoundA");

        var input = "Hello World from thanks $a.a";
        var output = trans.Interpolate(input).ToString();

        var expected = "Hello World from thanks FoundA";

        Assert.Equal(expected, output);
    }

    [Fact]
    public void TransformComplexString()
    {
        IDataValidatorService dvs = new DataValidatorService();
        DataService ds = new DataService(dvs);
        IInterpolate sInt = new StringInterpolator(ds);
        var trans = new InterpolatorService(sInt);

        ds.AddData("a", "a", "FoundA");
        ds.AddData("b", "b", "FoundB");
        ds.AddData("c", "c", "FoundC");
        ds.AddData("d", "d", "FoundD");

        var input = "Hello World from thanks $a.a $b.b $c $d";
        var output = trans.Interpolate(input).ToString();

        var expected = "Hello World from thanks FoundA FoundB FoundC FoundD";

        Assert.Equal(expected, output);
    }

    [Fact]
    public void TransformJsonString_NoDots()
    {
        var json = "{\"firstname\": \"$firstname\",\"surname\": \"$surname\",\"postcode\": \"$postcode\"}";

        IDataValidatorService dvs = new DataValidatorService();
        DataService ds = new DataService(dvs);
        IInterpolate sInt = new StringInterpolator(ds);
        var trans = new InterpolatorService(sInt);

        ds.AddData("firstname", "firstname", "John");
        ds.AddData("surname", "surname", "Doe");
        ds.AddData("postcode", "postcode", "IP6 0NL");

        var input = json;
        var output = trans.Interpolate(input).ToString();

        var expected = "{\"firstname\": \"John\",\"surname\": \"Doe\",\"postcode\": \"IP6 0NL\"}";

        Assert.Equal(expected, output);
    }

    [Fact]
    public void TransformJsonString_Dotted()
    {
        var json = "{\"firstname\": \"$person.firstname\",\"surname\": \"$person.surname\",\"postcode\": \"$person.postcode\"}";

        IDataValidatorService dvs = new DataValidatorService();
        DataService ds = new DataService(dvs);
        IInterpolate sInt = new StringInterpolator(ds);
        var trans = new InterpolatorService(sInt);

        ds.AddData("person", "firstname", "John");
        ds.AddData("person", "surname", "Doe");
        ds.AddData("person", "postcode", "IP6 0NL");

        var input = json;
        var output = trans.Interpolate(input).ToString();

        var expected = "{\"firstname\": \"John\",\"surname\": \"Doe\",\"postcode\": \"IP6 0NL\"}";

        Assert.Equal(expected, output);
    }

    [Fact]
    public void TransformJsonStringAndNumbers_Dotted()
    {
        var json = "{\"firstname\": \"$person.firstname\",\"surname\": \"$person.surname\",\"postcode\": \"$person.postcode\",\"discount\": $order.discount}";

        IDataValidatorService dvs = new DataValidatorService();
        DataService ds = new DataService(dvs);
        IInterpolate sInt = new StringInterpolator(ds);
        var trans = new InterpolatorService(sInt);

        ds.AddData("person", "firstname", "John");
        ds.AddData("person", "surname", "Doe");
        ds.AddData("person", "postcode", "IP6 0NL");
        ds.AddData("order", "discount", 15);

        var input = json;
        var output = trans.Interpolate(input).ToString();

        var expected = "{\"firstname\": \"John\",\"surname\": \"Doe\",\"postcode\": \"IP6 0NL\",\"discount\": 15}";

        Assert.Equal(expected, output);
    }
}
