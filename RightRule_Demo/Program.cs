using RightRule;
using RightRule.LanguageProcessor.Evaluator;
using RightRule.Models.BaseModels;
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
            var ReturnRules = GetRulesToReturn();
            var rules = ApplyRules(data, highValue, ReturnRules);

            Console.WriteLine();
            DisplayAppliedRules(rules);
            Console.WriteLine();

            Console.WriteLine("Press any key to exit...");
            Console.ReadLine();
        }

        private static EvaluationTypeEnum GetRulesToReturn()
        {
            var choice = RTRManager.GetRuleResultType();

            if (string.IsNullOrEmpty(choice))
            {
                return EvaluationTypeEnum.FIRST;
            }
            if (choice.Equals("s", StringComparison.CurrentCultureIgnoreCase))
            {
                return EvaluationTypeEnum.FIRST;
            }
            if (choice.Equals("a", StringComparison.CurrentCultureIgnoreCase))
            {
                return EvaluationTypeEnum.ANY;
            }
            return EvaluationTypeEnum.ANY;
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

        private static List<RightRuleResult<IProcessedRule>> ApplyRules(Dictionary<string, object> data, decimal HighValue, EvaluationTypeEnum ReturnRules)
        {
            return RuleManager.RuleManager.ApplyRules(data, HighValue, ReturnRules);
        }

        private static void DisplayAppliedRules(List<RightRuleResult<IProcessedRule>> appliedRules)
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

        private static void DisplayAppliedRule(RightRuleResult<IProcessedRule> appliedRule)
        {
            if (appliedRule == null)
            {
                Console.WriteLine("No rule applied.");
                return;
            }

            Console.WriteLine("Rule applied...");
            Console.WriteLine($"\tResult: {appliedRule.Result}");
            Console.WriteLine($"\tNumber: {appliedRule.RuleNumber}");
            Console.WriteLine($"\tName: {appliedRule.RuleName}");
            Console.WriteLine($"\tDecision: {appliedRule.Decision}");
            Console.WriteLine($"\tDecision Data: {appliedRule.DecisionData}");
            Console.WriteLine($"\tStatement Applied: {appliedRule.StatementApplied}");
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
