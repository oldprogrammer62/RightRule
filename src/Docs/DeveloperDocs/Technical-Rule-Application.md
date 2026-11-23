# Applying rules and reading results

Before applying rules, register the input data referenced by their statements in the same RightRule instance/session. The context selects the rules to evaluate. Rule numbers determine ascending order for `FIRST` evaluation.

## Evaluation methods

| API | Return type | Behavior |
|---|---|---|
| `ApplyRule(context)` / `ApplyRuleAsync(context)` | One `RightRuleResult<IProcessedRule>` | First-match (`FIRST`): returns the first true rule, or the context default when none matches. |
| `ApplyRules(context)` / `ApplyRulesAsync(context)` | List of results | Defaults to `FIRST`; the list contains the single selected outcome. |
| `ApplyRules(context, EvaluationTypeEnum)` / async equivalent | List of results | Selects `FIRST`, `ANY`, or `ALL` behavior. |

`FIRST` evaluates in number order and returns one applicable rule, or the default. `ANY` evaluates every rule and returns each rule's outcome, including false outcomes. `ALL` evaluates the group and returns the rule outcomes only when all are true; if any is false, the context default is returned. When there are no applicable rules or evaluation fails, the public methods return an error result; configure the required default for fallback paths.

Use the async methods from asynchronous code. The synchronous methods block while invoking the async implementation.

```csharp
RightRuleResult<IProcessedRule> first =
    await session.RuleService.ApplyRuleAsync("CustomerEligibility");

List<RightRuleResult<IProcessedRule>> perRule =
    await session.RuleService.ApplyRulesAsync(
        "CustomerEligibility", EvaluationTypeEnum.ANY);
```

## Reading results

`RightRuleResult<IProcessedRule>` exposes `Status`, `Result`, `RuleNumber`, `RuleName`, `Decision`, `DecisionData`, `StatementApplied`, and `Message`.

| Status | Meaning |
|---|---|
| `APPLICABLE` | The rule evaluated true. Decision data corresponds to the true outcome. |
| `NOTAPPLICABLE` | A rule evaluated false, primarily visible in `ANY` results. |
| `DEFAULT` | The context default was selected. |
| `ERROR` | Evaluation could not produce a normal outcome; inspect `Message`. |

`Result` is the Boolean rule outcome. `Decision` is the application routing word, while `DecisionData` is its optional payload. `StatementApplied` helps identify the evaluated statement. Error results should be handled using `Status`; do not interpret their decision fields as a successful application decision.

```csharp
switch (first.Status)
{
    case RightRuleResultEnum.APPLICABLE:
    case RightRuleResultEnum.DEFAULT:
        return HandleDecision(first.Decision, first.DecisionData);
    case RightRuleResultEnum.ERROR:
        throw new InvalidOperationException(first.Message);
    default:
        return HandleNonApplicable(first);
}
```

For `ANY`, inspect every result and branch on each result's status and boolean value. For `ALL`, account for the returned default when the combined condition fails. A default decision and payload should be handled just like a deliberate application outcome.

## Decision data interpolation

String `DecisionData` values may contain `$` references resolved against registered data at evaluation time. A dotted reference uses `$Dataset.Item`; a bare `$Name` looks up the item whose dataset and item names are both `Name`. For example, register `order.discount` with value `15`, then configure a decision payload string such as:

```json
{"discount": $order.discount}
```

The returned payload string contains the registered value in place of `$order.discount`. This lets a rule select a result-specific message or payload while the caller supplies run-time details. Non-string decision data is passed through without string interpolation. Unresolved references are left in the string, so ensure the required data is registered and inspect the returned payload shape.
