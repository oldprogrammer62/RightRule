namespace RightRuleTests;

public class HelperTests
{
    [Fact]
    public void ReadFromSettings_Test()
    {
        const string Key = "CustomOperatorConfigPath";

        var setting = RightRule.HelperFunctions.ConfigurationHelper.GetSetting(Key);

        Assert.NotNull(setting);
    }
}