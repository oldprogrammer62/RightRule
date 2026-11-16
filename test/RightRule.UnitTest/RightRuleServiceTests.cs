using RightRule.LanguageProcessor.Evaluator;
using RightRule.LanguageProcessor.SyntaxChecker;
using RightRule.Models;
using RightRule.OperatorServices;
using RightRule.RightRuleService;
using RightRule.RuleServices.DefaultRuleRegistration;
using RightRule.RuleServices.RuleApplication;
using RightRule.RuleServices.RuleLoader;
using RightRule.RuleServices.RuleModels;
using RightRule.RuleServices.RuleModels.BaseModels;
using RightRule.System.Common.Operator.Models;
using System.Reflection;

namespace RightRuleTests;

public class RightRuleServiceTests
{
    [Fact]
    public async Task SyntaxCheckAsync_ForwardsStatementAndReturnsCheckerTask()
    {
        const string statement = "account.isActive == true";
        var expectedResult = CheckStatementResult.FailureResult();
        var expectedTask = Task.FromResult(expectedResult);
        var checker = new StatementSyntaxCheckerStub(expectedTask);
        using var service = CreateService(checker);

        var actualTask = service.SyntaxCheckAsync(statement);
        var actualResult = await actualTask;

        Assert.Equal(statement, checker.ReceivedStatement);
        Assert.Same(expectedTask, actualTask);
        Assert.Equal(expectedResult, actualResult);
    }

    private static RightRuleService CreateService(
        IStatementSyntaxChecker statementSyntaxChecker)
    {
        return new RightRuleService(
            statementSyntaxChecker);
    }

    private static T CreateProxy<T>() where T : class
    {
        return DispatchProxy.Create<T, InterfaceDispatchProxy>();
    }

    public class InterfaceDispatchProxy : DispatchProxy
    {
        protected override object? Invoke(MethodInfo? targetMethod, object?[]? args) => null;
    }

    private sealed class StatementSyntaxCheckerStub(Task<CheckStatementResult> result)
        : IStatementSyntaxChecker
    {
        public string? ReceivedStatement { get; private set; }

        public Task<CheckStatementResult> CheckAsync(string Statement)
        {
            ReceivedStatement = Statement;
            return result;
        }
    }

    private sealed class DefaultRuleRegisterStub : IDefaultRuleRegister
    {
        public List<DefaultRulePerContext>? GetDefaultRules() => [];

        public bool SetDefaultRule(DefaultRule rule) => true;

        public DefaultRule GetDefaultRule(string Context) => null!;
    }

    private sealed class RuleApplicationServiceStub : IRuleApplicationService
    {
        public Task<List<IProcessedRule>> ApplyRulesAsync(
            string Context,
            EvaluationTypeEnum evaluationType) =>
            Task.FromResult(new List<IProcessedRule>());
    }

    private sealed class RuleLoaderServiceStub : IRuleLoaderService
    {
        public List<DefinedRule>? LoadRules(string JsonData) => [];
    }

    private sealed class OperatorServiceStub : IOperatorService, IDisposable
    {
        public List<Operator> Operators { get; } = [];
        public Dictionary<string, Operator> OperatorAliases { get; } = [];

        public int DisposeCount { get; private set; }

        public OperatorRegistrationResult RegisterAllOperators() =>
            throw new NotSupportedException();

        public OperatorRegistrationResult RegisterAllOperators(RightRuleOptions options) =>
            throw new NotSupportedException();

        public OperatorRegistrationResult RegisterOperator(Operator Operator) =>
            throw new NotSupportedException();

        public List<Operator> GetOperators() => Operators;

        public Dictionary<string, Operator> GetRegisteredAliases() => OperatorAliases;

        public Dictionary<string, Operator> GetRegisteredAliases(Operator Operator) =>
            throw new NotSupportedException();

        public bool UnloadOperator(string OperatorName) =>
            throw new NotSupportedException();

        public void Dispose() => DisposeCount++;
    }
}