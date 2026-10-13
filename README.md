## RightRule overview

RightRule is a .NET rules engine for externalizing business decisions. A typical session is to create a `RightRule` instance, register data and rules, apply the rules for a context, and use the returned decision to control your application flow.  
RightRule can be instantiated directly with new RightRule() (which creates and owns its service provider), or registered as a scoped service in an application’s IServiceCollection using AddRightRule() and resolved from the application’s service provider.

The basic steps for using RightRule are:  
- Register Data(s)
- Register Rules
- Apply Rules
- Obtain result
- Act on the result

### Data providers and Data registration

Rules evaluate against data registered with RightRule. A rule can refer to a value by its dataset and item name, such as `Customer.Name`, or use a standalone registered value such as `name`.

`RegisterData<T>(datasetName, data)` accepts input through a data provider. Built-in providers handle dictionaries, strings, tuples, lists of tuples, and objects.
  
For other application-specific data types, the library user can implement and register an `IDataProvider<TInput>` that converts the input into RightRule’s dataset-and-items structure. This lets the application decide how its data is shaped for rules.  
`DataProviderRegistrationResult? dataProviderResult = rr.RegisterDataProvider(dataProvider);`


Data can be registered as follows:

- Through `RegisterData<T>`, passing a dataset name and a supported or custom-provider input.

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

`OperatorRegistrationResult? registered = rr.RegisterOperator(op);`

### Applying rules and decisions

Rules are registered for a context and evaluated against the data available to that RightRule instance.  
The context will be specified from your code base to indicate which set of rules to apply.  
Rules are registered as follows:
  
Define a rule object:
DefinedRule rule = new DefinedRule()
{...}

Register the rule:
RuleRegistrationResult? ruleRegistered = rr.RegisterRule(rule);

Or a fluent builder can be used to build and register the rule.  
```plaintext
RuleRegistrationResult? rule = rr.BuildRule()
     .Number(1)
     .Name("Rule1")
     .Context("ContextWord")
     .Statement("a = b")
     .Decision("GOOD")
     .FalseDecision("BAD")
     .DecisionData("{}")
     .OptionalData("{}")
     .Build() 
     .Register(rr.RuleService);
```

The caller can also choose how rule results are returned:

- **FIRST** returns the first successful rule, or the context’s default rule if none succeeds.
- **ANY** evaluates and returns the applied rules, including each rule’s result and corresponding true or false decision.
- **ALL** returns all applied rules only when every rule succeeds; otherwise it returns the default rule.

Examples are:  
`RightRuleResult<IProcessedRule>? result = rr.ApplyRule("ContextWord");`  
  
`List<RightRuleResult<IProcessedRule>>? result = rr.ApplyRules("ContextWord", EvaluationTypeEnum.FIRST);`  
`List<RightRuleResult<IProcessedRule>>? result = rr.ApplyRules("ContextWord", EvaluationTypeEnum.ANY);`  
`List<RightRuleResult<IProcessedRule>>? result = rr.ApplyRules("ContextWord", EvaluationTypeEnum.ALL);`

Rules are ordered by rule number, from low to high. A result includes the decision and may include **decision data**. Decision data can contain replaceable elements identified with a dollar sign prefix, such as `$name.name`; RightRule substitutes the referenced value from registered data when it builds the result.
