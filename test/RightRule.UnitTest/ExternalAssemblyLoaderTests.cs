using RightRule.OperatorServices.OperatorLoader.CustomOperators;
using RightRule.System.Common.Operator.Models;

namespace RightRuleTests;

public class ExternalAssemblyLoaderTests
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("  ")]
    public void Load_WhenPathIsNullOrWhitespace_ThrowsFileNotFoundException(string? path)
    {
        var exception = Assert.Throws<FileNotFoundException>(
            () => ExternalAssemblyLoader.Load(path!));

        Assert.Equal("Operator assembly not found", exception.Message);
    }

    [Fact]
    public void Load_WhenAssemblyDoesNotExist_ThrowsFileNotFoundException()
    {
        var path = Path.Combine(
            Path.GetTempPath(),
            $"{Guid.NewGuid():N}.dll");

        var exception = Assert.Throws<FileNotFoundException>(
            () => ExternalAssemblyLoader.Load(path));

        Assert.Equal("Operator assembly not found", exception.Message);
        Assert.Equal(path, exception.FileName);
    }

    [Fact]
    public void Load_WhenAssemblyContainsOperators_ReturnsOperator()
    {
        var assemblyPath = typeof(ExternalAssemblyLoader).Assembly.Location;

        var result = ExternalAssemblyLoader.Load(assemblyPath);

        Assert.NotNull(result);
        Assert.IsAssignableFrom<Operator>(result);
        result.Dispose();
    }
}