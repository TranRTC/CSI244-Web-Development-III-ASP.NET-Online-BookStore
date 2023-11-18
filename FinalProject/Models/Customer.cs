using System.ComponentModel.DataAnnotations;

namespace FinalProject.Models


{ 

    public class Customer
    {
        [Key]
        public int CustomerID { get; set; }

        //[Required(ErrorMessage = "Name is required.")]
        [StringLength(100, ErrorMessage = "Name cannot be longer than 100 characters.")]
        public string? Name { get; set; }

        //[Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid Email Address.")]
        [StringLength(100, ErrorMessage = "Email cannot be longer than 100 characters.")]
        public string? Email { get; set; }

        [StringLength(200, ErrorMessage = "Address cannot be longer than 200 characters.")]
        public string? Address { get; set; }

        [Phone(ErrorMessage = "Invalid Phone Number.")]
        [StringLength(20, ErrorMessage = "Phone number cannot be longer than 20 characters.")]
        public string? Phone { get; set; }

        [Display(Name = "Deleted")]
        public bool IsDeleted { get; set; }

        // The UserId property to link the Customer with an Identity User
        //[Required]
        // Foreign key for ApplicationUser
        public string UserId { get; set; }



        // Navigation Property to the Identity User
        // public virtual ApplicationUser User { get; set; }

        // Navigation Property for Orders
        public ICollection<Order> Orders { get; set; }
    }

    /*
    public class Customer
    {
        [Key]
        public int CustomerID { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [StringLength(100, ErrorMessage = "Name cannot be longer than 100 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid Email Address.")]
        [StringLength(100, ErrorMessage = "Email cannot be longer than 100 characters.")]
        public string Email { get; set; }

        [StringLength(200, ErrorMessage = "Address cannot be longer than 200 characters.")]
        public string Address { get; set; }

        [Phone(ErrorMessage = "Invalid Phone Number.")]
        [StringLength(20, ErrorMessage = "Phone number cannot be longer than 20 characters.")]
        public string Phone { get; set; }

        // Add flag to use for soft delete
        [Display(Name = "Deleted")]
        public bool IsDeleted { get; set; }

        // Navigation Property
        public ICollection<Order> Orders { get; set; }  // One-to-Many with Orders
    }

     */
}
