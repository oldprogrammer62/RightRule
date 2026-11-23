# Service reference

Each `IRightRuleSession` exposes rule, data, adaptor, and operator services. Their state belongs to that session. The implementation classes are internal; use the public interfaces shown here. The implementation-only method notes below explain internal behavior and are not callable by consuming applications.

## Rule service

`session.RuleService` implements `IRuleService`.

### Inspect and build rules

| Member | Behavior |
| --- | --- |
| `RegisteredRules` | Registered-rules collection. |
| `DefaultRules` | Default rule registrations grouped by context. |
| `RuleContexts` / `GetRuleContexts()` | Context names currently containing rules. |
| `GetRegisteredRules()` | All parsed rules. |
| `GetRegisteredRules(context)` | Rules in a context (context comparison is case-insensitive). |
| `GetRegisteredRule(context, name)` | Find one rule by case-insensitive context and name; returns null when absent. |
| `GetRegisteredRule(context, number)` | Find one rule by context and number; returns null when absent. |
| `GetRuleNumbers(context)` | Rule number and name pairs for a context. |
| `GetRuleNames(context)` | Rule names for a context. |
| `BuildRule()` | Starts the fluent rule-definition builder. |
| `BuildDefaultRule()` | Starts the fluent default-rule builder. |

Lookup methods requiring a context reject null, empty, or whitespace context values. Name-based lookup also validates the name.

### Register, load, remove, and serialize

| Member | Behavior |
| --- | --- |
| `RegisterRule(DefinedRule)` | Validates and parses a definition, then registers it; returns a status/message and parsed rule when available. |
| `RegisterRules(IEnumerable<DefinedRule>)` | Registers each definition and returns one result per input, including failures. |
| `LoadRules(string json)` | Deserializes rule definitions and registers them; returns an empty list if loading yields no definitions. |
| `DeRegisterRules()` | Removes all rules. |
| `DeRegisterRules(context)` | Removes all rules in the context. |
| `DeRegisterRule(context, name)` | Removes a named rule in the context. |
| `DeRegisterRule(context, number)` | Removes a numbered rule in the context. |
| `SerializeRules()` | Serializes all rules; returns an empty string when none exist. |
| `SerializeRules(context)` | Serializes rules in a context; returns an empty string when none exist. |

Registration validates the definition before parsing. Parse or validation errors are returned in `RuleRegistrationResult`; callers should inspect `Status` and `Message` rather than assume success.

### Default rules

| Member | Behavior |
| --- | --- |
| `RegisterDefaultRule(DefaultRule)` | Registers a default for the rule's context and returns `DefaultRuleRegistrationResult`. |
| `GetDefaultRule(context)` | Gets the context default; behavior when no default exists is provided by the default-rule registry. |

When applying a single rule, the context default is returned if no rule produces a positive result.

### Apply rules

| Member | Behavior |
| --- | --- |
| `ApplyRule(context)` / `ApplyRuleAsync(context)` | Return one result using first-match evaluation, or the context default when no rule succeeds. |
| `ApplyRules(context)` / `ApplyRulesAsync(context)` | Return a list, using first-match evaluation by default. |
| `ApplyRules(context, evaluationType)` / `ApplyRulesAsync(context, evaluationType)` | Return results using the requested evaluation mode (first or all). |

The synchronous forms block on their asynchronous equivalents. Prefer async methods in asynchronous code. Context is required. Application exceptions are represented as error `RightRuleResult` values by the service methods.

### Rule implementation helpers (internal)

The internal `ValidateRule` and `ParseRule` methods validate definitions and parse statements, passing the `Diagnostics` option into parsing. `ApplyRules_SingleResultAsync` selects the first applied result or builds a default; `ApplyRules_MultipleResultAsync` delegates evaluation to the rule application service. `BuildDefaultResult`, `BuildResults`, and the `BuildResult` overloads translate service results into public `RightRuleResult` values. `BuildResultObject` maps a processed rule's boolean result to success/failure. `MakeDecisionData` interpolates the true or false decision payload corresponding to the evaluated outcome. These are private implementation details.

## Data service

`session.DataService` implements `IDataService`.

| Member | Behavior |
| --- | --- |
| `RegisteredDatasets` | Registered dataset objects. |
| `DataSetNames` / `GetDataSetNames()` | Names of registered datasets. |
| `RegisterData<T>(datasetName, data)` | Runs input through the registered data adaptor pipeline and registers the resulting data. `T` must be non-null. |
| `LoadData(json)` | Loads serialized dataset definitions and adds them. |
| `GetRegisteredData()` | Returns all datasets. |
| `GetRegisteredData(datasetName)` | Finds a dataset by case-insensitive name; returns null when absent. |
| `GetDataItem(datasetName, itemName)` | Finds a data item or returns null. |
| `GetDataItemValue(datasetName, itemName)` | Returns the item's value, or null when either lookup fails. |
| `GetDataItemNames(datasetName)` | Item names for a dataset, or an empty list if missing. |
| `GetDataItemNames(dataset)` | Item names from the supplied dataset. |
| `DeRegisterData()` | Clears every dataset. |
| `DeRegisterData(datasetName)` | Removes the named dataset. |
| `SerializeData()` | Serializes all datasets; empty string when none exist. |
| `SerializeData(datasetName)` | Serializes one dataset; empty string when absent. |

`RegisterData` expects adaptors to produce an `AdaptorDataStructure`; input that cannot be adapted returns a failure result. For direct application code, register normal input through this public method rather than the internal overloads.

### Data implementation helpers (internal)

`IsValidDataIdentifier` delegates identifier validation to the data validator. The internal `AddData` overloads merge a dictionary, one `(string, object)` tuple, a list of such tuples, or a named value into a dataset, creating the dataset when necessary. `AddData(List<Dataset>)` adds loaded datasets and creates success results; `CreateDataSet` constructs and tracks a dataset. These implementation helpers are not part of `IDataService`.

## Data adaptor service

`session.DataAdaptorService` implements `IDataAdaptorService`.

| Member | Behavior |
| --- | --- |
| `RegisteredDataAdaptors` | Types of registered adaptors. |
| `RegisterDataAdaptor(IDataAdaptor)` | Adds an adaptor and returns its registration result. |
| `ProcessIncomingData<TInput>(datasetName, data)` | Runs data through the adaptor manager and returns its adapted structure. `TInput` must be non-null. |

The data service calls `ProcessIncomingData` as part of `RegisterData`. Register an adaptor when application input needs conversion to RightRule's dataset representation.

## Operator service

`session.OperatorService` implements `IOperatorService`.

| Member | Behavior |
| --- | --- |
| `Operators` / `GetOperators()` | Current operators; `GetOperators()` returns a list copy. |
| `OperatorAliases` / `GetRegisteredAliases()` | Alias-to-operator mapping. |
| `GetRegisteredAliases(operator)` | Aliases belonging to one operator. |
| `RegisterAllOperators()` | Reloads using default options (default operators enabled). |
| `RegisterAllOperators(options)` | Loads built-in and/or custom operators according to options; reports registration status. |
| `RegisterOperator(operator)` | Registers one operator if its name is not already present and its aliases are unique. |
| `UnloadOperator(name)` | Removes and disposes an operator found through one of its aliases; returns false when not found. |
| `GetRegisteredAliases()` | Returns the current alias-to-operator map. |
| `GetRegisteredAliases(operator)` | Returns the cleaned alias map for the specified operator. |

Aliases are normalized and matched without case sensitivity. Duplicate aliases prevent successful registration. Session disposal disposes registered operators.

### Operator implementation helpers (internal)

`LoadDefaultOperators` and `LoadCustomOperators` call their respective loaders; custom loading returns no operators when the configured path is blank or the file is missing. `LoadStartUpOperators` combines enabled sources and checks alias uniqueness. `LoadAllOperators` selects sources using the two load flags. `RegisterAllOperators(options)` replaces the current operator list and alias map with the selected startup set. `OperatorExists`, both `OperatorAliasesUnique` overloads, `MakeOperatorAliases`, and `RegisterOperatorAliases` support duplicate checks and alias-map construction. `Dispose()` disposes every currently registered operator. These are implementation details; the helper methods are internal or private, not application entry points.

## Options

`RightRuleOptions` is public and sealed. Its parameterless constructor reads the library's accompanying `.dll.config` boolean settings and uses defaults when settings are absent. `RightRuleOptions(IConfiguration)` starts with those defaults and overlays the `RightRule` section. The configuration keys are `DataCheck`, `Diagnostics`, `LoadCustomOperators`, `LoadDefaultOperators`, and `CustomOperatorLibraryPath`. A null configuration throws `ArgumentNullException`; invalid boolean text throws during parsing. Empty or whitespace boolean values preserve the existing fallback.

| Property | Default | Effect |
| --- | --- | --- |
| `DataCheck` | `false` | Enables data checking behavior. |
| `Diagnostics` | `false` | Enables detailed rule parsing diagnostics. |
| `LoadCustomOperators` | `true` | Enables custom operator loading. |
| `LoadDefaultOperators` | `true` | Enables built-in operator loading. |
| `CustomOperatorLibraryPath` | Library config path or empty | Path to custom operator configuration; relative paths from library config resolve from the library assembly directory. |

`RightRule` copies these values when creating each session. Configure options before creating a local `RightRule`, or supply options through DI registration.

### Options implementation helpers (internal)

`RightRuleLibraryDefaults.ReadSetting` reads a key from the library assembly's adjacent config file. `ReadConfiguration` opens that file when present. `ReadCustomOperatorLibraryPath` resolves `CustomOperatorConfigPath` relative to the assembly directory and returns an empty string when it is absent. `ReadBoolean` parses a configuration value or preserves its fallback; `ReadLibraryBoolean` parses a library config value or returns null to signal use of the property default. `RightRule.CheckOptions` fills an empty custom operator path with the library default and rejects the options only if custom loading is enabled and the path remains empty. `RightRule.CreateSession` copies options before validation, so subsequent caller mutations do not affect sessions. These helpers and `RightRuleLibraryDefaults` are internal implementation details.

## Overrides

`RightRule` and `RightRuleSession` are sealed and have no overridable members. Session disposal is an implementation of `IDisposable` and `IAsyncDisposable`, not an override. The library's built-in data adaptors override `AdaptorName` and `Process` from `DataAdaptorBase<T>`: object and string adaptors wrap the input as a single named value; the dictionary adaptor returns its entries and, when an `Id` key exists, uses that value as the resulting dataset name; tuple and tuple-list adaptors map named values into the adaptor result. Applications can provide their own adaptor implementation to transform additional input types before `RegisterData` stores them. The parsed, applied, error, and base rule model types also override `ToString()` to return a useful textual representation of the model; this is for display and diagnostics, not evaluation customization.
