using RightRule.Models;

namespace RightRuleTests;

public class RightRuleSessionTests
{
    [Fact]
    public void SingleNewSession()
    {
        RightRuleOptions options = new RightRuleOptions()
        {
            DataCheck = false,
            Diagnostics = true,
            LoadCustomOperators = true,
            LoadDefaultOperators = true
        };

        var rr = new RightRule.RightRule(options).CreateSession();

        Assert.Equal(options.DataCheck, rr.RightRuleOptions.DataCheck);
        Assert.Equal(options.Diagnostics, rr.RightRuleOptions.Diagnostics);
        Assert.Equal(options.LoadCustomOperators, rr.RightRuleOptions.LoadCustomOperators);
        Assert.Equal(options.LoadDefaultOperators, rr.RightRuleOptions.LoadDefaultOperators);
    }

    [Fact]
    public void MultipleNewSession()
    {
        RightRuleOptions options1 = new RightRuleOptions()
        {
            DataCheck = false,
            Diagnostics = true,
            LoadCustomOperators = true,
            LoadDefaultOperators = true
        };

        var rr1 = new RightRule.RightRule(options1).CreateSession();

        Assert.Equal(options1.DataCheck, rr1.RightRuleOptions.DataCheck);
        Assert.Equal(options1.Diagnostics, rr1.RightRuleOptions.Diagnostics);
        Assert.Equal(options1.LoadCustomOperators, rr1.RightRuleOptions.LoadCustomOperators);
        Assert.Equal(options1.LoadDefaultOperators, rr1.RightRuleOptions.LoadDefaultOperators);

        RightRuleOptions options2 = new RightRuleOptions()
        {
            DataCheck = false,
            Diagnostics = false,
            LoadCustomOperators = false,
            LoadDefaultOperators = false
        };

        var rr2 = new RightRule.RightRule(options2).CreateSession();

        Assert.Equal(options2.DataCheck, rr2.RightRuleOptions.DataCheck);
        Assert.Equal(options2.Diagnostics, rr2.RightRuleOptions.Diagnostics);
        Assert.Equal(options2.LoadCustomOperators, rr2.RightRuleOptions.LoadCustomOperators);
        Assert.Equal(options2.LoadDefaultOperators, rr2.RightRuleOptions.LoadDefaultOperators);

        Assert.Equal(options1.DataCheck, rr1.RightRuleOptions.DataCheck);
        Assert.Equal(options1.Diagnostics, rr1.RightRuleOptions.Diagnostics);
        Assert.Equal(options1.LoadCustomOperators, rr1.RightRuleOptions.LoadCustomOperators);
        Assert.Equal(options1.LoadDefaultOperators, rr1.RightRuleOptions.LoadDefaultOperators);

        var dd1 = rr1.DataService.RegisterData("a", 100);
        var dd2 = rr2.DataService.RegisterData("x", 100);
    }
}
