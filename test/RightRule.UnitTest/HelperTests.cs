using RightRule;

namespace RightRuleTests;

public class HelperTests
{
    [Fact]
    public void ReadFromSettings_Test()
    {
        IRightRule rr = new RightRule.RightRule();

        const string Key = "CustomOperatorConfigPath";

        var setting = rr.GetConfigSettings(Key);

        Assert.NotNull(setting);
    }
}