using RightRule.Rules;

namespace RightRule_Demo.RuleLoader
{
    internal class RuleLoader
    {
        internal static List<RuleDefinition> Load(string Context)
        {
            return new List<RuleDefinition>
            {
                new RuleDefinition
                {
                    Context = Context,
                    Number = 1,
                    Name = "New Customer Rule - Low Value Order",
                    Statement = "NewCust = true and OrderValue >= 0 and OrderValue < 100",
                    Decision = "NEWCUST_LOWVALUE",
                    DecisionData = "{discount: 5, unit: '%'}"
                },
                new RuleDefinition
                {
                    Context = Context,
                    Number = 2,
                    Name = "New Customer Rule - High Value Order",
                    Statement = "NewCust = true and OrderValue >= 100",
                    Decision = "NEWCUST_HIGHVALUE",
                    DecisionData = "{discount: 20, unit: '%'}"
                },
                new RuleDefinition
                {
                    Context = Context,
                    Number = 3,
                    Name = "Existing Customer Rule - Low Value Order",
                    Statement = "NewCust = false and OrderValue >= 0 and OrderValue < 100",
                    Decision = "EXISTINGCUST_LOWVALUE",
                    DecisionData = "{discount: 15, unit: '%'}"
                },
                new RuleDefinition
                {
                    Context = Context,
                    Number = 4,
                    Name = "Existing Customer Rule - High Value Order",
                    Statement = "NewCust = false and OrderValue >= 100",
                    Decision = "EXISTINGCUST_HIGHVALUE",
                    DecisionData = "{discount: 25, unit: '%'}"
                }
            };
        }
    }
}
