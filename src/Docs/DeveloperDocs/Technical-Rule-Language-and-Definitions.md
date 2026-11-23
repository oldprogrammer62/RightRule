# RightRule language and rule definitions

## Statement syntax

A statement is a Boolean expression made from comparisons. A data reference uses `DatasetName.ItemName`; a simple value registered as a variable is addressed by its dataset and item name (for example, `age.age`). String literals may use single or double quotes. Supported literal categories include numbers, booleans, strings, dates/date-times, and arrays, subject to the parser and operator accepting the resulting values.

```text
Customer.age >= 18
Customer.status equals "active"
Customer.age >= 18 and Customer.country = "GB"
Customer.isBlocked = true or Customer.riskScore > 80
Customer.age >= 18 and (Customer.country = "GB" or Customer.country = "IE")
not Customer.isBlocked = true
```

`and` binds more tightly than `or`; parentheses make grouping explicit. `not` negates a primary expression. Operator words and aliases are resolved case-insensitively. The built-in operator set is described in [Technical-Operators](Technical-Operators.md); the active operator registrations determine which aliases can be used. Use `StatementSyntaxCheck` or `StatementSyntaxCheckAsync` to obtain syntax diagnostics.

The grammar in `LanguageProcessor/grammar.txt` is a useful overview, but is not a complete catalogue of runtime operator aliases or all typed-value behavior. A syntactically valid statement can still fail during evaluation if referenced data is missing or values are incompatible with the operator.

## Rule definition

A `DefinedRule` contains these fields:

| Field | Meaning |
|---|---|
| `Number` | Orders rules within a context. Lower numbers are considered first in `FIRST` evaluation. |
| `Name` | Human-readable rule identifier included in results. |
| `Context` | Groups rules for an application decision point. Applying a context filters to its rules. Keep it stable between registration and application. |
| `Statement` | Boolean expression evaluated against registered data. |
| `Decision` | Decision word returned when the rule applies. Use it to route application behavior. |
| `DecisionData` | Optional outcome payload returned with the decision. It may carry route, limit, reason, or other application details. String values can be interpolated from registered data. |
| `OptionalData` | Optional operator configuration made available during statement evaluation; the operator is responsible for interpreting it. |

Treat decision words and payload shapes as an application contract. Handle all configured outcomes for a context, plus default and error statuses.

## Registering rules

Register a `DefinedRule` with `RegisterRule` or a batch with `RegisterRules`. Check each `RuleRegistrationResult` for success and its message. Rules and their input data are scoped to a RightRule instance/session.

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

var registration = session.RuleService.RegisterRule(definition);
```

The fluent builder uses staged interfaces to require `Number`, `Name`, `Context`, `Statement`, and `Decision` in that order. Once `Decision` is set, `DecisionData` and `OptionalData` are optional and can be supplied in either order before `Build()`. Definitions can also be constructed directly and loaded from JSON; validate registration results in either case.

## Default rules

A default rule provides a context-specific fallback when `FIRST` finds no true rule, and when `ALL` cannot satisfy the all-true condition. It has a `Name`, `Context`, `Decision`, and optional `DecisionData`; it has no statement. Register a meaningful fallback that the calling application handles explicitly.

```csharp
var fallback = session.RuleService.BuildDefaultRule()
    .Name("No eligibility match")
    .Context("CustomerEligibility")
    .Decision("NO_DECISION")
    .DecisionData(new { Reason = "No rule matched" })
    .Build();

var registration = session.RuleService.RegisterDefaultRule(fallback);
```

`ANY` reports outcomes per rule; it does not mean “return the first matching rule.” Default behavior is most relevant to `FIRST` and `ALL`. Decide what the caller should do when a context has no matching rule, and ensure a default is registered where the evaluation path requires one.
