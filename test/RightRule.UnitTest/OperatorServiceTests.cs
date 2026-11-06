using Microsoft.Extensions.Logging.Abstractions;
using RightRule.Models;
using RightRule.OperatorServices;
using RightRule.OperatorServices.OperatorLoader.CustomOperators;
using RightRule.System.Common.Operator.Models;

namespace RightRuleTests;

public class OperatorServiceTests
{
    private static CustomOperatorLoader CreateLoader() =>
        new(NullLogger<CustomOperatorLoader>.Instance);

    private static RightRuleOptions NoOperatorsOptions() => new()
    {
        LoadDefaultOperators = false,
        LoadCustomOperators = false
    };

    [Fact]
    public void Constructor_WithLoadingDisabled_StartsEmpty()
    {
        using var service = new OperatorService(CreateLoader(), NoOperatorsOptions());

        Assert.Empty(service.Operators);
        Assert.Empty(service.OperatorAliases);
    }

    [Fact]
    public void RegisterOperator_AddsOperatorAndAliases()
    {
        using var service = new OperatorService(CreateLoader(), NoOperatorsOptions());
        var op = new FirstTestOperator();

        var result = service.RegisterOperator(op);

        Assert.Equal(RightRuleStatusEnum.SUCCESS, result.Status);
        Assert.Contains(op, service.Operators);
        Assert.Same(op, service.OperatorAliases["first-alias"]);
        Assert.Same(op, service.OperatorAliases["first-alias-alt"]);
    }

    [Fact]
    public void RegisterOperator_AliasLookupIsCaseInsensitive()
    {
        using var service = new OperatorService(CreateLoader(), NoOperatorsOptions());
        var op = new FirstTestOperator();

        service.RegisterOperator(op);

        Assert.Same(op, service.OperatorAliases["first-alias"]);
    }

    [Fact]
    public void RegisterOperator_WhenAliasIsAlreadyRegistered_FailsAndKeepsExistingAlias()
    {
        using var service = new OperatorService(CreateLoader(), NoOperatorsOptions());
        var first = new FirstTestOperator();
        var conflicting = new ConflictingTestOperator();

        service.RegisterOperator(first);
        var result = service.RegisterOperator(conflicting);

        Assert.Equal(RightRuleStatusEnum.FAILURE, result.Status);
        Assert.Same(first, service.OperatorAliases["first-alias"]);
    }

    [Fact]
    public void GetOperators_ReturnsCopyOfRegisteredList()
    {
        using var service = new OperatorService(CreateLoader(), NoOperatorsOptions());
        var op = new FirstTestOperator();
        service.RegisterOperator(op);

        var operators = service.GetOperators();
        operators.Clear();

        Assert.Single(service.Operators);
        Assert.Same(op, service.Operators[0]);
    }

    [Fact]
    public void GetRegisteredAliases_ForOperator_ReturnsOnlyItsAliases()
    {
        using var service = new OperatorService(CreateLoader(), NoOperatorsOptions());
        var first = new FirstTestOperator();
        var second = new SecondTestOperator();
        service.RegisterOperator(first);
        service.RegisterOperator(second);

        var aliases = service.GetRegisteredAliases(second);

        Assert.Equal(2, aliases.Count);
        Assert.All(aliases.Values, value => Assert.Same(second, value));
        Assert.DoesNotContain("first-alias", aliases.Keys);
    }

    [Fact]
    public void RegisterAllOperators_WithLoadingDisabled_ClearsRegisteredOperators()
    {
        using var service = new OperatorService(CreateLoader(), NoOperatorsOptions());
        service.RegisterOperator(new FirstTestOperator());

        var result = service.RegisterAllOperators(NoOperatorsOptions());

        Assert.Equal(RightRuleStatusEnum.SUCCESS, result.Status);
        Assert.Empty(service.Operators);
        Assert.Empty(service.OperatorAliases);
    }

    private sealed class FirstTestOperator : Operator
    {
        public override List<string> Aliases => ["first-alias", "first-alias-alt"];

        protected override Task<OperatorResult> ExecuteAsync(
            object Left,
            object Right,
            object? OptionalData) =>
            Task.FromResult(OperatorResult.SuccessResult("FirstTestOperator", true));
    }

    private sealed class ConflictingTestOperator : Operator
    {
        public override List<string> Aliases => ["FIRST-ALIAS"];

        protected override Task<OperatorResult> ExecuteAsync(
            object Left,
            object Right,
            object? OptionalData) =>
            Task.FromResult(OperatorResult.SuccessResult("ConflictingTestOperator", true));
    }

    private sealed class SecondTestOperator : Operator
    {
        public override List<string> Aliases => ["second-alias", "second-alias-alt"];

        protected override Task<OperatorResult> ExecuteAsync(
            object Left,
            object Right,
            object? OptionalData) =>
            Task.FromResult(OperatorResult.SuccessResult("SecondTestOperator", true));
    }
}