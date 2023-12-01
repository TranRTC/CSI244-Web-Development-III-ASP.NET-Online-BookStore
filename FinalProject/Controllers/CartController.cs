using FinalProject.Data;
using FinalProject.Models;
using Microsoft.AspNetCore.Identity;
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
            // handle the case where the quantity is less than 1
            if (quantity < 1)
            {
                
                TempData["ErrorMessage"] = "Quantity must be at least 1.";
                return RedirectToAction("Index");
            }

            var userId = GetUserId(); // Retrieve the current user's ID. This method is at the bottom
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

        //=====================================Get UserId======================================
        //Method to get UserId to server for the other methods need authentication

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


        

        public IActionResult Checkout()
        {
          

            // if authenticated find userId
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                // TempData is used to pass the message to the view
                TempData["ErrorMessage"] = "User not identified. Please log in.";

                //====If no direct to login page=========
                return LocalRedirect("~/Identity/Account/Login");
            }
            // get Name & Email of authenticated user
            var userName = User.FindFirstValue(ClaimTypes.Name);
            var userEmail = User.FindFirstValue(ClaimTypes.Email);


            // Synchronously get or create a customer record
            //if there is userId find customer from Customers table use userID
            var customer = _context.Customers.FirstOrDefault(c => c.UserId == userId);
            // if no customer found
            if (customer == null)
            {
                
                // Create new Customer from the login User by use Login Name, Email, userID
                customer = new Customer { UserId = userId, Name=userName, Email=userEmail };
                _context.Customers.Add(customer);
                _context.SaveChanges(); // Synchronous save
            }

             

            // create "cart" to hold Cart items belong to the authenticated user

            var cart = _context.Carts
                               .Include(c => c.CartItems)
                               .ThenInclude(ci => ci.Book)
                               .FirstOrDefault(c => c.UserId == userId);

            // if cart have nothing

            if (cart == null || !cart.CartItems.Any())
            {
                // inform the cart is empty
                TempData["ErrorMessage"] = "Your cart is empty.";

                //direct to index page of Book controller to see catalogue
                return RedirectToAction("Index", "Book");
            }

            // If cart have something/not null
            // create new Order
            var order = new Order
            {
                // CustomerID of new Order were get from new customer above
                CustomerID = customer.CustomerID,
                // Initialize other properties of the order...
                OrderItems = new List<OrderItem>(),
                // adding time for the Order it is the time of creating order
                OrderDate = DateTime.Now
            };

            // Add items from cart to order
            foreach (var cartItem in cart.CartItems)
            {
                var orderItem = new OrderItem
                {   // orderItemID handle by sql
                    
                    BookID = cartItem.BookID,
                    Quantity = cartItem.Quantity
                    // Initialize other properties of the order item...
                };
                order.OrderItems.Add(orderItem);
            }

            // add new order to database
            _context.Orders.Add(order);
            _context.SaveChanges();

            // clear the cart
            _context.CartItems.RemoveRange(cart.CartItems);
            _context.SaveChanges();

            return RedirectToAction("Details", "Order", new { id = order.OrderID });
        }
        



    }
}
