namespace Final_Project.Models
{
    public class Customer
    {
        public int CustomerID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
       
        // Navigation Property
        public ICollection<Order> Orders { get; set; }  // One-to-Many with Orders
    }
}
