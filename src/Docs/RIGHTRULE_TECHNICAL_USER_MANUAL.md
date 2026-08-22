# RightRule Technical User Manual


## Overview

RightRule is a .NET 8 rules engine for evaluating text-based rule statements against named data.
A rule belongs to a context, has an evaluation order, and returns a decision when its statement evaluates successfully.
Applications can register rules and data early at startup, or provide both at the required point of evaluation.
Data can be in the form of Key/Value pairs in a Dictionary<string, object> or as a name and variable reference.


## Core Concepts

Important decisions when implementing RightRule in your application:

1. Where are decisions made during execution of your application code?
2. What decision is being made?
3. What data is required to make a decision?
4. What logic is to be applied to make the decision?
5. What needs to happen when a decision is made?
6. Is there other data that can be used when a decision is made?


Implementing RightRule in your application.

1. Initialise RightRule.
2. Register Data. Data can be registered at any point in the RightRule life. At startup, when the data exists, when the rules are evaluated.
3. Register Rules. Rules are registered as a RuleDefinition object, returned as a ParsedRule object.
4. Call RightRule to evaluate Rules for a specified Context.
5. Receive the result from RightRule as an AppliedRule object.
6. Check AppliedRule.Result before acting on Decision.
7. Generate code to handle all expected Decision words for the specified Context.
8. If additional data (DecisionData) is also returned, develop code to handle the contents.

Important considerations to ensure accurate behaviour of RightRule

1. Validate rule syntax before production use.
2. Ensure data names match statement variable names exactly.
3. Log `StatementPath` and `StatementApplied` for diagnostics.


### Data
RightRule can accept data as either:

1. A Dataset - Dictionary<string,object>. Contains Key Value Pairs made up of string name, and object value.

```csharp
Dictionary<string, object> _Data = new Dictionary<string, object>(StringComparer.Ordinal)
{
   { "attribute1", "John" },
   { "attribute2", true },
   { "attribute3", false },
   { "attribute4", 17 }
};
BeSoft.IRightRule rr = new BeSoft.RightRule();
bool registered = rr.RegisterData("dataset1", _Data);
```

2. A Data Object - Contains a single name value pair, made up of a string name, and an object value.

```csharp
int x = 100; // Intrinsic type
BeSoft.IRightRule rr = new BeSoft.RightRule();
var attribute5 = rr.RegisterData("x", x);
```

```csharp
var xtype = new xtype(); // Where xtype is a custom type defined with properties for name and a value
BeSoft.IRightRule rr = new BeSoft.RightRule();
var attribute5 = rr.RegisterData("xtype", x);
```

Each data is registered so the values can be discovered whilst RightRule is evaluating a rule statement.
Registration provides RightRule with a name for the data, followed by the actual details of the data.


### RuleDefinition class. Used to define a rule for registration.
* Number {integer}
* Name {string}
* Context {string}
* Statement {string}
* OptionalData {object?}
* Decision {string}
* DecisionData {object?}

## RuleDefinition Properties
* Number: Rule order; lower numbers are evaluated first.
* Name: Descriptive rule name for logging or diagnostics.
* Context: A string that binds the rule to an event specified in application code.
* Statement: RightRule expression to evaluate.
* OptionalData: Sent to the logic Operator.
* Decision: Word returned to the calling application on a successful rule match.
* DecisionData: Additional result payload. Provides additional data that can be used by the application.


### ParsedRule class. Extends RuleDefinition adding:
Returned by the rule registration process.
* Node {Node}
* StatementPath {string}
* Message {string}

## ParsedRule Properties. As RuleDefinition plus:
* Node: Represents the Abstract Syntax Tree that defines the logic of the rule statement.
* StatementPath: String version of Abstract Tree (Node).
* Message: Message returned if the Rule statement does not Parse successfully.



### AppliedRule class. Extends RuleDefinition adding:
Returned by the rule evaluation process.
* Result {bool}
* StatementApplied {string}
* Message {string}

## AppliedRule Properties. As RuleDefinition plus:
* Result: true/false result of applying the rule.
* StatementApplied: Shows the actual rule statement applied with data values.
* Message: Message returned if an error occurs during rule evaluation.


### Rule Statement
A rule definition includes the statement that will be evaluated to return the result of the rule.
The statement must be syntactically correct. (See grammar.txt)

The basic syntax of a statement is: Dataname dot(.) Field operator Value

For example: 
```text
dataset1.attribute1 = "John"
This example references the registered data "dataset1", specifically the "attribute1" field.
The logic operator is "=". This refers to an alias of the EqualsOperator class.
The logic then compares to a literal value "John"
```

Examples:
```text
person.age equals 20
person.age = config.age
person.age = 20 AND person.name = "Harry"
(person.age = 20 OR person.name = "Harry") AND orderhistory.goodpayer equals true
NOT person.status equals "blocked"
```


### Rule evaluation
Once data and rules have been registered with RightRule, the rules can be evaluated and a result returned to the calling application.
Rules can be evaluated in 1 of 2 modes:

## First
Rules are evaluated in the order of their defined number.
The first rule to return a true result will be returned as the applied rule. No further rules will be evaluated.
If no rule returns a true result, a default applied rule will be returned.

## All
Rules are evaluated in the order of their defined number.
All rules will be added to a set of applied rules regardless of result, true or false.
The set of applied rules will be returned to the calling application.


### Decision:
If the Result is true, the AppliedRule Decision word will be the same as the ParsedRule decision.
If the Result is false, the AppliedRule Decision word will be "NODECISION".
If an error occurred, the AppliedRFule Decision word will be "ERROR".

### Decision Handling
The application code needs to handle all expected Decisions that RightRule may return as Applied Rules.

If the rule was defined with Decision Data, that will be returned as part of the Applied Rule.
The structure and content of the Decision Data will be known to the application.
The decision data can be used to enhance the decision handling process.


## Default Operators
A set of Operators are built in RightRule and available for inclusion in a rule statement.

Contains
DoesNotContain
Equals
GreaterThanOrEquals
LessThan
NotEquals


## Custom Operators
Custom Operators can be developed and added to RightRule. These can implement domain specific requirements.

Custom operators implement `RightRule.System.Common.IOperator`.

```csharp
using RightRule.System.Common;

public sealed class StartsWithOperator : IOperator
{
    public List<string> Identifiers => ["startswith"];

    public bool Process(object left, object right)
    {
        return 
            left?.ToString()?.
            StartsWith(right?.ToString() ?? string.Empty, StringComparison.OrdinalIgnoreCase) == true;
    }
}
```

Custom operators are loaded from paths listed in the JSON file configured by `CustomOperatorConfigPath` in `rightrule.dll.config`.

Operator identifiers must be unique across built-in and custom operators.

