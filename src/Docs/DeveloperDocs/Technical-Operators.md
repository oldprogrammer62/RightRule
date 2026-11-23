# Operators

Operators compare the left and right operands of a rule condition. A statement selects an operator by an alias, such as `equals`, `=`, or a custom word alias. The active aliases are available from `session.OperatorService.GetRegisteredAliases()`; operator names and aliases are resolved case-insensitively. Operators may also receive the rule's `OptionalData` as operator-specific configuration.

## Built-in operators

RightRule ships with `Contains`, `DoesNotContain`, `Equals`, `GreaterThanOrEquals`, `In`, `LessThan`, and `NotEquals`. Use aliases reported by the current operator service as the supported statement spellings. Input types and comparison semantics depend on each operator; make sure registered values and literal values are compatible.

## Implement a custom operator

Implement the operator contract (commonly by deriving from `Operator`), provide one or more aliases, and implement its asynchronous execution method. Validate operand types and optional configuration, and return an `OperatorResult` that identifies the operator and whether evaluation succeeded.

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
            : OperatorResult.FailureResult(
                "IsSmallerThan", "Left is not smaller than Right"));
    }
}
```

Register an instance with `session.OperatorService.RegisterOperator` and check the returned `OperatorRegistrationResult`. Operator names and aliases must not conflict with already registered operators. A runtime registration belongs to that session.

## Load custom operators from an assembly

Startup loading is controlled by `LoadDefaultOperators`, `LoadCustomOperators`, and `CustomOperatorLibraryPath` in `RightRuleOptions`. Custom operator configuration is stored in `customoperatorconfig.json`, which identifies assemblies to load. Set the options before creating the RightRule instance/session so its startup loader can read them. `RegisterAllOperators(options)` can replace the active operator set; inspect the result. Operator instances registered with a session are disposed when that session is disposed, so avoid sharing disposable instances across sessions.
