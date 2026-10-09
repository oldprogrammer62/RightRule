using RightRule;

namespace RightRuleTests;

public class HelperTests
{
    [Fact]
    public void ReadFromSettings_Test()
    {
        IRightRule rr = new RightRule.RightRule();

        var setting = rr.GetRightRuleOptions().CustomOperatorLibraryPath;

        Assert.NotNull(setting);
    }
}