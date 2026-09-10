# RightRule
### Overview
C# Rules Engine with a simple Rule grammar and extensible operator library.

It consists of the following parts:
- Data.
- Rules.
- Internal logic Operators, like 'Equals' and 'NotEquals'.
- External logic Operators, that can be developed and integrated in to Right Rule.
- An Interpreter responsible for evaluating a Rule statement.
- An Evaluator responsible for applying the interpreted rules over the Data.

### How to use
There are 3 main steps to implement RightRule in your application:
1. Register your Data.
2. Register your Rule(s).
3. Apply the specified Rule(s) to your Data.
4. Act on the result.

### Data can be registered in different formats:
- Dictionary<string, object>
- Variable
- Tuple

### A Rule is defined as follows
Number: Identifies the Rule and is used to order the set of rules to be applied.
Name: Description of the Rule.
Context: A word that indicates when this Rule will be applied.
Statement: A syntactically correct statement that defines the Rule criteria to be evaluated.
Decision: A string that will be returned when the Rule statement evaluates to true.
DecisionData: Additional details that will be returned when the Rule statement evaluates to true.
FalseDecision: A string that will be returned when the Rule statement evaluates to false.
FalseDecisionData: Additional details that will be returned when the Rule statement evaluates to false.
(Note: False details are used only when a set of rule results are returned.)

Rules can be registered early.  
This allows for Rule registration at startup, potentially with multiple Context words.

Rules can be registered late.  
This allows a subset of rules to be registered when needed.

### The Rule Statement
A Rule Statement must follow the RightRule grammar (See below).  
Examples of syntactically correct statements are as follows:
- age equals 20
- age = 20
- age = 20 AND name = "Harry"
- age = 20 OR name = "Harry"

### Applying Rules
The Data upon which the rules will be applied must be declared as Dictionary<string, object>  
This data can then be sent to RightRule for rules to be applied.  
The rules will be selected for the specified Context and applied in order of the Rule number.
When applying Rules, it is also possible to specify if the first successful Rule provides the result, or if all rules need to be successful to provide a result

### Specifying a Context
At the point in your logic where the rules need to be applied, you will define a Context word.  
The Context word will be passed to RightRule so it can select the correct set of rules to apply.

### The RightRule process
When the Rules have been registered and the Data has been provided, RightRule will apply each Rule in turn and evaluate the result of the Rule statement
(Note: Some dependency on RulesToApply needs to be considered).  
If the Rule statement evaluates to false, the next Rule will be applied.  
If the Rule statement evaluates to true, the details of that Rule will be returned. No more rules will be applied.  
If no rules exists for the specified Context, or no Rule evaluates to true, a default Rule will be returned.

### Example Rule
```csharp
new RuleDefinition()
{
  Number = 1,
  Name = "FindHarryRule",
  Context = "FindPerson",
  Decision = "FOUND",
  Statement = @"name == ""Harry"" and surname equals ""Potter"" and age = 28",
  DecisionData = null
};
```

### Example implementation of RightRule
```csharp
{
  var rightRule = new RightRule.RightRule();
  Dictionary<string, object> data = MakeData();

  Rule rule = MakeRule();
  var registeredRule = rightRule.RegisterRule(rule);

  string context = "FindPerson";
  var appliedRule = rightRule.ApplyRules(context, data, RulesToApply.First);
}
```

### Definition of the Default Rule
```csharp
new AppliedRule()
{
  Number = -1,
  Name = "DefaultRule",
  Statement = string.Empty,
  Context = string.Empty,
  Decision = "NODECISION",
  DecisionData = null
};
```

## The RightRule grammar
expr       = or_expr ;

or_expr    = and_expr { "OR" and_expr } ;

and_expr   = not_expr { "AND" not_expr } ;

not_expr   = [ "NOT" ] primary ;

primary    = "(" expr ")"
           | condition ;

condition  = variable operator value ;  
condition  = variable operator variable ;

operator   = "contains"
           | "equals"
           | "notequals" ;

value      = number
           | string
           | boolean ;

