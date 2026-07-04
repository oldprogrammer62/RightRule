using RightRule.Rules;

namespace RightRule_Demo.RuleLoader
{
    internal class RuleLoader
    {
        internal static List<RuleDefinition> Load(string Context, object HighValue)
        {
            return new List<RuleDefinition>
            {
                new RuleDefinition
                {
                    Context = Context,
                    Number = 1,
                    Name = "New Customer Rule - Exact Prize Value Order",
                    Statement = "NewCust = true and OrderValue equals 750",
                    Decision = "NEWCUST_EXACT_PRIZEVALUE",
                    DecisionData = "{discount: 50, unit: '%'}",
                    Arguments = new Dictionary<string, object>
                    {
                        { "hv", HighValue }
                    }
                },
                new RuleDefinition
                {
                    Context = Context,
                    Number = 2,
                    Name = "New Customer Rule - Low Value Order",
                    Statement = "NewCust = true and OrderValue >= 0 and OrderValue < {hv}",
                    Decision = "NEWCUST_LOWVALUE",
                    DecisionData = "{discount: 5, unit: '%'}",
                    Arguments = new Dictionary<string, object>
                    {
                        { "hv", HighValue }
                    }
                },
                new RuleDefinition
                {
                    Context = Context,
                    Number = 3,
                    Name = "New Customer Rule - High Value Order",
                    Statement = "NewCust = true and OrderValue >= {hv}",
                    Decision = "NEWCUST_HIGHVALUE",
                    DecisionData = "{discount: 20, unit: '%'}",
                    Arguments = new Dictionary<string, object>
                    {
                        { "hv", HighValue }
                    }
                },
                new RuleDefinition
                {
                    Context = Context,
                    Number = 4,
                    Name = "Existing Customer Rule - Low Value Order",
                    Statement = "NewCust = false and OrderValue >= 0 and OrderValue < {hv}",
                    Decision = "EXISTINGCUST_LOWVALUE",
                    DecisionData = "{discount: 15, unit: '%'}",
                    Arguments = new Dictionary<string, object>
                    {
                        { "hv", HighValue }
                    }
                },
                new RuleDefinition
                {
                    Context = Context,
                    Number = 5,
                    Name = "Existing Customer Rule - High Value Order",
                    Statement = "NewCust = false and OrderValue >= {hv}",
                    Decision = "EXISTINGCUST_HIGHVALUE",
                    DecisionData = "{discount: 25, unit: '%'}",
                    Arguments = new Dictionary<string, object>
                    {
                        { "hv", HighValue }
                    }
                }
            };
        }
    }
}
