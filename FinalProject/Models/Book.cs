using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FinalProject.Models
{
    public class Book
    {
        [Key]
        public int BookID { get; set; }

        [Required]
        [StringLength(200, ErrorMessage = "Title cannot be longer than 200 characters.")]
        public string Title { get; set; }

        [Required]
        [StringLength(13, ErrorMessage = "ISBN must be 13 characters long.", MinimumLength = 13)]
        public string ISBN { get; set; }

        [StringLength(1000, ErrorMessage = "Description cannot be more than 1000 characters.")]
        public string Description { get; set; }

        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 2)")]
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = true)]
        public decimal Price { get; set; }
        

        // Navigation Properties
        [Required]
        public int AuthorID { get; set; }

        // Add flag to use for soft delete
        [Display(Name = "Deleted")]
        public bool IsDeleted { get; set; }

        public Author Author { get; set; }  // Many-to-One with Authors
        public ICollection<OrderItem> OrderItems { get; set; }  // One-to-Many with OrderItems
        


    }
}
