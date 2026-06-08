using RightRule.Rules;

namespace RightRule_Demo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Welcome();

            var CustomerData = GetCustomerData();
            if (CustomerData == null)
            {
                Console.WriteLine("No customer data available.");
                Console.WriteLine();
                Console.WriteLine("Press any key to exit...");
                Console.ReadLine();
                return;
            }

            var rule = ApplyRules(CustomerData);

            Console.WriteLine();
            DisplayAppliedRule(rule);
            Console.WriteLine();

            Console.WriteLine("Press any key to exit...");
            Console.ReadLine();
        }

        private static Dictionary<string, object>? GetCustomerData()
        {
            return CustomerManager.CustomerManager.GetCustomerData();
        }

        private static AppliedRule ApplyRules(Dictionary<string, object> data)
        {
            return RuleManager.RuleManager.ApplyRules(data);
        }

        private static void DisplayAppliedRule(AppliedRule appliedRule)
        {
            if (appliedRule == null)
            {
                Console.WriteLine("No rule applied.");
                return;
            }

            Console.WriteLine("Rule applied...");
            Console.WriteLine($"\tNumber: {appliedRule.Number}");
            Console.WriteLine($"\tName: {appliedRule.Name}");
            Console.WriteLine($"\tDecision: {appliedRule.Decision}");
            Console.WriteLine($"\tDecision Data: {appliedRule.DecisionData}");
            Console.WriteLine($"\tDataset Matched: {appliedRule.DatasetMatched}");
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
