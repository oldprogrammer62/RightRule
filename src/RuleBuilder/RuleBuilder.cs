using RightRule.RuleModels;

namespace RightRule.RuleBuilder;

public sealed class RuleBuilder
{
    private readonly RuleDefinition _ruleDefinition;
    private readonly bool _checkData;
    private readonly bool _diagnostics;

    private RuleBuilder(RuleDefinition ruleDefinition, bool checkData, bool diagnostics)
    {
        _ruleDefinition = ruleDefinition;
        _checkData = checkData;
        _diagnostics = diagnostics;
    }

    internal static RuleBuilder Create(RuleDefinition ruleDefinition)
    {
        return new RuleBuilder(ruleDefinition, checkData: true, diagnostics: false);
    }

    public RuleBuilder WithDataCheck(bool checkData = true)
    {
        return new RuleBuilder(_ruleDefinition, checkData, _diagnostics);
    }

    public RuleBuilder WithDataDiagnostics(bool diagnostics = true)
    {
        return new RuleBuilder(_ruleDefinition, _checkData, diagnostics);
    }

    public RuleDefinition Build()
    {
        return RuleDefinition.Build(_ruleDefinition, _checkData, _diagnostics);
    }
}
