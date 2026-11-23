# RightRule language and rule definitions

## Statement syntax

A statement compares data references using a registered operator. References use a dataset name, optionally followed by a data item name. For example, `Customer.age` refers to item `age` in dataset `Customer`.

```text
Customer.age >= 18
Customer.status equals "active"
Customer.age >= 18 and Customer.country = "GB"
Customer.isBlocked = true or Customer.riskScore > 80
Customer.age >= 18 and (Customer.country = "GB" or Customer.country = "IE")
```

Use parentheses to make grouping explicit. `and`, `or`, and `not` combine or negate comparisons. Operators can be written using any registered alias; aliases are case-insensitive. The standard operators include `Contains`, `DoesNotContain`, `Equals`, `GreaterThanOrEquals`, `In`, `LessThan`, and `NotEquals`. Equality is also commonly written `=`. Check the operators registered in the session when using custom aliases.

RightRule also supports intrinsic checks for primitive values and values of types such as `string`, `decimal`, `DateTime`, `DateOnly`, and `TimeOnly`. The value names and types available to a statement come from registered datasets and their adaptors. Use `StatementSyntaxCheck`/`StatementSyntaxCheckAsync` to diagnose statement syntax before registration where appropriate.

## Rule structure

A `DefinedRule` contains `Number`, `Name`, `Context`, `Statement`, `Decision`, `DecisionData`, and `OptionalData`.

| Field | Meaning |
|---|---|
| `Number` | Evaluation order within its context; lower numbers are evaluated first. |
| `Name` | Human-readable identifier for lookup and returned results. |
| `Context` | Event or decision point that groups rules. Applying a context evaluates only its rules. Use a stable, non-empty context name in both registration and application. |
| `Statement` | Boolean expression evaluated against registered data. |
| `Decision` | Decision word returned to the application for the rule's evaluated outcome. Use it to route application behavior. |
| `DecisionData` | Optional payload returned with the decision, useful for downstream details such as a route, limit, or reason. |
| `OptionalData` | Optional configuration passed to operators during evaluation; custom operators define how to interpret it. |

Decision words and data are application contracts. For each context, handle every possible decision in application code, including the default/no-decision path. Do not assume a decision payload has one shape unless the rule configuration and calling code agree on that shape.

## Registering rules

Register definitions with `session.RuleService.RegisterRule` or `RegisterRules`. Inspect each `RuleRegistrationResult` for status and message; invalid definitions are reported rather than silently becoming usable rules. Rule registration and data registration belong to the current `IRightRuleSession`.

```csharp
var definition = new DefinedRule
{
    Number = 10,
    Name = "Adult customer",
    Context = "CustomerEligibility",
    Statement = "Customer.age >= 18",
    Decision = "ELIGIBLE",
    DecisionData = new { Queue = "standard" },
    OptionalData = null
};

var registered = session.RuleService.RegisterRule(definition);
```

The fluent `BuildRule()` API enforces the required fields at compile time and makes their order clear. After `Number`, `Name`, `Context`, `Statement`, and `Decision`, optional payloads can be supplied in either order, then call `Build()` and register the resulting definition:

```csharp
var definition = session.RuleService.BuildRule()
    .Number(10)
    .Name("Adult customer")
    .Context("CustomerEligibility")
    .Statement("Customer.age >= 18")
    .Decision("ELIGIBLE")
    .DecisionData(new { Queue = "standard" })
    .OptionalData(null)
    .Build();

var registered = session.RuleService.RegisterRule(definition);
```

Rules can also be loaded from JSON with `LoadRules`. Use serialization and the context/name/number lookup methods to inspect registered definitions. Context is the key boundary for applying a coherent set of rules; avoid using a different spelling or context for application.

## Default rules

A default rule provides a defined outcome when no condition succeeds in first-match evaluation, or when an all-rules evaluation cannot satisfy its success condition. Register one per context as a fallback decision that the caller knows how to handle. It has `Name`, `Context`, `Decision`, and optional `DecisionData` fields; it has no statement.

```csharp
var fallback = session.RuleService.BuildDefaultRule()
    .Name("No eligibility match")
    .Context("CustomerEligibility")
    .Decision("NO_DECISION")
    .DecisionData(new { Reason = "No rule matched" })
    .Build();

var registeredDefault = session.RuleService.RegisterDefaultRule(fallback);
```

Without a meaningful default, callers may lack a context-specific fallback when none of the rules matches. Treat default decision words and payloads as part of the same application contract as ordinary decisions.
