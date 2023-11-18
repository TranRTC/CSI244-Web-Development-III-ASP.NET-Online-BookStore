using FinalProject.Data;
using FinalProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Security.Principal;

namespace FinalProject.Controllers
{
    public class CartController : Controller
    {

        private readonly ApplicationDbContext _context;

        public CartController(ApplicationDbContext context)
        {
            _context = context;
        }

        
        public ActionResult AddToCart(int bookId, int quantity = 1)
        { // Validate the quantity
            //if (quantity < 1)
            //{
            //    // Optionally return an error message or set a default value
            //    TempData["ErrorMessage"] = "Invalid quantity. Quantity must be at least 1.";
            //    return RedirectToAction("Index");
            //}

            // Retrieve the current user's ID

            var userId = GetUserId();
            if (string.IsNullOrEmpty(userId))
            {
                // Handle unauthenticated user scenario
                // Redirect to login or show a suitable message
                TempData["ErrorMessage"] = "User not identified. Please log in.";
                return LocalRedirect("~/Identity/Account/Login");
            }

            // Check if the book exists

            var book = _context.Books.Find(bookId);
            if (book == null)
            {
                // Handle non-existent book scenario
                TempData["ErrorMessage"] = "Book not found.";
                return RedirectToAction("Index", "Book");
            }

            // Retrieve or create the cart
            var cart = _context.Carts.FirstOrDefault(c => c.UserId == userId);
            if (cart == null)
            {
                cart = new Cart { UserId = userId };
                _context.Carts.Add(cart);
            }

            // Add or update the cart item
            var cartItem = cart.CartItems.FirstOrDefault(ci => ci.BookID == bookId);
            if (cartItem != null)
            {
                cartItem.Quantity += quantity;
            }
            else
            {
                cartItem = new CartItem { BookID = bookId, Quantity = quantity };
                cart.CartItems.Add(cartItem);
            }

            // Save changes with error handling
            try
            {
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                // Log the exception and handle it
                // For example, return an error message to the view
                TempData["ErrorMessage"] = "An error occurred while updating the cart.";
                return RedirectToAction("Index");
            }

            // Redirect to the cart view on success
            TempData["SuccessMessage"] = "Item added to cart successfully.";
            return RedirectToAction("Index");
        }


        public IActionResult Index()
        {
            var userId = GetUserId(); // Method to retrieve the current user's ID
            var cart = _context.Carts
                        .Include(c => c.CartItems)
                        .ThenInclude(ci => ci.Book) // Assuming each CartItem has a Product
                        .FirstOrDefault(c => c.UserId == userId);

            if (cart == null)
            {
                // Handle the case where the cart is not found
                // This could involve creating a new cart, showing an empty cart message, etc.
                return View(new Cart());
            }

            return View(cart);
        }



        public IActionResult RemoveFromCart(int bookId)
        {
            var userId = GetUserId(); // Retrieve the current user's ID
            var cart = _context.Carts.Include(c => c.CartItems)
                        .FirstOrDefault(c => c.UserId == userId);

            if (cart != null)
            {
                var cartItem = cart.CartItems.FirstOrDefault(ci => ci.BookID == bookId);
                if (cartItem != null)
                {
                    cart.CartItems.Remove(cartItem);
                    _context.SaveChanges();

                    // Optionally, add a success message to TempData or ViewBag
                    TempData["SuccessMessage"] = "Item removed from cart successfully.";
                }
                else
                {
                    // Optionally, handle the case where the item is not found in the cart
                    TempData["ErrorMessage"] = "Item not found in cart.";
                }
            }
            else
            {
                // Optionally, handle the case where the cart is not found
                TempData["ErrorMessage"] = "Cart not found.";
            }

            return RedirectToAction("Index"); // Redirect to the cart view
        }


        

          public IActionResult UpdateCartItem(int bookId, int quantity)
        {
            if (quantity < 1)
            {
                // Optionally handle the case where the quantity is less than 1
                TempData["ErrorMessage"] = "Quantity must be at least 1.";
                return RedirectToAction("Index");
            }

            var userId = GetUserId(); // Retrieve the current user's ID
            var cart = _context.Carts.Include(c => c.CartItems)
                        .FirstOrDefault(c => c.UserId == userId);

            if (cart != null)
            {
                var cartItem = cart.CartItems.FirstOrDefault(ci => ci.BookID == bookId);
                if (cartItem != null)
                {
                    cartItem.Quantity = quantity;
                    _context.SaveChanges();

                    // Optionally, add a success message
                    TempData["SuccessMessage"] = "Cart updated successfully.";
                }
                else
                {
                    // Optionally, handle the case where the cart item is not found
                    TempData["ErrorMessage"] = "Item not found in cart.";
                }
            }
            else
            {
                // Optionally, handle the case where the cart is not found
                TempData["ErrorMessage"] = "Cart not found.";
            }

            return RedirectToAction("Index"); // Redirect back to the cart view
        }
      

        private string GetUserId()
        {
            if (User.Identity.IsAuthenticated)
            {
                return User.FindFirstValue(ClaimTypes.NameIdentifier);
            }
            else
            {
                // Handle the scenario where the user is not authenticated
                // You might return a null or empty string, or handle it some other way
                return null;
            }
        }


        private int GenerateNewCartId()
        {

            // Logic to generate a unique identifier for the cart

            var newCart = new Cart();
            // Initialize any required fields of the Cart, if necessary

            _context.Carts.Add(newCart);
            _context.SaveChanges();

            return newCart.CartID; // Return the auto-generated CartId
        }

        /*
         
        */
        public ActionResult Checkout()
        {
            // Retrieve the current user's ID (assuming UserId is a string; adjust if it's a different type)
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                // Handle unauthenticated user scenario
                TempData["ErrorMessage"] = "Please log in to proceed.";
                return LocalRedirect("~/Identity/Account/Login"); // Adjust as needed based on login route
            }

            // Fetch the cart for the current user
            var cart = _context.Carts.Include(c => c.CartItems)
                         .ThenInclude(ci => ci.Book) // Assuming CartItem has a navigation property to Book
                         .FirstOrDefault(c => c.UserId == userId);

            if (cart == null || !cart.CartItems.Any())
            {
                // Handle the case where there is no cart or the cart is empty
                TempData["ErrorMessage"] = "Your cart is empty.";
                return RedirectToAction("Index", "Cart");
            }
            //===================================

            //var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var customer = _context.Customers.FirstOrDefault(c => c.UserId == userId);

            if (customer == null)
            {
                // Create a customer record if not found
                customer = new Customer { UserId = userId };
                _context.Customers.Add(customer);
                _context.SaveChanges();
            }


            //==============================


            // Create a new order
            var order = new Order
            {
                CustomerID = customer.CustomerID, // Replace with appropriate logic to get CustomerID from UserId
                OrderDate = DateTime.Now,
                TotalPrice = cart.CartItems.Sum(ci => ci.Quantity * ci.Book.Price),
                IsConfirmed = false,
                IsDeleted = false,
                OrderItems = cart.CartItems.Select(ci => new OrderItem
                {
                    BookID = ci.BookID,
                    Quantity = ci.Quantity,
                    // Set other properties as needed
                }).ToList()
            };

            // Add and save the order
            _context.Orders.Add(order);
            _context.SaveChanges();

            // Optional: Clear the cart after successful order creation
            // ...

            return RedirectToAction("OrderConfirmation", new { orderId = order.OrderID });
        }

       

        /*
        private int GetCustomerId()
        {
            if (User.Identity.IsAuthenticated)
            {
                // Assuming the user's ID is stored as the NameIdentifier claim
                // and that it's an integer value. Adjust the parsing as needed.
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (int.TryParse(userId, out int customerId))
                {
                    return customerId;
                }
            }

            // Handle the scenario where the user is not authenticated or the ID is not found.
            // This might involve throwing an exception, returning a default value, or redirecting to a login page.
            throw new InvalidOperationException("User is not authenticated or customer ID is not available.");
        }

        */

    }
}
