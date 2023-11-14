namespace Final_Project.Models
{
    public class OrderItem
    {
        public int OrderItemID { get; set; }
        public int OrderID { get; set; }  // Foreign Key
        public int BookID { get; set; }   // Foreign Key
        public int Quantity { get; set; }
        // Other properties...

        // Navigation Properties
        public Order Order { get; set; }  // Many-to-One with Orders
        public Book Book { get; set; }    // Many-to-One with Books

    }
}
