using RightRule.Models;

namespace RightRuleTests;

public class RightRuleOptionsTests
{
    // Default location on test PC
    private string OriginalPath = "C:\\Development\\BSoft\\RightRule\\src\\Operators\\CustomOperators\\customoperatorconfig.json";

    [Fact]
    public void No_Options()
    {
        var rr = new RightRule.RightRule();
        Assert.NotNull(rr);

        var optionsFound = rr.GetRightRuleOptions();
        Assert.NotNull(optionsFound);

        Assert.False(optionsFound.DataCheck);
        Assert.False(optionsFound.Diagnostics);
        Assert.True(optionsFound.LoadCustomOperators);
        Assert.True(optionsFound.LoadDefaultOperators);
        Assert.NotNull(optionsFound.CustomOperatorLibraryPath);

        // This confirms the Operators were loaded
        // Assumes the path to the Operator Library was valid
        Assert.Equal(10, rr.GetOperators().Count);
    }

    [Fact]
    public void Default_Options()
    {
        RightRuleOptions options = new RightRuleOptions();

        var rr = new RightRule.RightRule(options);
        Assert.NotNull(rr);

        var optionsFound = rr.GetRightRuleOptions();
        Assert.NotNull(optionsFound);

        Assert.False(optionsFound.DataCheck);
        Assert.False(optionsFound.Diagnostics);
        Assert.True(optionsFound.LoadCustomOperators);
        Assert.True(optionsFound.LoadDefaultOperators);
        Assert.NotNull(optionsFound.CustomOperatorLibraryPath);

        // This confirms the Operators were loaded
        // Assumes the path to the Operator Library was valid
        Assert.Equal(10, rr.GetOperators().Count);
    }

    [Fact]
    public void Default_ModifiedBooleanOptions()
    {
        // Path to Custom Operators left as default
        RightRuleOptions options = new RightRuleOptions()
        {
            DataCheck = true,
            Diagnostics = true,
            LoadDefaultOperators = true,
            LoadCustomOperators = true
        };

        var rr = new RightRule.RightRule(options);
        Assert.NotNull(rr);

        var optionsFound = rr.GetRightRuleOptions();
        Assert.NotNull(optionsFound);

        Assert.True(optionsFound.DataCheck);
        Assert.True(optionsFound.Diagnostics);
        Assert.True(optionsFound.LoadCustomOperators);
        Assert.True(optionsFound.LoadDefaultOperators);
        Assert.NotNull(optionsFound.CustomOperatorLibraryPath);

        // This confirms the Operators were loaded
        // Assumes the path to the Operator Library was valid
        Assert.Equal(10, rr.GetOperators().Count);
    }

    [Fact]
    public void Default_ModifiedPathOption()
    {
        RightRuleOptions options = new RightRuleOptions()
        {
            CustomOperatorLibraryPath = @"c:\public"
        };

        var rr = new RightRule.RightRule(options);
        Assert.NotNull(rr);

        var optionsFound = rr.GetRightRuleOptions();
        Assert.NotNull(optionsFound);

        Assert.False(optionsFound.DataCheck);
        Assert.False(optionsFound.Diagnostics);
        Assert.True(optionsFound.LoadCustomOperators);
        Assert.True(optionsFound.LoadDefaultOperators);
        Assert.NotNull(optionsFound.CustomOperatorLibraryPath);

        // This confirms the Operators were loaded
        // Assumes the path to the Operator Library was valid
        Assert.Equal(7, rr.GetOperators().Count);
    }

    [Fact]
    public void Default_PathEmpty()
    {
        RightRuleOptions options = new RightRuleOptions()
        {
            CustomOperatorLibraryPath = string.Empty
        };

        var rr = new RightRule.RightRule(options);
        Assert.NotNull(rr);

        var optionsFound = rr.GetRightRuleOptions();
        Assert.NotNull(optionsFound);

        Assert.False(optionsFound.DataCheck);
        Assert.False(optionsFound.Diagnostics);
        Assert.True(optionsFound.LoadCustomOperators);
        Assert.True(optionsFound.LoadDefaultOperators);
        Assert.NotNull(optionsFound.CustomOperatorLibraryPath);

        // This confirms the Operators were loaded
        // Assumes the path to the Operator Library was valid
        Assert.Equal(10, rr.GetOperators().Count);
    }

    [Fact]
    public void Default_AllOperators()
    {
        // Path to Custom Operators left as default
        RightRuleOptions options = new RightRuleOptions()
        {
            LoadCustomOperators = true,
            LoadDefaultOperators = true
        };

        var rr = new RightRule.RightRule(options);
        Assert.NotNull(rr);

        var optionsFound = rr.GetRightRuleOptions();
        Assert.NotNull(optionsFound);

        Assert.False(optionsFound.DataCheck);
        Assert.False(optionsFound.Diagnostics);
        Assert.True(optionsFound.LoadCustomOperators);
        Assert.True(optionsFound.LoadDefaultOperators);

        // This confirms the Operators were loaded
        // Assumes the path to the Operator Library was valid
        Assert.Equal(10, rr.GetOperators().Count);
    }

    [Fact]
    public void Default_DefaultOnlyOperators()
    {
        // Path to Custom Operators left as default
        RightRuleOptions options = new RightRuleOptions()
        {
            LoadCustomOperators = false,
            LoadDefaultOperators = true
        };

        var rr = new RightRule.RightRule(options);
        Assert.NotNull(rr);

        var optionsFound = rr.GetRightRuleOptions();
        Assert.NotNull(optionsFound);

        Assert.False(optionsFound.DataCheck);
        Assert.False(optionsFound.Diagnostics);
        Assert.False(optionsFound.LoadCustomOperators);
        Assert.True(optionsFound.LoadDefaultOperators);

        // This confirms the Operators were loaded
        // Assumes the path to the Operator Library was valid
        Assert.Equal(7, rr.GetOperators().Count);
    }

    [Fact]
    public void Default_CustomOnlyOperators()
    {
        // Path to Custom Operators left as default
        RightRuleOptions options = new RightRuleOptions()
        {
            LoadCustomOperators = true,
            LoadDefaultOperators = false
        };

        var rr = new RightRule.RightRule(options);
        Assert.NotNull(rr);

        var optionsFound = rr.GetRightRuleOptions();
        Assert.NotNull(optionsFound);

        Assert.False(optionsFound.DataCheck);
        Assert.False(optionsFound.Diagnostics);
        Assert.False(optionsFound.LoadDefaultOperators);
        Assert.True(optionsFound.LoadCustomOperators);

        // This confirms the Operators were loaded
        // Assumes the path to the Operator Library was valid
        Assert.Equal(3, rr.GetOperators().Count);
    }

    [Fact]
    public void Default_NoOperators()
    {
        // Path to Custom Operators left as default
        RightRuleOptions options = new RightRuleOptions()
        {
            LoadCustomOperators = false,
            LoadDefaultOperators = false
        };

        var rr = new RightRule.RightRule(options);
        Assert.NotNull(rr);

        var optionsFound = rr.GetRightRuleOptions();
        Assert.NotNull(optionsFound);

        Assert.False(optionsFound.DataCheck);
        Assert.False(optionsFound.Diagnostics);
        Assert.False(optionsFound.LoadCustomOperators);
        Assert.False(optionsFound.LoadDefaultOperators);

        // This confirms the Operators were loaded
        // Assumes the path to the Operator Library was valid
        Assert.Empty(rr.GetOperators());
    }
}
