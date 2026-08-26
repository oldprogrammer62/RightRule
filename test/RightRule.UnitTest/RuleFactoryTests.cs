//using RightRule.Factories.RuleFactory;
//using RightRule.RuleModels;

//namespace RightRuleTests;

//public class RuleFactoryTests
//{
//    [Fact]
//    public void BuildAppliedResult()
//    {
//        var rule = new AppliedRule()
//        {
//            Result = true,
//            Number = 1,
//            Name = "Name1",
//            Context = "Context",
//            Decision = "GOOD",
//            Message = "All good",
//            Statement = "a = b"
//        };

//        var result = RuleFactory.Complete(rule);
//        Assert.NotNull(result);
//        Assert.NotNull(result.Rule);
//        Assert.Equal(result.Rule.Name, rule.Name);
//    }

//    [Fact]
//    public void BuildDefaultResult()
//    {
//        var rule = new DefaultRule()
//        {
//            Result = true,
//            Number = -1,
//            Name = "Default_Name",
//            Context = "Default_Context",
//            Decision = "DEFAULT",
//            Message = "",
//            Statement = ""
//        };

//        var result = RuleFactory.Default(rule);
//        Assert.NotNull(result);
//        Assert.NotNull(result.Rule);
//        Assert.Equal(result.Rule.Name, rule.Name);
//    }

//    [Fact]
//    public void BuildErrorResult()
//    {
//        var rule = new ErrorRule()
//        {
//            Result = true,
//            Number = 1,
//            Name = "Name1",
//            Context = "Context",
//            Decision = "GOOD",
//            Message = "All good",
//            Statement = "a = b"
//        };

//        var result = RuleFactory.Error(rule);
//        Assert.NotNull(result);
//        Assert.NotNull(result.Rule);
//        Assert.Equal(result.Rule.Name, rule.Name);
//    }
//}