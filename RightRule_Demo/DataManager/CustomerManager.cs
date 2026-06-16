using RightRule_Demo.Data;

namespace RightRule_Demo.DataManager;

internal class CustomerManager
{
    internal static Dictionary<string, object>? GetCustomerData()
    {
        Console.WriteLine("Existing Customer Codes: ");
        Console.WriteLine("\tC001 = Tom");
        Console.WriteLine("\tC002 = Dick");
        Console.WriteLine("\tC003 = Harry");

        var customerCode = GetCustomerCode() ?? string.Empty;
        if (string.IsNullOrEmpty(customerCode))
        {
            Console.WriteLine();
            return default;
        }

        var customer = GetCustomerRecord(customerCode);
        return CreateCustomerData(customer, customerCode);
    }

    private static string? GetCustomerCode()
    {
        Console.WriteLine("Please enter a Customer Code...");

        var customerCode = Console.ReadLine();
        if (string.IsNullOrEmpty(customerCode))
        {
            Console.WriteLine("Customer Code cannot be empty.");
            return default;
        }
        Console.WriteLine();
        return customerCode;
    }

    private static Customer? GetCustomerRecord(string? customerCode)
    {
        if (string.IsNullOrEmpty(customerCode))
        {
            return default;
        }

        var customer = DataLoader.CustomerLoader.Load(customerCode);
        if (customer != null)
        {
            Console.WriteLine($"Customer found: {customerCode.ToUpper()} {customer.Name} (ID: {customer.Id})");
        }
        else
        {
            Console.WriteLine($"Customer not found: {customerCode.ToUpper()}");
        }
        return customer;
    }

    private static Dictionary<string, object> CreateCustomerData(Customer? customer, string code)
    {
        if (customer == null)
        {
            return CreateCustomerData(code);
        }

        var data = new Dictionary<string, object>();
        data["CustomerCode"] = customer.Code;
        data["CustomerName"] = customer.Name;
        data["NewCust"] = false;

        return data;
    }

    private static Dictionary<string, object> CreateCustomerData(string code)
    {
        var data = new Dictionary<string, object>();
        data["CustomerCode"] = code;
        data["CustomerName"] = string.Empty;
        data["NewCust"] = true;

        return data;
    }
}
