using RightRule_Demo.Data;

namespace RightRule_Demo.CustomerManager
{
    internal class CustomerManager
    {
        internal static Dictionary<string, object> GetCustomerData()
        {
            var customer = GetCustomerRecord();
            return CreateCustomerSet(customer);
        }

        private static Customer? GetCustomerRecord()
        {
            Console.WriteLine("Please enter a Customer Code...");

            var customerCode = Console.ReadLine();
            if (string.IsNullOrEmpty(customerCode))
            {
                Console.WriteLine("Customer Code cannot be empty. Exiting.");
                Console.WriteLine("Press any key to exit...");
                Console.ReadLine();
                return default;
            }

            Console.WriteLine();

            var customer = DataLoader.CustomerLoader.Load(customerCode);
            if (customer != null)
            {
                Console.WriteLine($"Customer found: {customer.Name} (ID: {customer.Id})");
            }
            else
            {
                Console.WriteLine("Customer not found.");
            }

            return customer;
        }

        private static Dictionary<string, object> CreateCustomerSet(Customer? customer)
        {
            if (customer == null)
            {
                return CreateCustomerSet();
            }

            var data = new Dictionary<string, object>();
            data["CustomerCode"] = customer.Code;
            data["CustomerName"] = customer.Name;
            data["NewCust"] = false;

            return data;
        }

        private static Dictionary<string, object> CreateCustomerSet()
        {
            var data = new Dictionary<string, object>();
            data["CustomerCode"] = string.Empty;
            data["CustomerName"] = string.Empty;
            data["NewCust"] = true;

            return data;
        }
    }
}
