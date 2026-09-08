using BeSoft;
using RightRule.RuleModels.RuleModels;

namespace RightRule_Demo.RuleLoader
{
    internal class RuleLoader
    {
        internal static List<DefinedRule> Load(IRightRule rightRule, string Context)
        {
            return new List<DefinedRule>
            {
                rightRule.BuildRule()
                .Number(10)
                .Name("Rule10")
                .Context("Context")
                .Statement("a = b")
                .Decision("GOOD")
                .FalseDecision("BAD")
                .WithDataCheck(false)
                .Build(),

                rightRule.BuildRule()
                .Number(11)
                .Name("Rule11")
                .Context("Context")
                .Statement("a = b")
                .Decision("GOOD")
                .FalseDecision("BAD")
                .WithDataCheck(false)
                .Build(),

                new DefinedRule
                {
                    Context = Context,
                    Number = 1,
                    Name = "New Customer Rule - Exact Prize Value Order",
                    Statement = "Demo.NewCust = true and Demo.OrderValue equals 750",
                    Decision = "NEWCUST_EXACT_PRIZEVALUE",
                    DecisionData = "{discount: 50, unit: '%'}",
                    FalseDecision = "False Decision"
                },
                new DefinedRule
                {
                    Context = Context,
                    Number = 2,
                    Name = "New Customer Rule - Low Value Order",
                    Statement = "Demo.NewCust = true and Demo.OrderValue >= 0 and Demo.OrderValue < hv.value",
                    Decision = "NEWCUST_LOWVALUE",
                    DecisionData = "{discount: 5, unit: '%'}",
                    FalseDecision = "False Decision"
                },
                new DefinedRule
                {
                    Context = Context,
                    Number = 3,
                    Name = "New Customer Rule - High Value Order",
                    Statement = "Demo.NewCust = true and Demo.OrderValue >= hv.value",
                    Decision = "NEWCUST_HIGHVALUE",
                    DecisionData = "{discount: 20, unit: '%'}",
                    FalseDecision = "False Decision"
                },
                new DefinedRule
                {
                    Context = Context,
                    Number = 4,
                    Name = "Existing Customer Rule - Low Value Order",
                    Statement = "Demo.NewCust = false and Demo.OrderValue >= 0 and Demo.OrderValue < hv.value",
                    Decision = "EXISTINGCUST_LOWVALUE",
                    DecisionData = "{discount: 15, unit: '%'}",
                    FalseDecision = "False Decision"
                },
                new DefinedRule
                {
                    Context = Context,
                    Number = 5,
                    Name = "Existing Customer Rule - High Value Order",
                    Statement = "Demo.NewCust = false and Demo.OrderValue >= hv.value",
                    Decision = "EXISTINGCUST_HIGHVALUE",
                    DecisionData = "{discount: 25, unit: '%'}",
                    FalseDecision = "False Decision"
                }
            };
        }
    }
}
