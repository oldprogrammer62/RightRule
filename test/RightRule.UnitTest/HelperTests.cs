using RightRule;

namespace RightRuleTests;

public class HelperTests
{
    [Fact]
    public void ReadFromSettings_Test()
    {
        var rr = new RightRule.RightRule().CreateSession();

        var setting = rr.RightRuleOptions.CustomOperatorLibraryPath;

        Assert.NotNull(setting);
    }
}