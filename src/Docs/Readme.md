## RightRule overview

RightRule is a .NET rules engine for externalizing business decisions. 
The basic steps for using RightRule are:  
- Register Data(s)
- Register Rules
- Apply Rules
- Obtain result
- Act on the result

## Creating and disposing a RightRule session
RightRule is a factory configured with RightRuleOptions and an ILoggerFactory. Call CreateSession() to create an IRightRuleSession.
Each session has its own registered data and rules.
For manual construction, dispose the session—not the factory. The session owns its service provider and disposes it when the session is disposed.

```csharp
var factory = new RightRule(options, loggerFactory);
await using var session = factory.CreateSession();
session.DataService.RegisterData("Demo", data);
session.RuleService.RegisterRules(rules);
var result = await session.RuleService.ApplyRuleAsync("Demo");
```

When using host dependency injection, call AddRightRule() and resolve IRightRuleSession from a DI scope.
The session uses the host’s scoped services and does not own or dispose the host’s service provider.
Dispose the scope to end the session and release its services.

```csharp
services.AddRightRule();
await using var scope = serviceProvider.CreateAsyncScope();
var session = scope.ServiceProvider.GetRequiredService<IRightRuleSession>();
session.DataService.RegisterData("Demo", data);
session.RuleService.RegisterRules(rules);
var result = await session.RuleService.ApplyRuleAsync("Demo");
```

AddRightRule() registers the session and its services as scoped. 
Use one scope per independent session; 
If you need multiple independent sessions at once, create a separate scope for each. 
ServiceRegister is an internal implementation detail, not the registration API for host applications.


### Data providers and Data registration

Rules evaluate against data registered with RightRule. A rule can refer to a value by its dataset and item name, such as `Customer.Name`, or use a standalone registered value such as `name`.

`RegisterData<T>(datasetName, data)` accepts input through a Data Adaptor. Built-in adaptors handle dictionaries, strings, tuples, lists of tuples, and objects.
  
For other application-specific data types, the library user can implement and register an `IDataAdaptor<TInput>` that converts the input into RightRule’s dataset-and-items structure. This lets the application decide how its data is shaped for rules.  
`DataAdaptorRegistrationResult? dataAdaptorResult = rr.DataAdaptorService.RegisterDataAdaptor(dataAdaptor);`


Data can be registered as follows:

- Through `RegisterData<T>`, passing a dataset name and a supported or custom-adaptor input.

Datasets can also be inspected and deregistered.

### Rule statements and grammar

A RightRule statement is made up of conditions combined with logical keywords and, optionally, parentheses. Its core principle is that an **operator acts on its left and right operands**:
```plaintext
Customer.OrderValue >= 50
```

Here, `Customer.OrderValue` is the left operand, `>=` is the operator, and `50` is the right operand. Operands may be registered data references, literals, booleans, or arrays.

The logical keywords are **AND**, **OR**, and **NOT**. The parser supports grouping with parentheses; its precedence is `NOT`, then `AND`, then `OR`. The current tokenizer recognizes the keywords in lowercase (`and`, `or`, `not`).

RightRule’s default operators include equality (`equals`, `eq`, `=`, `==`), inequality (`notequals`, `neq`, `!=`, `<>`), `contains`, `doesnotcontain`, less-than (`<`), greater-than-or-equal (`>=`), and `in`.  
Each Operator can register one or more aliases that can be used in a statement. This improves the readability of a rule statement by all stakeholders.  

The language includes an array type for membership checks with `IN`. For example, a rule can compare a registered value to an array literal (`number in [1,2,3]`) or to a registered array (`value in values`).

### Custom operators

Custom operators extend the operators available in rule statements. An operator developer derives from RightRule’s `Operator` type, supplies its aliases, and implements the operation on the **left** and **right** operands; optional rule data can also be supplied to the operator.

Custom operators can be loaded automatically from a JSON configuration that lists operator assembly paths. RightRule’s library configuration provides the default configuration path, and `RightRuleOptions` can control whether default or custom operators are loaded and can override the custom-operator path. Alternatively, an application can load or construct an operator itself and register it through the RightRule API. The operator’s aliases then become available for use in rule statements.

`OperatorRegistrationResult? registered = rr.OperatorService.RegisterOperator(op);`

### Applying rules and decisions

Rules are registered for a context and evaluated against the data available to that RightRule instance.  
The context will be specified from your code base to indicate which set of rules to apply.  
Rules are registered as follows:
  
Define a rule object:  
`DefinedRule rule = new DefinedRule() {...}`

Register the rule:  
`RuleRegistrationResult? ruleRegistered = rr.RuleService.RegisterRule(rule);`

Or a fluent builder can be used to build and register the rule.  
```plaintext
RuleRegistrationResult? rule = rr.RuleService.BuildRule()
     .Number(1)
     .Name("Rule1")
     .Context("ContextWord")
     .Statement("a = b")
     .TrueDecision("GOOD")
     .TrueDecisionData("{}")
     .OptionalData("{}")
     .Build() 
     .Register(rr.RuleService);
```

The caller can also choose how rule results are returned:

- **FIRST** returns the first successful rule, or the context’s default rule if none succeeds.
- **ANY** evaluates and returns the applied rules, including each rule’s result and corresponding true or false decision.
- **ALL** returns all applied rules only when every rule succeeds; otherwise it returns the default rule.

Examples are:  
`RightRuleResult<IProcessedRule>? result = rr.RuleService.ApplyRule("ContextWord");`  
  
`List<RightRuleResult<IProcessedRule>>? result = rr.RuleService.ApplyRules("ContextWord", EvaluationTypeEnum.FIRST);`  
`List<RightRuleResult<IProcessedRule>>? result = rr.RuleService.ApplyRules("ContextWord", EvaluationTypeEnum.ANY);`  
`List<RightRuleResult<IProcessedRule>>? result = rr.RuleService.ApplyRules("ContextWord", EvaluationTypeEnum.ALL);`

Rules are ordered by rule number, from low to high. A result includes the decision and may include **decision data**.
  
Decision data can contain replaceable elements identified with a dollar sign prefix, such as `$datasetname.itemname`; RightRule substitutes the referenced value from registered data when it builds the result.

### Rule application results
Applying rules returns a RightRuleResult<IProcessedRule> for a single result or a list of results, depending on the selected evaluation type. Each result describes the evaluated rule and its outcome:
- **RuleNumber** and **RuleName** identify the rule.
- **Result** indicates whether the rule’s statement evaluated to true.
- **Decision** contains the rule’s true decision when it matches, or its false decision when it does not.
- **DecisionData** contains the corresponding true or false decision data. Replaceable values in that data are interpolated from registered data.
- **StatementApplied** contains the evaluated statement when diagnostics are enabled; otherwise, it is empty.
- **Message** provides additional information, such as a failure or error message.

When no rule matches in FIRST mode, RightRule returns the context’s default rule result. ANY returns results for the evaluated rules, while ALL returns results only if every rule succeeds; otherwise, it returns the default rule result.

