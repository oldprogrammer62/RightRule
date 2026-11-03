using System;
using System.IO;
using RightRule.OperatorServices.OperatorLoader.CustomOperators;
using Xunit;

namespace RightRuleTests;

public class CustomOperatorLoaderTests
{
    [Theory]
    [InlineData("")]
    [InlineData("  ")]
    public void LoadOperators_WithEmptyOrWhitespacePath_ReturnsEmptyList(string path)
    {
        Assert.Empty(CustomOperatorLoader.LoadOperators(path));
    }

    [Fact]
    public void LoadOperators_WithNullPath_ReturnsEmptyList()
    {
        Assert.Empty(CustomOperatorLoader.LoadOperators(null!));
    }

    [Fact]
    public void LoadOperators_WithMissingFile_ReturnsEmptyList()
    {
        var path = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid():N}.json");

        Assert.Empty(CustomOperatorLoader.LoadOperators(path));
    }

    [Theory]
    [InlineData("{")]
    [InlineData("null")]
    public void LoadOperators_WithInvalidConfiguration_ReturnsEmptyList(string configuration)
    {
        var path = CreateConfigFile(configuration);

        try
        {
            Assert.Empty(CustomOperatorLoader.LoadOperators(path));
        }
        finally
        {
            File.Delete(path);
        }
    }

    [Theory]
    [InlineData("{\"Operators\":[]}")]
    [InlineData("{\"Operators\":null}")]
    public void LoadOperators_WithNoConfiguredOperators_ReturnsEmptyList(string configuration)
    {
        var path = CreateConfigFile(configuration);

        try
        {
            Assert.Empty(CustomOperatorLoader.LoadOperators(path));
        }
        finally
        {
            File.Delete(path);
        }
    }

    [Fact]
    public void LoadOperator_WithMissingAssembly_ThrowsFileNotFoundException()
    {
        var path = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid():N}.dll");

        Assert.Throws<FileNotFoundException>(() => CustomOperatorLoader.LoadOperator(path));
    }

    private static string CreateConfigFile(string configuration)
    {
        var path = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid():N}.json");
        File.WriteAllText(path, configuration);
        return path;
    }
}