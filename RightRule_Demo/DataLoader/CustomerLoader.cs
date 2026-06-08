using RightRule_Demo.Data;

namespace RightRule_Demo.DataLoader
{
    internal class CustomerLoader
    {
        internal static Customer? Load(string code)
        {
            return LoadCustomers().FirstOrDefault(c => c.Code.Equals(code, StringComparison.CurrentCultureIgnoreCase));
        }

        private static List<Customer> LoadCustomers()
        {
            return new List<Customer>
            {
                new Customer()
                {
                    Id = 1,
                    Name = "Tom",
                    Code = "C001",
                    CreatedDate = DateTime.Now.AddDays(-10)
                },
                new Customer()
                {
                    Id = 1,
                    Name = "Dick",
                    Code = "C002",
                    CreatedDate = DateTime.Now.AddDays(-10)
                },
                new Customer()
                {
                    Id = 1,
                    Name = "Harry",
                    Code = "C003",
                    CreatedDate = DateTime.Now.AddDays(-10)
                }
            };
        }
    }
}
