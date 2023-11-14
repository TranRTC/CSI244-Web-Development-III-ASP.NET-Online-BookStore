namespace Final_Project.Models
{
    public class Book
    {
        public int BookID { get; set; }
        public string Title { get; set; }
        public string ISBN { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        // Other properties...

        // Navigation Properties
        public int AuthorID { get; set; }
        public Author Author { get; set; }  // Many-to-One with Authors
        public ICollection<OrderItem> OrderItems { get; set; }  // One-to-Many with OrderItems
    }
}
