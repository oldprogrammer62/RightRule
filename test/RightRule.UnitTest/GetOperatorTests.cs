namespace RightRuleTests;

public class GetOperatorTests
{
    [Fact]
    public void GetOperatorDetails()
    {
        BeSoft.IRightRule rr = new BeSoft.RightRule();
        var operatorDetails = rr.GetOperatorDetails();
        Assert.NotNull(operatorDetails);
    }
}
