using System.ComponentModel.DataAnnotations;

namespace FinalProject.Models
{
    public class Author
    {
        [Key]
        public int AuthorID { get; set; }

        [Required(ErrorMessage = "Author name is required.")]
        [StringLength(100, ErrorMessage = "Name cannot be longer than 100 characters.")]
        public string Name { get; set; }

        [StringLength(1000, ErrorMessage = "Biography cannot be more than 1000 characters.")]

        [Required(ErrorMessage ="Biography of Author is required")]
        public string Biography { get; set; }
        // Add flag to use for soft delete
        [Display(Name = "Deleted")]
        public bool IsDeleted { get; set; }

        // Navigation Property
        public ICollection<Book> Books { get; set; }  // One-to-Many with Books

    }
}
