# Getting started

## Create a local instance

Construct `RightRule` with default options, configured options, a logger factory, or both. Then create and dispose a session for the work being performed.

```csharp
using Microsoft.Extensions.Logging;
using RightRule;
using RightRule.Models;
using RightRule.RightRuleSession;

var options = new RightRuleOptions
{
    DataCheck = false,
    Diagnostics = true,
    LoadDefaultOperators = true,
    LoadCustomOperators = false
};

var rightRule = new RightRule.RightRule(options);
using IRightRuleSession session = rightRule.CreateSession();

var syntax = await session.StatementSyntaxCheckAsync("age >= 18");
```

If an application already configures logging, pass its `ILoggerFactory` to `RightRule`. The logger factory is used by services created in each session. With no factory supplied, RightRule uses a null logger factory.

```csharp
var rightRule = new RightRule.RightRule(options, loggerFactory);
```

The four constructors are `RightRule()`, `RightRule(RightRuleOptions)`, `RightRule(ILoggerFactory)`, and `RightRule(RightRuleOptions, ILoggerFactory)`. The options and logger factory arguments cannot be null.

## Configure options

`RightRuleOptions` controls session behavior:

| Option | Default | Purpose |
| --- | --- | --- |
| `DataCheck` | `false` | Enables data checking behavior. |
| `Diagnostics` | `false` | Enables diagnostic behavior. |
| `LoadCustomOperators` | `true` | Loads custom operators. When enabled, a usable custom operator library path must be available. |
| `LoadDefaultOperators` | `true` | Loads the built-in default operators. |
| `CustomOperatorLibraryPath` | Library configured default, if present | Path used to locate custom operator configuration and libraries. |

Options can be initialized in code or from an `IConfiguration` section named `RightRule`. The configuration constructor reads the same option names shown above; boolean values must parse as booleans. The library also reads fallback boolean settings and `CustomOperatorConfigPath` from its accompanying `.dll.config` file. Relative custom operator paths from that file are resolved relative to the library assembly. If no path is configured, the library default path is empty; disable `LoadCustomOperators` when custom operators are not needed.

For local instance creation, `CreateSession()` copies option values into session-specific options. It fills an empty custom operator path with the library default and throws `ArgumentException` if the resulting options are invalid. Changes to the original options object after creating `RightRule` do not reconfigure that instance's sessions.

## Register with dependency injection

In an ASP.NET Core or other Microsoft DI application, register RightRule once in the service collection. Sessions are registered as scoped services, so resolve them from a scope (an HTTP request scope is suitable for web applications).

```csharp
using RightRule.Models;
using RightRule.RightRuleSession;
using RightRule.ServiceRegister;

var services = new ServiceCollection();
services.AddLogging();
services.AddRightRule(options =>
{
    options.Diagnostics = true;
    options.LoadDefaultOperators = true;
    options.LoadCustomOperators = false;
});

using var provider = services.BuildServiceProvider();
using var scope = provider.CreateScope();
var session = scope.ServiceProvider.GetRequiredService<IRightRuleSession>();

var syntax = await session.StatementSyntaxCheckAsync("age >= 18");
```

Registration supports `AddRightRule()`, `AddRightRule(RightRuleOptions)`, and `AddRightRule(Action<RightRuleOptions>)`. Each returns the `IServiceCollection`, so further registrations can be chained. The no-argument overload uses default options. The options overload registers the supplied options instance. The action overload creates default options, applies the action, then registers them. Registration fills a blank custom operator path with the library default and adds RightRule's scoped services and logging services. The application owns the root provider and scopes it creates; dispose them according to the host's lifetime model.

## Disposal

Always dispose local sessions. A session created by `RightRule.CreateSession()` owns its private service provider and disposes it. A DI-created session does not own the application's provider; disposing the DI scope controls the lifetime of that session and its scoped dependencies. Both synchronous and asynchronous disposal are supported.
