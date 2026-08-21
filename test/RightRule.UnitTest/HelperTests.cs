using BeSoft;

namespace RightRuleTests;

public class HelperTests
{
    [Fact]
    public void ReadFromSettings_Test()
    {
        IRightRule rr = new BeSoft.RightRule();

        const string Key = "CustomOperatorConfigPath";

        var setting = rr.GetConfigSettings(Key);

        Assert.NotNull(setting);
    }
}