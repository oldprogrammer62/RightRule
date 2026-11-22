using RightRule.RightRuleSession;
using RightRule.RuleServices.RuleModels;

namespace RightRule_Demo.RuleLoader
{
    internal class RuleLoader
    {
        internal static List<DefinedRule> Load(IRightRuleSession rr, string Context)
        {
            return new List<DefinedRule>
            {
                rr.RuleService.BuildRule()
                .Number(10)
                .Name("Rule10")
                .Context("Context")
                .Statement("a = b")
                .TrueDecision("GOOD")
                .Build(),

                rr.RuleService.BuildRule()
                .Number(11)
                .Name("Rule11")
                .Context("Context")
                .Statement("a = b")
                .TrueDecision("GOOD")
                .Build(),

                new DefinedRule
                {
                    Context = Context,
                    Number = 1,
                    Name = "New Customer Rule - Exact Prize Value Order",
                    Statement = "Demo.NewCust = true and Demo.OrderValue equals 750",
                    TrueDecision = "NEWCUST_EXACT_PRIZEVALUE",
                    TrueDecisionData = "{discount: 50, unit: '%'}",
                },
                new DefinedRule
                {
                    Context = Context,
                    Number = 2,
                    Name = "New Customer Rule - Low Value Order",
                    Statement = "Demo.NewCust = true and Demo.OrderValue >= 0 and Demo.OrderValue < hv.value",
                    TrueDecision = "NEWCUST_LOWVALUE",
                    TrueDecisionData = "{discount: 5, unit: '%'}",
                },
                new DefinedRule
                {
                    Context = Context,
                    Number = 3,
                    Name = "New Customer Rule - High Value Order",
                    Statement = "Demo.NewCust = true and Demo.OrderValue >= hv.value",
                    TrueDecision = "NEWCUST_HIGHVALUE",
                    TrueDecisionData = "{discount: 20, unit: '%'}",
                },
                new DefinedRule
                {
                    Context = Context,
                    Number = 4,
                    Name = "Existing Customer Rule - Low Value Order",
                    Statement = "Demo.NewCust = false and Demo.OrderValue >= 0 and Demo.OrderValue < hv.value",
                    TrueDecision = "EXISTINGCUST_LOWVALUE",
                    TrueDecisionData = "{discount: 15, unit: '%'}",
                },
                new DefinedRule
                {
                    Context = Context,
                    Number = 5,
                    Name = "Existing Customer Rule - High Value Order",
                    Statement = "Demo.NewCust = false and Demo.OrderValue >= hv.value",
                    TrueDecision = "EXISTINGCUST_HIGHVALUE",
                    TrueDecisionData = "{discount: 25, unit: '%'}",
                }
            };
        }
    }
}
