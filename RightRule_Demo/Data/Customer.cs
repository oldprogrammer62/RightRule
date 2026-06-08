namespace RightRule_Demo.Data
{
    internal class Customer
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Code { get; set; }
        public required DateTime CreatedDate { get; set; }
    }
}
