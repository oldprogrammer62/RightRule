namespace RightRuleTests;

public class OperatorUnloadTests
{
    [Fact]
    public void TestItWorks_UnloadOperator_DoesNotContain()
    {
        // Force RightRule to load all operators
        RightRule.RightRule rr = new RightRule.RightRule();

        // Unload the "Contains" operator
        bool result = rr.UnloadOperator("Contains");
        Assert.True(result);
    }

    [Fact]
    public void TestItWorks_UnloadOperator_GreaterThanOrEquals()
    {
        // Force RightRule to load all operators
        RightRule.RightRule rr = new RightRule.RightRule();

        // Unload the "GreaterThanOrEquals" operator
        bool result = rr.UnloadOperator("GreaterThanOrEquals");
        Assert.True(result);
    }
}
