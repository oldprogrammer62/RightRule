# Copilot Instructions

## Project Guidelines
- For RightRule, the library itself should own its configuration and provide defaults from its own config file; host applications should be able to override settings, but if they omit a setting, RightRule should use its library-owned default.
- RightRule is intended to be used as a per-session instance: construct it, register data and rules, apply rules, then dispose it when the instance leaves scope.

## Code Style
- When composing token sequences for a `params Token[]` helper, expand each `Token[]` sequence instead of passing it as a single argument.

## Testing Instructions
- When generating tests, provide the actual test code as edits in the response rather than a file-location placeholder.