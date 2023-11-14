namespace Final_Project.Models
{
    public class Author
    {
        public int AuthorID { get; set; }
        public string Name { get; set; }
        public string Biography { get; set; }
        // Other properties...

        // Navigation Property
        public ICollection<Book> Books { get; set; }  // One-to-Many with Books

    }
}
