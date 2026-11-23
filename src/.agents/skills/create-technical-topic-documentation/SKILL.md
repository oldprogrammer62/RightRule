---
name: create-rightrule-technical-topic-docs
description: writes technical developer documentation for the RightRule library.
---

## 1. Scope & Rules
The Documentation should cover technical details of each topic, with examples where it makes the document more useful.
- The RightRule language syntax
- Registering data. Cover all data types that can be registered
- Developing and registering custom data adaptors
- Registering rules. Include details of using the fluent pattern Build structure.
- Registering default rules and why they are important.
- Developing and registering custom Operators
- Rule structure
    - Cover the importance of the "Context" field.
    - The rule statement, include details of Operators and how they are used in a rule statement
    - The decision, include how the decision word can be used to influence the flow of a users code
    - The decisiondata, include how this can return influential details to the calling app along with the decision word
- Applying rules. Include details of all methods for applying rules, and describe the results from each method.

Do **not** modify existing C# code.
Do **not** create any new tests.
Do **not** create XML comments inside the code base.
Document names:
- Technical-Rule-Language-and-Definitions.md
- Technical-Rule-Application.md
- Technical-Operators.md
- Technical-Data-Registration-and-Adaptors.md
Do **not** modify any other existing documents.
Do create new Technical docs if other topics are discovered.

## 2. Markdown guide
Create the documents in the Docs/DeveloperDocs/ folder.
If it makes sense to split the documents into different files each one describing different topics then that is preferable
