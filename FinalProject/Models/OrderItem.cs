using System.ComponentModel.DataAnnotations;

namespace FinalProject.Models
{
    public class OrderItem
    {
        [Key]

        public int OrderItemID { get; set; }

        [Required]
        public int OrderID { get; set; }  // Foreign Key

        [Required]
        public int BookID { get; set; }   // Foreign Key

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
        public int Quantity { get; set; }
        
        public bool IsDeleted { get; set; } // used for soft delete

        // Navigation Properties
        public Order Order { get; set; }  // Many-to-One with Orders
        public Book Book { get; set; }    // Many-to-One with Books
    }
}
