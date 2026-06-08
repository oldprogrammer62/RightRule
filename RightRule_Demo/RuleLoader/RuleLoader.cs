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
                    Name = "New Customer Rule",
                    Statement = "NewCust = true",
                    Decision = "NEWCUST",
                    DecisionData = "{discount: 10, unit: '%'}"
                },
                new RuleDefinition
                {
                    Context = Context,
                    Number = 2,
                    Name = "Existing Customer Rule",
                    Statement = "NewCust = false",
                    Decision = "EXISTINGCUST",
                    DecisionData = "{discount: 25, unit: '%'}"
                }
            };
        }
    }
}
