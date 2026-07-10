using RightRule.Rules;
using RightRule.System.Common;
using RightRule_Demo.DataManager;

namespace RightRule_Demo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Welcome();

            var data = GetData();
            var highValue = GetHighValue();
            var rulesToReturn = GetRulesToReturn();
            var rules = ApplyRules(data, highValue, rulesToReturn);

            Console.WriteLine();
            DisplayAppliedRules(rules);
            Console.WriteLine();

            Console.WriteLine("Press any key to exit...");
            Console.ReadLine();
        }

        private static RulesToReturn GetRulesToReturn()
        {
            var choice = RTRManager.GetRuleResultType();

            if (string.IsNullOrEmpty(choice))
            {
                return RulesToReturn.First;
            }
            if (choice.Equals("s", StringComparison.CurrentCultureIgnoreCase))
            {
                return RulesToReturn.First;
            }
            if (choice.Equals("a", StringComparison.CurrentCultureIgnoreCase))
            {
                return RulesToReturn.All;
            }
            return RulesToReturn.All;
        }

        private static decimal GetHighValue()
        {
            var highValue = HighValueManager.GetHighValue();
            return highValue ?? 100m;
        }

        private static Dictionary<string, object> GetData()
        {
            var data = new Dictionary<string, object>();
            var customerData = GetCustomerData();
            if (customerData != null)
            {
                foreach (var kvp in customerData)
                {
                    data[kvp.Key] = kvp.Value;
                }
            }
            var orderValue = GetOrderValue();
            data["OrderValue"] = orderValue;
            return data;
        }

        private static Dictionary<string, object>? GetCustomerData()
        {
            return CustomerManager.GetCustomerData();
        }

        private static decimal GetOrderValue()
        {
            return OrderManager.GetOrderValue();
        }

        private static List<AppliedRule> ApplyRules(Dictionary<string, object> data, decimal HighValue, RulesToReturn rulesToReturn)
        {
            return RuleManager.RuleManager.ApplyRules(data, HighValue, rulesToReturn);
        }

        private static void DisplayAppliedRules(List<AppliedRule> appliedRules)
        {
            if (appliedRules == null || appliedRules.Count == 0)
            {
                Console.WriteLine("No rules applied.");
                return;
            }
            Console.WriteLine("Applied Rules:");
            foreach (var appliedRule in appliedRules)
            {
                DisplayAppliedRule(appliedRule);
                Console.WriteLine();
            }
        }

        private static void DisplayAppliedRule(AppliedRule appliedRule)
        {
            if (appliedRule == null)
            {
                Console.WriteLine("No rule applied.");
                return;
            }

            Console.WriteLine("Rule applied...");
            Console.WriteLine($"\tResult: {appliedRule.Result}");
            Console.WriteLine($"\tNumber: {appliedRule.Number}");
            Console.WriteLine($"\tName: {appliedRule.Name}");
            Console.WriteLine($"\tDecision: {appliedRule.Decision}");
            Console.WriteLine($"\tDecision Data: {appliedRule.DecisionData}");
            Console.WriteLine($"\tStatement: {appliedRule.Statement}");
            Console.WriteLine($"\tStatement Applied: {appliedRule.StatementApplied}");
            Console.WriteLine($"\tError Message: {appliedRule.ErrorMessage}");
        }

        private static void Welcome()
        {
            string WelcomeMessage = "* Welcome to the RightRule Demo! *";
            Console.WriteLine(new string('*', WelcomeMessage.Length));
            Console.WriteLine(WelcomeMessage);
            Console.WriteLine(new string('*', WelcomeMessage.Length));

            Console.WriteLine();
        }
    }
}
