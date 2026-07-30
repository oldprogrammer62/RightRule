using System.Diagnostics.CodeAnalysis;

namespace RightRule.Models;

public class OperatorAliases
{
    public List<OpAlias> Aliases {  get; init; }

    [SetsRequiredMembers]
    public OperatorAliases()
    {
        Aliases = [];
    }

    public void Add(string opName, bool opCustom, string alias) => Aliases.Add(new OpAlias(opName, opCustom, alias));

    public override string? ToString()
    {
        return base.ToString();
    }
}

public class OpAlias
{
    public required string Alias { get; init; }
    public required string OpName { get; init; }
    public required bool OpCustom { get; init; }

    [SetsRequiredMembers]
    public OpAlias(string opName, bool opCustom, string alias) 
    {
        OpName = opName;
        OpCustom = opCustom;
        Alias = alias;
    }
}