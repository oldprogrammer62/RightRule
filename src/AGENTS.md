# Repository Guidelines

## Project Structure & Module Organization

RightRule is a .NET 10 C# library. The root `RightRule.csproj` builds and packages the library. Top-level folders group code by responsibility: `DataServices`, `DataAdaptors`, `DataTransformer`, `RuleServices`, `LanguageProcessor`, `Operators`, `Factories`, and `ServiceRegister`. Shared models and exceptions live in `Models` and `Exceptions`; user and technical documentation is under `Docs`. Custom operator configuration and binaries are under `Operators/CustomOperators`. `bin` and `obj` contain generated build output; do not edit them.

## Build, Test, and Development Commands

- `dotnet restore RightRule.csproj` restores NuGet and project-reference dependencies.
- `dotnet build RightRule.csproj` builds the library and generates its package because package generation is enabled in the project.
- `dotnet pack RightRule.csproj --no-build` packages a previously built project.

Tests can be found in ..\test\RightRule.UnitTest

If adding tests, follow the existing `RightRuleTests` assembly visibility declaration and document the test project’s run command.

## Coding Style & Naming Conventions

Follow the existing C# style: four-space indentation, nullable reference types enabled, and implicit usings. Use PascalCase for public types and members, and descriptive filenames matching their primary type (for example, `RuleValidationService.cs`). Keep related implementations in their existing responsibility-based folders and update interfaces alongside service behavior. Preserve public API compatibility unless a change explicitly requires otherwise.

## Testing Guidelines

Tests can be found in ..\test\RightRule.UnitTest
Tests should only be added if gaps are found in existing test coverage, or new functionality is developed.

## Commit & Pull Request Guidelines

Recent commits use short, plain descriptions of the change rather than a visible ticket or conventional-commit prefix. Keep commit subjects concise and action-oriented. Pull requests should explain the behavior changed, note compatibility or configuration impacts, link relevant issues, and include test/build results. Add screenshots only when a change affects a visual artifact or documentation rendering.

## Configuration & Generated Files

Keep operator and application configuration changes in the appropriate source configuration files. Do not commit generated `bin` or `obj` output. Package metadata, target framework, dependencies, and packaging rules are defined in `RightRule.csproj`.
