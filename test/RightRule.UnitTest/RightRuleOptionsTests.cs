using RightRule.Models;
using System.Runtime.CompilerServices;

namespace RightRuleTests;

public class RightRuleOptionsTests
{
    // Default location on test PC
    private static readonly string OriginalPath = "C:\\Development\\BSoft\\RightRule\\src\\Operators\\CustomOperators\\customoperatorconfig.json";

    private static readonly string assemblyDirectory = Path.GetDirectoryName(typeof(RightRuleLibraryDefaults).Assembly.Location);
    private static readonly string configuredPath = "Operators\\CustomOperators\\customoperatorconfig.json";
    private static readonly string path = Path.GetFullPath(configuredPath, assemblyDirectory!);

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
        Assert.Equal(10, rr.OperatorService.Operators.Count);
    }

    [Fact]
    public void Default_Options()
    {
        /*
         * DataCheck = false
         * Diagnostics = false
         * LoadCustomOperators = true
         * LoadDefaultOperators = true
         * CustomOperatorLibraryPath = 
         * Example: C:\Development\BSoft\RightRule\test\RightRule.UnitTest\bin\Debug\net10.0\Operators\CustomOperators\customoperatorconfig.json
         */

        RightRuleOptions options = new RightRuleOptions();

        Assert.NotNull(options);
        Assert.False(options.DataCheck);
        Assert.False(options.Diagnostics);
        Assert.True(options.LoadCustomOperators);
        Assert.True(options.LoadDefaultOperators);
        Assert.Equal(options.CustomOperatorLibraryPath, path);

        var rr = new RightRule.RightRule(options);
        Assert.NotNull(rr);

        var optionsFound = rr.GetRightRuleOptions();

        Assert.NotNull(optionsFound);
        Assert.False(optionsFound.DataCheck);
        Assert.False(optionsFound.Diagnostics);
        Assert.True(optionsFound.LoadCustomOperators);
        Assert.True(optionsFound.LoadDefaultOperators);
        Assert.Equal(optionsFound.CustomOperatorLibraryPath, path);

        // This confirms the Operators were loaded
        // Assumes the path to the Operator Library was valid
        Assert.Equal(10, rr.OperatorService.Operators.Count);
    }

    [Fact]
    public void Default_ModifiedBooleanOptions()
    {
        /*
         * DataCheck = true
         * Diagnostics = true
         * LoadCustomOperators = true
         * LoadDefaultOperators = true
         * CustomOperatorLibraryPath = 
         * Example: C:\Development\BSoft\RightRule\test\RightRule.UnitTest\bin\Debug\net10.0\Operators\CustomOperators\customoperatorconfig.json
         */

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
        Assert.Equal(optionsFound.CustomOperatorLibraryPath, path);

        // This confirms the Operators were loaded
        // Assumes the path to the Operator Library was valid
        Assert.Equal(10, rr.OperatorService.Operators.Count);
    }

    [Fact]
    public void Default_ModifiedPathOption()
    {
        /*
         * DataCheck = false
         * Diagnostics = false
         * LoadCustomOperators = true
         * LoadDefaultOperators = true
         * CustomOperatorLibraryPath = 
         * Example: C:\Development\BSoft\RightRule\test\RightRule.UnitTest\bin\Debug\net10.0\Operators\CustomOperators\customoperatorconfig.json
         */

        var customPath = @"c:\public\customoperatorconfig.json";

        RightRuleOptions options = new RightRuleOptions()
        {
            CustomOperatorLibraryPath = customPath
        };

        var rr = new RightRule.RightRule(options);

        Assert.NotNull(options);
        Assert.False(options.DataCheck);
        Assert.False(options.Diagnostics);
        Assert.True(options.LoadCustomOperators);
        Assert.True(options.LoadDefaultOperators);
        Assert.Equal(options.CustomOperatorLibraryPath, customPath);

        var optionsFound = rr.GetRightRuleOptions();

        Assert.NotNull(optionsFound);
        Assert.False(optionsFound.DataCheck);
        Assert.False(optionsFound.Diagnostics);
        Assert.True(optionsFound.LoadCustomOperators);
        Assert.True(optionsFound.LoadDefaultOperators);
        Assert.Equal(optionsFound.CustomOperatorLibraryPath, customPath);

        // This confirms the Operators were loaded
        // Assumes the path to the Operator Library was valid
        Assert.Equal(7, rr.OperatorService.Operators.Count);
    }

    [Fact]
    public void Default_PathEmpty()
    {
        /*
         * DataCheck = false
         * Diagnostics = false
         * LoadCustomOperators = true
         * LoadDefaultOperators = true
         * CustomOperatorLibraryPath = 
         * Example: C:\Development\BSoft\RightRule\test\RightRule.UnitTest\bin\Debug\net10.0\Operators\CustomOperators\customoperatorconfig.json
         */

        var customPath = @"";

        RightRuleOptions options = new RightRuleOptions()
        {
            CustomOperatorLibraryPath = customPath
        };

        var rr = new RightRule.RightRule(options);

        Assert.NotNull(options);
        Assert.False(options.DataCheck);
        Assert.False(options.Diagnostics);
        Assert.True(options.LoadCustomOperators);
        Assert.True(options.LoadDefaultOperators);
        Assert.Equal(options.CustomOperatorLibraryPath, customPath);

        var optionsFound = rr.GetRightRuleOptions();

        Assert.NotNull(optionsFound);
        Assert.False(optionsFound.DataCheck);
        Assert.False(optionsFound.Diagnostics);
        Assert.True(optionsFound.LoadCustomOperators);
        Assert.True(optionsFound.LoadDefaultOperators);
        Assert.Equal(optionsFound.CustomOperatorLibraryPath, customPath);

        // This confirms the Operators were not loaded
        // Empty Path to the Operator Library
        Assert.Equal(7, rr?.OperatorService.Operators.Count ?? 0);
    }

    [Fact]
    public void Default_AllOperators()
    {
        /*
         * DataCheck = false
         * Diagnostics = false
         * LoadCustomOperators = true
         * LoadDefaultOperators = true
         * CustomOperatorLibraryPath = 
         * Example: C:\Development\BSoft\RightRule\test\RightRule.UnitTest\bin\Debug\net10.0\Operators\CustomOperators\customoperatorconfig.json
         */

        // Path to Custom Operators left as default
        RightRuleOptions options = new RightRuleOptions()
        {
            LoadCustomOperators = true,
            LoadDefaultOperators = true
        };

        var rr = new RightRule.RightRule(options);

        Assert.NotNull(options);
        Assert.False(options.DataCheck);
        Assert.False(options.Diagnostics);
        Assert.True(options.LoadCustomOperators);
        Assert.True(options.LoadDefaultOperators);
        Assert.Equal(options.CustomOperatorLibraryPath, path);

        var optionsFound = rr.GetRightRuleOptions();

        Assert.NotNull(optionsFound);
        Assert.False(optionsFound.DataCheck);
        Assert.False(optionsFound.Diagnostics);
        Assert.True(optionsFound.LoadCustomOperators);
        Assert.True(optionsFound.LoadDefaultOperators);
        Assert.Equal(optionsFound.CustomOperatorLibraryPath, path);

        // This confirms the Operators were loaded
        // Assumes the path to the Operator Library was valid
        Assert.Equal(10, rr.OperatorService.Operators.Count);
    }

    [Fact]
    public void Default_DefaultOnlyOperators()
    {
        /*
         * DataCheck = false
         * Diagnostics = false
         * LoadCustomOperators = false
         * LoadDefaultOperators = true
         * CustomOperatorLibraryPath = 
         * Example: C:\Development\BSoft\RightRule\test\RightRule.UnitTest\bin\Debug\net10.0\Operators\CustomOperators\customoperatorconfig.json
         */

        // Path to Custom Operators left as default
        RightRuleOptions options = new RightRuleOptions()
        {
            LoadCustomOperators = false,
            LoadDefaultOperators = true
        };

        var rr = new RightRule.RightRule(options);

        Assert.NotNull(options);
        Assert.False(options.DataCheck);
        Assert.False(options.Diagnostics);
        Assert.False(options.LoadCustomOperators);
        Assert.True(options.LoadDefaultOperators);
        Assert.Equal(options.CustomOperatorLibraryPath, path);

        var optionsFound = rr.GetRightRuleOptions();

        Assert.NotNull(optionsFound);
        Assert.False(optionsFound.DataCheck);
        Assert.False(optionsFound.Diagnostics);
        Assert.False(optionsFound.LoadCustomOperators);
        Assert.True(optionsFound.LoadDefaultOperators);
        Assert.Equal(optionsFound.CustomOperatorLibraryPath, path);

        // This confirms the Operators were loaded
        // Assumes the path to the Operator Library was valid
        Assert.Equal(7, rr.OperatorService.Operators.Count);
    }

    [Fact]
    public void Default_CustomOnlyOperators()
    {
        /*
         * DataCheck = false
         * Diagnostics = false
         * LoadCustomOperators = true
         * LoadDefaultOperators = false
         * CustomOperatorLibraryPath = 
         * Example: C:\Development\BSoft\RightRule\test\RightRule.UnitTest\bin\Debug\net10.0\Operators\CustomOperators\customoperatorconfig.json
         */

        // Path to Custom Operators left as default
        RightRuleOptions options = new RightRuleOptions()
        {
            LoadCustomOperators = true,
            LoadDefaultOperators = false
        };

        var rr = new RightRule.RightRule(options);

        Assert.NotNull(options);
        Assert.False(options.DataCheck);
        Assert.False(options.Diagnostics);
        Assert.True(options.LoadCustomOperators);
        Assert.False(options.LoadDefaultOperators);
        Assert.Equal(options.CustomOperatorLibraryPath, path);

        var optionsFound = rr.GetRightRuleOptions();

        Assert.NotNull(optionsFound);
        Assert.False(optionsFound.DataCheck);
        Assert.False(optionsFound.Diagnostics);
        Assert.True(optionsFound.LoadCustomOperators);
        Assert.False(optionsFound.LoadDefaultOperators);
        Assert.Equal(optionsFound.CustomOperatorLibraryPath, path);

        // This confirms the Operators were loaded
        // Assumes the path to the Operator Library was valid
        Assert.Equal(3, rr.OperatorService.Operators.Count);
    }

    [Fact]
    public void Default_NoOperators()
    {
        /*
         * DataCheck = false
         * Diagnostics = false
         * LoadCustomOperators = false
         * LoadDefaultOperators = false
         * CustomOperatorLibraryPath = 
         * Example: C:\Development\BSoft\RightRule\test\RightRule.UnitTest\bin\Debug\net10.0\Operators\CustomOperators\customoperatorconfig.json
         */

        // Path to Custom Operators left as default
        RightRuleOptions options = new RightRuleOptions()
        {
            LoadCustomOperators = false,
            LoadDefaultOperators = false
        };

        var rr = new RightRule.RightRule(options);

        Assert.NotNull(options);
        Assert.False(options.DataCheck);
        Assert.False(options.Diagnostics);
        Assert.False(options.LoadCustomOperators);
        Assert.False(options.LoadDefaultOperators);
        Assert.Equal(options.CustomOperatorLibraryPath, path);

        var optionsFound = rr.GetRightRuleOptions();

        Assert.NotNull(optionsFound);
        Assert.False(optionsFound.DataCheck);
        Assert.False(optionsFound.Diagnostics);
        Assert.False(optionsFound.LoadCustomOperators);
        Assert.False(optionsFound.LoadDefaultOperators);
        Assert.Equal(optionsFound.CustomOperatorLibraryPath, path);

        // This confirms the Operators were loaded
        // Assumes the path to the Operator Library was valid
        Assert.Empty(rr.OperatorService.Operators);
    }
}
