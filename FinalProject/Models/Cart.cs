namespace FinalProject.Models
{
    public class Cart
    {
        public int CartID { get; set; } // Unique identifier for the Cart
        public string UserId { get; set; } // User identifier (if applicable)      

        public DateTime CreatedDate { get; set; }
        public DateTime LastUpdatedDate { get; set; }

        // Collection of items in the cart
        public ICollection<CartItem> CartItems { get; set; }
        public Cart()
        {
            // Initialize the CartItems collection to prevent null reference issues
            CartItems = new List<CartItem>();

            // Set the creation and last updated dates to the current date and time
            CreatedDate = DateTime.Now;
            LastUpdatedDate = DateTime.Now;
        }

    }

}
