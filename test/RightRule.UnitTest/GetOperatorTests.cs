using RightRule;

namespace RightRuleTests;

public class GetOperatorTests
{
    [Fact]
    public void GetOperatorDetails()
    {
        IRightRule rr = new RightRule.RightRule();
        var operatorDetails = rr.GetOperatorDetails();
        Assert.NotNull(operatorDetails);
    }
}
