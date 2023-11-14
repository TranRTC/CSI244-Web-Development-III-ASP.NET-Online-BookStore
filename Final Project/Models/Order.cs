namespace Final_Project.Models
{
    public class Order
    {

        public int OrderID { get; set; }
        public int CustomerID { get; set; }  // Foreign Key
        public DateTime OrderDate { get; set; }
        public decimal TotalPrice { get; set; }
        // Other properties...

        // Navigation Properties
        public Customer Customer { get; set; }  // Many-to-One with Customers
        public ICollection<OrderItem> OrderItems { get; set; }  // One-to-Many with

    }
}
