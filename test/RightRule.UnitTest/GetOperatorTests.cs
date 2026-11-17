using RightRule;

namespace RightRuleTests;

public class GetOperatorTests
{
    [Fact]
    public void GetOperatorDetails()
    {
        var rr = new RightRule.RightRule().CreateSession();
        var operatorDetails = rr.OperatorService.GetOperators();
        Assert.NotNull(operatorDetails);
    }
}
