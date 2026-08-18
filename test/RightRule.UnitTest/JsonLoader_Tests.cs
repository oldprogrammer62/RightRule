using System.Data.Common;

namespace RightRuleTests;

public class JsonLoader_Tests
{
    [Fact]
    public void Json_Load_Test()
    {
        BeSoft.IRightRule rr = new BeSoft.RightRule();

        var loaderFactory = new RightRule.Factories.Loaders.LoaderFactory(rr);

        var loader = loaderFactory.BuildLoader("JSON");
        Assert.NotNull(loader);

        var rules = loader.Load();

        Assert.NotNull(rules);
    }
}
