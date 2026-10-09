using RightRule.OperatorServices;
using RightRule.OperatorServices.OperatorLoader;
using RightRule.System.Common.Operator.Models;

namespace RightRuleTests;

public class OperatorRegistrationTests
{
    [Fact]
    public void TestUniqueOperatorName_Unique()
    {
        var operators = new List<Operator> { new IsSmallerThan() };
        var isbig = new IsBiggerThan();

        var nameFound = Helpers.OperatorNameExists(operators, isbig);

        Assert.False(nameFound);
    }

    [Fact]
    public void TestUniqueOperatorName_False()
    {
        var operators = new List<Operator> { new IsBiggerThan() };
        var isbig = new IsBiggerThan();

        var nameFound = Helpers.OperatorNameExists(operators, isbig);
        Assert.True(nameFound);
    }

    [Fact]
    public void TestUniqueAliases()
    {
        Dictionary<string, Operator> aliases = new Dictionary<string, Operator>();
        aliases.Add("IsBiggerThan", new IsBiggerThan());

        var unique = Helpers.OperatorAliasesUnique(aliases, new string[] { "IsSmallerThan" });
        Assert.True(unique);
    }

    [Fact]
    public void TestUniqueAliases_False()
    {
        Dictionary<string, Operator> aliases = new Dictionary<string, Operator>();
        aliases.Add("IsBiggerThan", new IsBiggerThan());

        var unique = Helpers.OperatorAliasesUnique(aliases, new string[] { "IsBiggerThan" });
        Assert.False(unique);
    }

    [Fact]
    public void TestCleanAliasesByList()
    {
        var aliases = new List<string> { "IsBiggerThan", "IsSmallerThan", "", "  ", "IsBiggerThan" };
        var cleanedAliases = Helpers.CleanAliases(aliases);
        Assert.Equal(2, cleanedAliases.Length);
        Assert.Contains("IsBiggerThan", cleanedAliases);
        Assert.Contains("IsSmallerThan", cleanedAliases);
    }

    [Fact]
    public void TestCleanAliasesByOperator()
    {
        var op = new IsBiggerThan();
        var cleanedAliases = Helpers.MakeCleanAliases(op);
        Assert.Single(cleanedAliases);
        Assert.Contains("IsBiggerThan", cleanedAliases);
    }

    [Fact]
    public void RegisterOperatorTest()
    {
        var rr = new RightRule.RightRule();
        Assert.NotNull(rr);
        Assert.NotNull(rr.OperatorService.Operators);
        Assert.NotNull(rr.OperatorService.OperatorAliases);
        Assert.Equal(9, rr.OperatorService.Operators.Count);
        Assert.Equal(22, rr.OperatorService.OperatorAliases.Count);

        var op = new IsBiggerThan();
        Assert.NotNull(op);

        var isRegistered = rr.RegisterOperator(op);
        Assert.Equal(RightRule.Models.RightRuleStatusEnum.SUCCESS, isRegistered.Status);
        Assert.Equal(10, rr.OperatorService.Operators.Count);
        Assert.Equal(23, rr.OperatorService.OperatorAliases.Count);

        var ops = rr.GetOperators();
        Assert.NotNull(ops);
        Assert.Equal(10, ops.Count);
    }

    [Fact]
    public void RegisterOperator()
    {
        IOperatorService ops = new OperatorService();

        var op = new IsBiggerThan();
        Assert.NotNull(op);

        ops.RegisterOperator(op);

        Assert.NotNull(ops.Operators);
        Assert.NotNull(ops.OperatorAliases);
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


public class IsBiggerThan : Operator
{
    public override List<string> Aliases => ["IsBiggerThan", "", "IsBiggerthan"];

    protected override Task<OperatorResult> ExecuteAsync(object Left, object Right, object? OptionalData)
    {
        var result = (int)Left > (int)Right;
        if (result)
        {
            return Task.FromResult(OperatorResult.SuccessResult("IsBiggerThan", true));
        }
        return Task.FromResult(OperatorResult.FailureResult("IsBiggerThan", "Left is not bigger than Right"));
    }
}

public class IsSmallerThan : Operator
{
    public override List<string> Aliases => ["IsSmallerThan"];

    protected override Task<OperatorResult> ExecuteAsync(object Left, object Right, object? OptionalData)
    {
        var result = (int)Left < (int)Right;
        if (result)
        {
            return Task.FromResult(OperatorResult.SuccessResult("IsSmallerThan", true));
        }
        return Task.FromResult(OperatorResult.FailureResult("IsSmallerThan", "Left is not smaller than Right"));
    }
}
