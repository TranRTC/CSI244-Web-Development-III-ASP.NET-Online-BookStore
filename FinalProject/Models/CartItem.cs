using System.ComponentModel.DataAnnotations;

namespace FinalProject.Models
{
    public class CartItem
    {

        [Key]
        public int CartItemID { get; set; } // Primary key

        [Required]
        public int CartID { get; set; } // Foreign key to the Cart

        [Required]
        public int BookID { get; set; } // Foreign key to the Product

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
        public int Quantity { get; set; } // Quantity of the product

        // Navigation properties for Entity Framework relationships
        public virtual Cart Cart { get; set; }
        public virtual Book Book { get; set; }

        // Additional properties like price, added date, etc., can be included as needed
    }
}
