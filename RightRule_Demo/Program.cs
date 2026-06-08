using RightRule.Rules;

namespace RightRule_Demo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to the RightRule Demo!");

            var Data = GetCustomerData();
            var rule = ApplyRules(Data);

            Console.WriteLine();
            DisplayAppliedRule(rule);
            Console.WriteLine();

            Console.WriteLine("Press any key to exit...");
            Console.ReadLine();
        }

        private static Dictionary<string, object> GetCustomerData()
        {
            return CustomerManager.CustomerManager.GetCustomerData();
        }

        private static AppliedRule ApplyRules(Dictionary<string, object> data)
        {
            return RuleManager.RuleManager.ApplyRules(data);
        }

        private static void DisplayAppliedRule(AppliedRule appliedRule)
        {
            Console.WriteLine("Rule applied...");
            Console.WriteLine($"\tNumber: {appliedRule.Number}");
            Console.WriteLine($"\tName: {appliedRule.Name}");
            Console.WriteLine($"\tDecision: {appliedRule.Decision}");
            Console.WriteLine($"\tDecision Data: {appliedRule.DecisionData}");
            Console.WriteLine($"\tDataset Matched: {appliedRule.DatasetMatched}");
        }
    }
}
