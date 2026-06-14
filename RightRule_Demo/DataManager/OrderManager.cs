namespace RightRule_Demo.DataManager
{
    internal class OrderManager
    {
        internal static decimal GetOrderValue()
        {
            Console.WriteLine("Order Values: ");
            Console.WriteLine("\t< 100.00 is considered Low Value");
            Console.WriteLine("\t>= 100.00 is considered High Value");

            Console.WriteLine("Please enter the Order Value...");
            var orderValueInput = Console.ReadLine();
            if (string.IsNullOrEmpty(orderValueInput) || !decimal.TryParse(orderValueInput, out var orderValue))
            {
                Console.WriteLine("Invalid Order Value. Defaulting to 0.");
                return 0;
            }
            Console.WriteLine();
            return orderValue;
        }
    }
}
