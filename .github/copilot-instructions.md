# Copilot Instructions

## Project Guidelines
- For RightRule, the library itself should own its configuration and provide defaults from its own config file; host applications should be able to override settings, but if they omit a setting, RightRule should use its library-owned default.
- RightRule is intended to be used as a per-session instance: construct it, register data and rules, apply rules, then dispose it when the instance leaves scope.
- The custom operator projects in the solution are test/proof-of-concept examples, not production operators to be shipped or deployed by RightRule.

## Code Style
- When composing token sequences for a `params Token[]` helper, expand each `Token[]` sequence instead of passing it as a single argument.
- Use the exact casing for constructor parameters, such as `IsCollectible` (capital I and C), when calling it with a named argument.

## Testing Instructions
- When generating tests, provide the actual test code as edits in the response rather than a file-location placeholder.
- In the RuleService test helper, use `Enqueue` to enqueue values into the parse-result queue, which is a `Queue<ParsedRule?>`, instead of using `Add`.