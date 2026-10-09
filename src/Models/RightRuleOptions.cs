namespace RightRule.Models;

public sealed class RightRuleOptions
{
    public bool LoadDefaultOperators { get; set; } = true;
    public bool DataCheck { get; set; } = false;
    public bool Diagnostics { get; set; } = false;

    public string CustomOperatorLibraryPath { get; set; } = "C:\\Development\\BSoft\\RightRule\\src\\Operators\\CustomOperators\\customoperatorconfig.json";
}
