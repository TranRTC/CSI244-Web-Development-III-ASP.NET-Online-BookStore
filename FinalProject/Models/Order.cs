using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinalProject.Models
{
    public class Order
    {
        [Key]
        public int OrderID { get; set; }

        [Required]
        public int CustomerID { get; set; }  // Foreign Key

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime OrderDate { get; set; }

        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 2)")]
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = true)]
        public decimal TotalPrice { get; set; }

        // Add flag to use for soft delete
        [Display(Name = "Deleted")]
        public bool IsDeleted { get; set; }

        // This property is added to hold order status
        public bool IsConfirmed { get; set; }

        // Navigation Properties
        public Customer Customer { get; set; }  // Many-to-One with Customers
        public ICollection<OrderItem> OrderItems { get; set; }  // One-to-Many with

        

    }
}
