using RightRule;

namespace RightRuleTests;

public class HelperTests
{
    [Fact]
    public void ReadFromSettings_Test()
    {
        IRightRule rr = new RightRule.RightRule();

        var setting = rr.RightRuleOptions.CustomOperatorLibraryPath;

        Assert.NotNull(setting);
    }
}