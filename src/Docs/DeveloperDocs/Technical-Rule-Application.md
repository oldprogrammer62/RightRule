# Applying rules and reading results

Apply rules through `session.RuleService`, using the context registered on the rules. The context selects the rule set; dataset names referenced by statements must already have been registered in the same session.

## Evaluation modes

| Mode | Behavior |
|---|---|
| `FIRST` | Evaluates by ascending rule number and returns the first true rule. If none is true, the context default is returned. |
| `ANY` | Evaluates all rules and returns each result, with each rule's result indicating true or false and the corresponding decision details. |
| `ALL` | Returns the applied rules when all evaluate true; a false result leads to the context default. |

`ApplyRule` and `ApplyRuleAsync` return a single `RightRuleResult<IProcessedRule>` and use first-match behavior. `ApplyRules` and `ApplyRulesAsync` return a list; without an explicit mode they also use `FIRST`. The overloads accepting `EvaluationTypeEnum` allow `FIRST`, `ANY`, or `ALL` behavior. Prefer async methods from asynchronous application code.

```csharp
RightRuleResult<IProcessedRule> one =
    await session.RuleService.ApplyRuleAsync("CustomerEligibility");

List<RightRuleResult<IProcessedRule>> allChecks =
    await session.RuleService.ApplyRulesAsync(
        "CustomerEligibility", EvaluationTypeEnum.ANY);
```

## Result fields

Each result reports `Status`, `Result`, `RuleNumber`, `RuleName`, `Decision`, `DecisionData`, `StatementApplied`, and `Message`. `Status` identifies applicable, not applicable, default, or error outcomes. `Result` is the evaluated boolean for a rule result. `Decision` is the routing word; `DecisionData` carries its optional payload. For an `ANY` result list, inspect every result rather than assuming only true results are present. A default result carries the registered fallback decision and data. An error result has `Status` `ERROR` and a message; the application methods represent evaluation exceptions as error results.

Use a decision dispatch that covers every configured outcome for the context, as well as default and error cases:

```csharp
switch (one.Status)
{
    case RightRuleResultEnum.APPLICABLE:
    case RightRuleResultEnum.DEFAULT:
        return HandleDecision(one.Decision, one.DecisionData);
    case RightRuleResultEnum.ERROR:
        throw new InvalidOperationException(one.Message);
    default:
        return HandleNonApplicable(one);
}
```

Choose `ApplyRule` when the caller needs one first-match outcome. Choose `ApplyRules` with `ANY` when the caller needs to inspect each rule outcome, or `ALL` when the set is a combined condition. A list-returning API always returns a list, so handle default/error results by their status rather than treating every list entry as a successful decision.
