# Sessions and public API

`RightRule.CreateSession()` returns an `IRightRuleSession`. A session is the entry point to the services that load, register, inspect, and apply rules and data. Use the relevant service properties for those operations.

## `RightRule` API

| Member | Description |
| --- | --- |
| `RightRule()` | Creates an instance using default `RightRuleOptions` and `NullLoggerFactory`. |
| `RightRule(RightRuleOptions options)` | Creates an instance with the supplied options and `NullLoggerFactory`. `options` must not be null. |
| `RightRule(ILoggerFactory loggerFactory)` | Creates an instance with default options and the supplied logger factory. `loggerFactory` must not be null. |
| `RightRule(RightRuleOptions options, ILoggerFactory loggerFactory)` | Creates an instance with both dependencies. Neither argument may be null. |
| `CreateSession()` | Validates copied session options, builds a private DI provider, and returns a session that owns that provider. It throws `ArgumentException` when options are invalid. |

## `IRightRuleSession` properties

| Property | Description |
| --- | --- |
| `RuleService` | Rule operations, including rule registration, loading, retrieval, validation, serialization, and application. |
| `DataService` | Data operations exposed by RightRule. |
| `DataAdaptorService` | Registration and use of data adaptors. |
| `OperatorService` | Access to operator operations and registration. The interface annotation is nullable; check it before use in nullable-aware code. |
| `RightRuleOptions` | Effective options used by this session. |
| `ServiceProvider` | The provider from which session services were resolved. Use this for advanced integration with the session's service scope. |
| `Logger` | Logger associated with this session. |

The services expose their own public interfaces; consult those interface APIs for their individual methods.

## Syntax checks

`StatementSyntaxCheck(string Statement)` checks a rule statement synchronously and returns a `CheckStatementResult`. `StatementSyntaxCheckAsync(string Statement)` provides the asynchronous form. Both return a failure result for null, empty, or whitespace input; otherwise they delegate to RightRule's syntax checker. The synchronous method blocks for the asynchronous check, so prefer the asynchronous method in asynchronous application code.

## Lifetime methods

`Dispose()` releases a session synchronously. `DisposeAsync()` releases it asynchronously, preferring asynchronous provider disposal when available. Both are idempotent: only the first disposal performs cleanup. For a locally created session, disposal tears down its owned provider. For a DI-created session, these methods do not dispose the host's provider; let the DI scope manage the scoped service lifetime. Use `using` or `await using` to ensure cleanup.

## Inheritance and overrides

`RightRule` and `RightRuleSession` are sealed classes. They expose no virtual or override methods, so developers cannot customize these entry points through subclassing. `RightRuleSession` implements `IDisposable` and `IAsyncDisposable`; these are interface implementations, not overrides. For data conversion, implement an `IDataAdaptor` (or derive from the applicable `DataAdaptorBase<T>` and override `AdaptorName` and `Process`) and register it with `DataAdaptorService`. The built-in adaptors override those members for objects, dictionaries, strings, tuples, and lists of `(string, int)` tuples. `DictionaryDataAdaptor` uses an `Id` value as the output dataset name when present; the other built-ins use the supplied dataset name. Rule result model `ToString()` overrides provide textual representations for parsed, applied, and error rules. These are library implementation details, not hooks for overriding session behavior.
