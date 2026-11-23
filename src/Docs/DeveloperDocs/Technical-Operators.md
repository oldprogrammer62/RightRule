# Operators

Operators implement comparisons used in rule statements. A statement supplies left and right values and selects an operator by one of its aliases. An operator returns a success or failure evaluation; it may also receive the rule's `OptionalData` for operator-specific configuration.

## Built-in operators

The built-in set includes `Contains`, `DoesNotContain`, `Equals`, `GreaterThanOrEquals`, `In`, `LessThan`, and `NotEquals`. Operators can have aliases (for example, a symbolic comparison form); aliases are normalized without case sensitivity. Use `session.OperatorService.GetOperators()` and `GetRegisteredAliases()` to inspect the active set.

## Implement a custom operator

Derive from `Operator`, provide at least one alias, and implement `ExecuteAsync`. Validate types and optional configuration rather than assuming all input is well-formed. Return an `OperatorResult` that identifies the operator and whether the comparison succeeded.

```csharp
public sealed class IsSmallerThan : Operator
{
    public override List<string> Aliases => ["IsSmallerThan"];

    protected override Task<OperatorResult> ExecuteAsync(
        object left, object right, object? optionalData)
    {
        if (left is not int leftValue || right is not int rightValue)
            return Task.FromResult(OperatorResult.FailureResult(
                "IsSmallerThan", "Both values must be integers"));

        return Task.FromResult(leftValue < rightValue
            ? OperatorResult.SuccessResult("IsSmallerThan", true)
            : OperatorResult.FailureResult("IsSmallerThan", "Left is not smaller than Right"));
    }
}
```

Register an instance at runtime with `session.OperatorService.RegisterOperator`. Duplicate operator names or aliases prevent successful registration. A runtime-registered operator applies to that session.

## Load operators from an assembly

Startup options control built-in and custom loading: `LoadDefaultOperators`, `LoadCustomOperators`, and `CustomOperatorLibraryPath`. Custom operators can be listed in `customoperatorconfig.json` with a name and assembly path. Configure the path through `RightRuleOptions` or the library configuration, then create the session so startup loading can take place. `RegisterAllOperators(options)` can reload the active set; inspect its registration status. Disposing the session disposes registered operators, so operator instances should not be shared across sessions unless their lifecycle supports it.
