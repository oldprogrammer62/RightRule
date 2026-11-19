---
name: csharp-library-docs
description: writes technical developer documentation for the RightRule library.
---

## 1. Scope & Rules
The Documentation should cover how RightRule can be used in a project. Include creating a new instance of RightRule, and when RightRule is included in a DI enabled project.
Do **not** modify existing C# code.
Do **not** create any new tests.
Do **not** create XML comments inside the code base.

Cover all public methods available in RightRule.cs and RightRuleSession.cs
Then cover each public and internal method in:
RuleService
DataService
DataAdaptorService
OperatorService
RightRuleOptions

Include all overrides available with details of what each does, and what each override offers the developer.

## 2. Markdown guide
Create the documents in the docs/developerdocs/ folder.
If it makes sense to split the documents into different files each one describing different aspects of RightRule usage, then go ahead.
