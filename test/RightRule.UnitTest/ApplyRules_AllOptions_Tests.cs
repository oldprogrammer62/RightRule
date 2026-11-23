using RightRule.Models;

namespace RightRuleTests;

public class ApplyRules_AllOptions_Tests
{
    [Fact]
    public void ApplyRule_True()
    {
        RightRuleOptions options = new RightRuleOptions
        {
            DataCheck = false,
            Diagnostics = true,
            LoadCustomOperators = true,
            LoadDefaultOperators = true
        };
        var rr = new RightRule.RightRule(options).CreateSession();
        rr.DataService.RegisterData("a", 100);
        rr.DataService.RegisterData("b", 200);

        var rule1 = rr.RuleService.BuildRule()
            .Number(1)
            .Name("Rule1")
            .Context("Context")
            .Statement("a = 100")
            .Decision("TRUE")
            .Build()
            .Register(rr.RuleService);
        Assert.NotNull(rule1);
        Assert.NotNull(rule1.RegisteredRule);
        Assert.Equal(RegistrationStatusEnum.SUCCESS, rule1.Status);

        var result = rr.RuleService.ApplyRule("Context");

        Assert.NotNull(result);
        Assert.Equal(rule1.RegisteredRule.Decision, result.Decision);
    }

    [Fact]
    public void ApplyRule_False_StdDefaultRule()
    {
        RightRuleOptions options = new RightRuleOptions
        {
            DataCheck = false,
            Diagnostics = true,
            LoadCustomOperators = true,
            LoadDefaultOperators = true
        };
        var rr = new RightRule.RightRule(options).CreateSession();
        rr.DataService.RegisterData("a", 99);
        rr.DataService.RegisterData("b", 200);

        var rule1 = rr.RuleService.BuildRule()
            .Number(1)
            .Name("Rule1")
            .Context("Context")
            .Statement("a = 100")
            .Decision("TRUE")
            .Build()
            .Register(rr.RuleService);
        Assert.NotNull(rule1);
        Assert.NotNull(rule1.RegisteredRule);
        Assert.Equal(RegistrationStatusEnum.SUCCESS, rule1.Status);

        var result = rr.RuleService.ApplyRule("Context");
        Assert.NotNull(result);

        var defaultRule = rr.RuleService.GetDefaultRule("Context");
        Assert.NotNull(defaultRule);

        Assert.Equal(defaultRule.Decision, result.Decision);
        Assert.Equal(RightRuleResultEnum.DEFAULT, result.Status);
    }

    [Fact]
    public void ApplyRule_False_NewDefaultRule()
    {
        RightRuleOptions options = new RightRuleOptions
        {
            DataCheck = false,
            Diagnostics = true,
            LoadCustomOperators = true,
            LoadDefaultOperators = true
        };
        var rr = new RightRule.RightRule(options).CreateSession();
        rr.DataService.RegisterData("a", 99);
        rr.DataService.RegisterData("b", 200);

        var rule1 = rr.RuleService.BuildRule()
            .Number(1)
            .Name("Rule1")
            .Context("Context")
            .Statement("a = 100")
            .Decision("TRUE")
            .Build()
            .Register(rr.RuleService);
        Assert.NotNull(rule1);
        Assert.NotNull(rule1.RegisteredRule);
        Assert.Equal(RegistrationStatusEnum.SUCCESS, rule1.Status);

        var defRule = rr.RuleService.BuildDefaultRule()
            .Name("NewDefaultRule")
            .Context("Context")
            .Decision("DefaultRule")
            .Build()
            .Register(rr.RuleService);
        Assert.NotNull(defRule);
        Assert.NotNull(defRule.RegisteredRule);
           
        var result = rr.RuleService.ApplyRule("Context");
        Assert.NotNull(result);

        var defaultRule = rr.RuleService.GetDefaultRule("Context");
        Assert.NotNull(defaultRule);
        Assert.Equal(defaultRule.Decision, defRule.RegisteredRule.Decision);

        Assert.Equal(defaultRule.Decision, result.Decision);
        Assert.Equal(RightRuleResultEnum.DEFAULT, result.Status);
    }

    [Fact]
    public void ApplyRules_SingleNotSpec_True()
    {
        RightRuleOptions options = new RightRuleOptions
        {
            DataCheck = false,
            Diagnostics = true,
            LoadCustomOperators = true,
            LoadDefaultOperators = true
        };
        var rr = new RightRule.RightRule(options).CreateSession();
        rr.DataService.RegisterData("a", 99);
        rr.DataService.RegisterData("b", 200);

        var rule1 = rr.RuleService.BuildRule()
            .Number(1)
            .Name("Rule1")
            .Context("Context")
            .Statement("a = 100")
            .Decision("TRUE1")
            .Build()
            .Register(rr.RuleService);
        Assert.NotNull(rule1);
        Assert.NotNull(rule1.RegisteredRule);
        Assert.Equal(RegistrationStatusEnum.SUCCESS, rule1.Status);

        var rule2 = rr.RuleService.BuildRule()
            .Number(2)
            .Name("Rule2")
            .Context("Context")
            .Statement("b = 200")
            .Decision("TRUE2")
            .Build()
            .Register(rr.RuleService);
        Assert.NotNull(rule2);
        Assert.NotNull(rule2.RegisteredRule);
        Assert.Equal(RegistrationStatusEnum.SUCCESS, rule2.Status);

        var result = rr.RuleService.ApplyRules("Context");
        Assert.NotNull(result);
        Assert.Single(result);

        Assert.Equal(rule2.RegisteredRule.Decision, result.Single().Decision);
    }

    [Fact]
    public void ApplyRules_False_SingleNotSpec_StdDefaultRule()
    {
        RightRuleOptions options = new RightRuleOptions
        {
            DataCheck = false,
            Diagnostics = true,
            LoadCustomOperators = true,
            LoadDefaultOperators = true
        };
        var rr = new RightRule.RightRule(options).CreateSession();
        rr.DataService.RegisterData("a", 99);
        rr.DataService.RegisterData("b", 99);

        var rule1 = rr.RuleService.BuildRule()
            .Number(1)
            .Name("Rule1")
            .Context("Context")
            .Statement("a = 100")
            .Decision("TRUE1")
            .Build()
            .Register(rr.RuleService);
        Assert.NotNull(rule1);
        Assert.NotNull(rule1.RegisteredRule);
        Assert.Equal(RegistrationStatusEnum.SUCCESS, rule1.Status);

        var rule2 = rr.RuleService.BuildRule()
            .Number(2)
            .Name("Rule2")
            .Context("Context")
            .Statement("b = 200")
            .Decision("TRUE2")
            .Build()
            .Register(rr.RuleService);
        Assert.NotNull(rule2);
        Assert.NotNull(rule2.RegisteredRule);
        Assert.Equal(RegistrationStatusEnum.SUCCESS, rule2.Status);

        var result = rr.RuleService.ApplyRules("Context");
        Assert.NotNull(result);
        Assert.Single(result);

        var defaultRule = rr.RuleService.GetDefaultRule("Context");
        Assert.NotNull(defaultRule);

        Assert.Equal(defaultRule.Decision, result.Single().Decision);
        Assert.Equal(RightRuleResultEnum.DEFAULT, result.Single().Status);
    }

    [Fact]
    public void ApplyRules_False_SingleNotSpec_NewDefaultRule()
    {
        RightRuleOptions options = new RightRuleOptions
        {
            DataCheck = false,
            Diagnostics = true,
            LoadCustomOperators = true,
            LoadDefaultOperators = true
        };
        var rr = new RightRule.RightRule(options).CreateSession();
        rr.DataService.RegisterData("a", 99);
        rr.DataService.RegisterData("b", 99);

        var rule1 = rr.RuleService.BuildRule()
            .Number(1)
            .Name("Rule1")
            .Context("Context")
            .Statement("a = 100")
            .Decision("TRUE1")
            .Build()
            .Register(rr.RuleService);
        Assert.NotNull(rule1);
        Assert.NotNull(rule1.RegisteredRule);
        Assert.Equal(RegistrationStatusEnum.SUCCESS, rule1.Status);

        var rule2 = rr.RuleService.BuildRule()
            .Number(2)
            .Name("Rule2")
            .Context("Context")
            .Statement("b = 200")
            .Decision("TRUE2")
            .Build()
            .Register(rr.RuleService);
        Assert.NotNull(rule2);
        Assert.NotNull(rule2.RegisteredRule);
        Assert.Equal(RegistrationStatusEnum.SUCCESS, rule2.Status);

        var defRule = rr.RuleService.BuildDefaultRule()
            .Name("NewDefaultRule")
            .Context("Context")
            .Decision("DefaultRule")
            .Build()
            .Register(rr.RuleService);
        Assert.NotNull(defRule);
        Assert.NotNull(defRule.RegisteredRule);

        var result = rr.RuleService.ApplyRules("Context");
        Assert.NotNull(result);
        Assert.Single(result);

        var defaultRule = rr.RuleService.GetDefaultRule("Context");
        Assert.NotNull(defaultRule);
        Assert.Equal(defaultRule.Decision, defRule.RegisteredRule.Decision);

        Assert.Equal(defaultRule.Decision, result.Single().Decision);
        Assert.Equal(RightRuleResultEnum.DEFAULT, result.Single().Status);
    }

    [Fact]
    public void ApplyRules_SingleSpec_True()
    {
        RightRuleOptions options = new RightRuleOptions
        {
            DataCheck = false,
            Diagnostics = true,
            LoadCustomOperators = true,
            LoadDefaultOperators = true
        };
        var rr = new RightRule.RightRule(options).CreateSession();
        rr.DataService.RegisterData("a", 99);
        rr.DataService.RegisterData("b", 200);

        var rule1 = rr.RuleService.BuildRule()
            .Number(1)
            .Name("Rule1")
            .Context("Context")
            .Statement("a = 100")
            .Decision("TRUE1")
            .Build()
            .Register(rr.RuleService);
        Assert.NotNull(rule1);
        Assert.NotNull(rule1.RegisteredRule);
        Assert.Equal(RegistrationStatusEnum.SUCCESS, rule1.Status);

        var rule2 = rr.RuleService.BuildRule()
            .Number(2)
            .Name("Rule2")
            .Context("Context")
            .Statement("b = 200")
            .Decision("TRUE2")
            .Build()
            .Register(rr.RuleService);
        Assert.NotNull(rule2);
        Assert.NotNull(rule2.RegisteredRule);
        Assert.Equal(RegistrationStatusEnum.SUCCESS, rule2.Status);

        var result = rr.RuleService.ApplyRules("Context", RightRule.LanguageProcessor.Evaluator.EvaluationTypeEnum.FIRST);
        Assert.NotNull(result);
        Assert.Single(result);

        Assert.Equal(rule2.RegisteredRule.Decision, result.Single().Decision);
    }

    [Fact]
    public void ApplyRules_False_SingleSpec_StdDefaultRule()
    {
        RightRuleOptions options = new RightRuleOptions
        {
            DataCheck = false,
            Diagnostics = true,
            LoadCustomOperators = true,
            LoadDefaultOperators = true
        };
        var rr = new RightRule.RightRule(options).CreateSession();
        rr.DataService.RegisterData("a", 99);
        rr.DataService.RegisterData("b", 99);

        var rule1 = rr.RuleService.BuildRule()
            .Number(1)
            .Name("Rule1")
            .Context("Context")
            .Statement("a = 100")
            .Decision("TRUE1")
            .Build()
            .Register(rr.RuleService);
        Assert.NotNull(rule1);
        Assert.NotNull(rule1.RegisteredRule);
        Assert.Equal(RegistrationStatusEnum.SUCCESS, rule1.Status);

        var rule2 = rr.RuleService.BuildRule()
            .Number(2)
            .Name("Rule2")
            .Context("Context")
            .Statement("b = 200")
            .Decision("TRUE2")
            .Build()
            .Register(rr.RuleService);
        Assert.NotNull(rule2);
        Assert.NotNull(rule2.RegisteredRule);
        Assert.Equal(RegistrationStatusEnum.SUCCESS, rule2.Status);

        var result = rr.RuleService.ApplyRules("Context", RightRule.LanguageProcessor.Evaluator.EvaluationTypeEnum.FIRST);
        Assert.NotNull(result);
        Assert.Single(result);

        var defaultRule = rr.RuleService.GetDefaultRule("Context");
        Assert.NotNull(defaultRule);

        Assert.Equal(defaultRule.Decision, result.Single().Decision);
        Assert.Equal(RightRuleResultEnum.DEFAULT, result.Single().Status);
    }

    [Fact]
    public void ApplyRules_False_SingleSpec_NewDefaultRule()
    {
        RightRuleOptions options = new RightRuleOptions
        {
            DataCheck = false,
            Diagnostics = true,
            LoadCustomOperators = true,
            LoadDefaultOperators = true
        };
        var rr = new RightRule.RightRule(options).CreateSession();
        rr.DataService.RegisterData("a", 99);
        rr.DataService.RegisterData("b", 99);

        var rule1 = rr.RuleService.BuildRule()
            .Number(1)
            .Name("Rule1")
            .Context("Context")
            .Statement("a = 100")
            .Decision("TRUE1")
            .Build()
            .Register(rr.RuleService);
        Assert.NotNull(rule1);
        Assert.NotNull(rule1.RegisteredRule);
        Assert.Equal(RegistrationStatusEnum.SUCCESS, rule1.Status);

        var rule2 = rr.RuleService.BuildRule()
            .Number(2)
            .Name("Rule2")
            .Context("Context")
            .Statement("b = 200")
            .Decision("TRUE2")
            .Build()
            .Register(rr.RuleService);
        Assert.NotNull(rule2);
        Assert.NotNull(rule2.RegisteredRule);
        Assert.Equal(RegistrationStatusEnum.SUCCESS, rule2.Status);

        var defRule = rr.RuleService.BuildDefaultRule()
            .Name("NewDefaultRule")
            .Context("Context")
            .Decision("DefaultRule")
            .Build()
            .Register(rr.RuleService);
        Assert.NotNull(defRule);
        Assert.NotNull(defRule.RegisteredRule);

        var result = rr.RuleService.ApplyRules("Context", RightRule.LanguageProcessor.Evaluator.EvaluationTypeEnum.FIRST);
        Assert.NotNull(result);
        Assert.Single(result);

        var defaultRule = rr.RuleService.GetDefaultRule("Context");
        Assert.NotNull(defaultRule);
        Assert.Equal(defaultRule.Decision, defRule.RegisteredRule.Decision);

        Assert.Equal(defaultRule.Decision, result.Single().Decision);
        Assert.Equal(RightRuleResultEnum.DEFAULT, result.Single().Status);
    }

    [Fact]
    public void ApplyRules_AnySpec_BothTrue()
    {
        RightRuleOptions options = new RightRuleOptions
        {
            DataCheck = false,
            Diagnostics = true,
            LoadCustomOperators = true,
            LoadDefaultOperators = true
        };
        var rr = new RightRule.RightRule(options).CreateSession();
        rr.DataService.RegisterData("a", 100);
        rr.DataService.RegisterData("b", 200);

        var rule1 = rr.RuleService.BuildRule()
            .Number(1)
            .Name("Rule1")
            .Context("Context")
            .Statement("a = 100")
            .Decision("TRUE1")
            .Build()
            .Register(rr.RuleService);
        Assert.NotNull(rule1);
        Assert.NotNull(rule1.RegisteredRule);
        Assert.Equal(RegistrationStatusEnum.SUCCESS, rule1.Status);

        var rule2 = rr.RuleService.BuildRule()
            .Number(2)
            .Name("Rule2")
            .Context("Context")
            .Statement("b = 200")
            .Decision("TRUE2")
            .Build()
            .Register(rr.RuleService);
        Assert.NotNull(rule2);
        Assert.NotNull(rule2.RegisteredRule);
        Assert.Equal(RegistrationStatusEnum.SUCCESS, rule2.Status);

        var result = rr.RuleService.ApplyRules("Context", RightRule.LanguageProcessor.Evaluator.EvaluationTypeEnum.ANY);
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);

        Assert.Equal(rule1.RegisteredRule.Decision, result[0].Decision);
        Assert.Equal(RightRuleResultEnum.APPLICABLE, result[0].Status);

        Assert.Equal(rule2.RegisteredRule.Decision, result[1].Decision);
        Assert.Equal(RightRuleResultEnum.APPLICABLE, result[1].Status);
    }

    [Fact]
    public void ApplyRules_False_AnySpec_OneTrue()
    {
        RightRuleOptions options = new RightRuleOptions
        {
            DataCheck = false,
            Diagnostics = true,
            LoadCustomOperators = true,
            LoadDefaultOperators = true
        };
        var rr = new RightRule.RightRule(options).CreateSession();
        rr.DataService.RegisterData("a", 100);
        rr.DataService.RegisterData("b", 99);

        var rule1 = rr.RuleService.BuildRule()
            .Number(1)
            .Name("Rule1")
            .Context("Context")
            .Statement("a = 100")
            .Decision("TRUE1")
            .Build()
            .Register(rr.RuleService);
        Assert.NotNull(rule1);
        Assert.NotNull(rule1.RegisteredRule);
        Assert.Equal(RegistrationStatusEnum.SUCCESS, rule1.Status);

        var rule2 = rr.RuleService.BuildRule()
            .Number(2)
            .Name("Rule2")
            .Context("Context")
            .Statement("b = 200")
            .Decision("TRUE2")
            .Build()
            .Register(rr.RuleService);
        Assert.NotNull(rule2);
        Assert.NotNull(rule2.RegisteredRule);
        Assert.Equal(RegistrationStatusEnum.SUCCESS, rule2.Status);

        var result = rr.RuleService.ApplyRules("Context", RightRule.LanguageProcessor.Evaluator.EvaluationTypeEnum.ANY);
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);

        Assert.Equal(rule1.RegisteredRule.Decision, result[0].Decision);
        Assert.Equal(RightRuleResultEnum.APPLICABLE, result[0].Status);

        Assert.Equal(rule2.RegisteredRule.Decision, result[1].Decision);
        Assert.Equal(RightRuleResultEnum.NOTAPPLICABLE, result[1].Status);
    }

    [Fact]
    public void ApplyRules_False_AnySpec_BothFalse()
    {
        RightRuleOptions options = new RightRuleOptions
        {
            DataCheck = false,
            Diagnostics = true,
            LoadCustomOperators = true,
            LoadDefaultOperators = true
        };
        var rr = new RightRule.RightRule(options).CreateSession();
        rr.DataService.RegisterData("a", 99);
        rr.DataService.RegisterData("b", 99);

        var rule1 = rr.RuleService.BuildRule()
            .Number(1)
            .Name("Rule1")
            .Context("Context")
            .Statement("a = 100")
            .Decision("TRUE1")
            .Build()
            .Register(rr.RuleService);
        Assert.NotNull(rule1);
        Assert.NotNull(rule1.RegisteredRule);
        Assert.Equal(RegistrationStatusEnum.SUCCESS, rule1.Status);

        var rule2 = rr.RuleService.BuildRule()
            .Number(2)
            .Name("Rule2")
            .Context("Context")
            .Statement("b = 200")
            .Decision("TRUE2")
            .Build()
            .Register(rr.RuleService);
        Assert.NotNull(rule2);
        Assert.NotNull(rule2.RegisteredRule);
        Assert.Equal(RegistrationStatusEnum.SUCCESS, rule2.Status);

        var result = rr.RuleService.ApplyRules("Context", RightRule.LanguageProcessor.Evaluator.EvaluationTypeEnum.ANY);
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);

        Assert.Equal(rule1.RegisteredRule.Decision, result[0].Decision);
        Assert.Equal(RightRuleResultEnum.NOTAPPLICABLE, result[0].Status);

        Assert.Equal(rule2.RegisteredRule.Decision, result[1].Decision);
        Assert.Equal(RightRuleResultEnum.NOTAPPLICABLE, result[1].Status);
    }

    [Fact]
    public void ApplyRules_AllSpec_BothTrue()
    {
        RightRuleOptions options = new RightRuleOptions
        {
            DataCheck = false,
            Diagnostics = true,
            LoadCustomOperators = true,
            LoadDefaultOperators = true
        };
        var rr = new RightRule.RightRule(options).CreateSession();
        rr.DataService.RegisterData("a", 100);
        rr.DataService.RegisterData("b", 200);

        var rule1 = rr.RuleService.BuildRule()
            .Number(1)
            .Name("Rule1")
            .Context("Context")
            .Statement("a = 100")
            .Decision("TRUE1")
            .Build()
            .Register(rr.RuleService);
        Assert.NotNull(rule1);
        Assert.NotNull(rule1.RegisteredRule);
        Assert.Equal(RegistrationStatusEnum.SUCCESS, rule1.Status);

        var rule2 = rr.RuleService.BuildRule()
            .Number(2)
            .Name("Rule2")
            .Context("Context")
            .Statement("b = 200")
            .Decision("TRUE2")
            .Build()
            .Register(rr.RuleService);
        Assert.NotNull(rule2);
        Assert.NotNull(rule2.RegisteredRule);
        Assert.Equal(RegistrationStatusEnum.SUCCESS, rule2.Status);

        var result = rr.RuleService.ApplyRules("Context", RightRule.LanguageProcessor.Evaluator.EvaluationTypeEnum.ALL);
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);

        Assert.Equal(rule1.RegisteredRule.Decision, result[0].Decision);
        Assert.Equal(RightRuleResultEnum.APPLICABLE, result[0].Status);

        Assert.Equal(rule2.RegisteredRule.Decision, result[1].Decision);
        Assert.Equal(RightRuleResultEnum.APPLICABLE, result[1].Status);
    }

    [Fact]
    public void ApplyRules_False_AllSpec_OneTrue_StdDefaultRule()
    {
        RightRuleOptions options = new RightRuleOptions
        {
            DataCheck = false,
            Diagnostics = true,
            LoadCustomOperators = true,
            LoadDefaultOperators = true
        };
        var rr = new RightRule.RightRule(options).CreateSession();
        rr.DataService.RegisterData("a", 100);
        rr.DataService.RegisterData("b", 99);

        var rule1 = rr.RuleService.BuildRule()
            .Number(1)
            .Name("Rule1")
            .Context("Context")
            .Statement("a = 100")
            .Decision("TRUE1")
            .Build()
            .Register(rr.RuleService);
        Assert.NotNull(rule1);
        Assert.NotNull(rule1.RegisteredRule);
        Assert.Equal(RegistrationStatusEnum.SUCCESS, rule1.Status);

        var rule2 = rr.RuleService.BuildRule()
            .Number(2)
            .Name("Rule2")
            .Context("Context")
            .Statement("b = 200")
            .Decision("TRUE2")
            .Build()
            .Register(rr.RuleService);
        Assert.NotNull(rule2);
        Assert.NotNull(rule2.RegisteredRule);
        Assert.Equal(RegistrationStatusEnum.SUCCESS, rule2.Status);

        var result = rr.RuleService.ApplyRules("Context", RightRule.LanguageProcessor.Evaluator.EvaluationTypeEnum.ALL);
        Assert.NotNull(result);
        Assert.Single(result);

        var defaultRule = rr.RuleService.GetDefaultRule("Context");
        Assert.NotNull(defaultRule);

        Assert.Equal(defaultRule.Decision, result.Single().Decision);
        Assert.Equal(RightRuleResultEnum.DEFAULT, result.Single().Status);
    }

    [Fact]
    public void ApplyRules_False_AllSpec_BothFalse_NewDefaultRule()
    {
        RightRuleOptions options = new RightRuleOptions
        {
            DataCheck = false,
            Diagnostics = true,
            LoadCustomOperators = true,
            LoadDefaultOperators = true
        };
        var rr = new RightRule.RightRule(options).CreateSession();
        rr.DataService.RegisterData("a", 99);
        rr.DataService.RegisterData("b", 99);

        var rule1 = rr.RuleService.BuildRule()
            .Number(1)
            .Name("Rule1")
            .Context("Context")
            .Statement("a = 100")
            .Decision("TRUE1")
            .Build()
            .Register(rr.RuleService);
        Assert.NotNull(rule1);
        Assert.NotNull(rule1.RegisteredRule);
        Assert.Equal(RegistrationStatusEnum.SUCCESS, rule1.Status);

        var rule2 = rr.RuleService.BuildRule()
            .Number(2)
            .Name("Rule2")
            .Context("Context")
            .Statement("b = 200")
            .Decision("TRUE2")
            .Build()
            .Register(rr.RuleService);
        Assert.NotNull(rule2);
        Assert.NotNull(rule2.RegisteredRule);
        Assert.Equal(RegistrationStatusEnum.SUCCESS, rule2.Status);

        var defRule = rr.RuleService.BuildDefaultRule()
            .Name("NewDefaultRule")
            .Context("Context")
            .Decision("DefaultRule")
            .Build();
        Assert.NotNull(defRule);

        var defRuleReg = rr.RuleService.RegisterDefaultRule(defRule);
        Assert.NotNull(defRuleReg);

        var result = rr.RuleService.ApplyRules("Context", RightRule.LanguageProcessor.Evaluator.EvaluationTypeEnum.ALL);
        Assert.NotNull(result);
        Assert.Single(result);

        var defaultRule = rr.RuleService.GetDefaultRule("Context");
        Assert.NotNull(defaultRule);
        Assert.Equal(defaultRule.Decision, defRule.Decision);

        Assert.Equal(defaultRule.Decision, result.Single().Decision);
        Assert.Equal(RightRuleResultEnum.DEFAULT, result.Single().Status);
    }
}
