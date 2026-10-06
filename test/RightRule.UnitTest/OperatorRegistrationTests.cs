using RightRule.System.Common;

namespace RightRuleTests;

public class OperatorRegistrationTests
{
    [Fact]
    public void RegisterOperatorTest()
    {
        var rr = new RightRule.RightRule();

        var op = new IsBiggerThan();
        Assert.NotNull(op);

        var isRegistered = rr.RegisterOperator(op);
        Assert.True(isRegistered);

        var ops = rr.GetOperators();
        Assert.NotNull(ops);
    }

    [Fact]
    public void TestIsBiggerThanOperator()
    {
        var rr = new RightRule.RightRule();
        var op = new IsBiggerThan();
        var isRegistered = rr.RegisterOperator(op);

        var left = 10;
        var right = 5;
        var leftReg = rr.RegisterData(nameof(left), left);
        var rightReg = rr.RegisterData(nameof(right), right);

        var rule = rr.BuildRule()
           .Number(1)
           .Name("BiggerThanRule")
           .Context("TestContext")
           .Statement("left IsBiggerThan right")
           .Decision("Success")
           .FalseDecision("Failure")
           .Build()
           .Register(rr.RuleService);
        Assert.NotNull(rule);

        var result = rr.ApplyRule("TestContext");
        Assert.NotNull(result);

        Assert.Equal(rule!.RegisteredRule!.Decision, result.Decision);
    }
}


public class IsBiggerThan : RightRule.System.Common.Operator
{
    public override List<string> Identifiers => ["IsBiggerThan"];

    protected override Task<OperatorResult> DoOperatorProcessAsync(object Left, object Right, object? OptionalData, bool Diagnostics)
    {
        var result = (int)Left > (int)Right;
        if (result)
        {
            return Task.FromResult(OperatorResult.SuccessResult("IsBiggerThan", true));
        }
        return Task.FromResult(OperatorResult.FailureResult("IsBiggerThan", "Left is not bigger than Right"));
    }
}
