using FinalProject.Data;
using FinalProject.Models;
using FinalProject.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Controllers
{
    [Authorize(Roles = "Admin")]
    public class OrderController : Controller
    {

        private readonly ApplicationDbContext _context;
        private readonly OrderService _orderService;

        public OrderController(ApplicationDbContext context, OrderService orderService )
        {
            _context = context;
            _orderService = orderService;// orderService handle for price & discount
        }

        //==========================================Index=========================================
        public IActionResult Index()
        {
            var orders = _context.Orders
            .Where(o => !o.IsDeleted)
            .Include(o => o.Customer)
            .ToList();
            return View(orders);
        }

        //=========================================Details==========================================

        [HttpGet]
        public IActionResult Details(int id)
        {
            var order = _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Book)
                .FirstOrDefault(o => o.OrderID == id && !o.IsDeleted);

            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }
        

        //===========================================Create=========================================

        //======================Create for HttpGet===============
        public IActionResult Create()
        {
            // Populate ViewData or ViewBag with necessary data, like CustomerID options
            return View();
        }

        //=====================Create for HttpPost================
        [HttpPost]
        
        public IActionResult Create([Bind("CustomerID,OrderDate,TotalPrice")] Order order)
        {
            if (ModelState.IsValid)
            {
                

                //3. Check if the provided CustomerID exists in the database and not (soft) deleted
                if (!_context.Orders.Any(o => o.CustomerID == order.CustomerID && !o.IsDeleted))
                {
                    //4. If AuthorID does not exist, add a model error
                    ModelState.AddModelError("CustomerID", "Invalid Customer ID.");
                }
                else
                {
                    // 5.If AuthorID is valid, proceed to update
                    _context.Update(order);
                    _context.SaveChanges();
                    return RedirectToAction("Index");
                }

            }
            return View(order);
        }

        //=======================================Edit===============================================

        //===========Edit for HttpGet===============
        public IActionResult Edit(int id)
        {
            var order = _context.Orders.Find(id);
            if (order == null || order.IsDeleted)
            {
                return NotFound();
            }
            return View(order);
        }

        //============Edit for HttpPost===============
        [HttpPost]
        
        public IActionResult Edit(int id, [Bind("OrderID,CustomerID,OrderDate,TotalPrice")] Order order)
        {
            //1.verify the OrderID prevent from being changed in URL
            if (id != order.OrderID)
            {
                return NotFound();
            }

            //2. Validate the model
            if (ModelState.IsValid)
            {

                //3. Check if the provided CustomerID exists in the database and not (soft) deleted
                if (!_context.Orders.Any(o => o.CustomerID == order.CustomerID && !o.IsDeleted))
                {
                    //4. If AuthorID does not exist, add a model error
                    ModelState.AddModelError("CustomerID", "Invalid Customer ID.");
                }
                else
                {
                    // 5.If AuthorID is valid, proceed to update
                    _context.Update(order);
                    _context.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            return View(order);
        }
        

        //===========================================Delete==============================================
        //===============Delete for HttpGet=====================
        public IActionResult Delete(int id)
        {
            var order = _context.Orders.Find(id);
            if (order == null || order.IsDeleted)
            {
                return NotFound();
            }
            return View(order);
        }

        //===============Delete for HttpPost=====================
        [HttpPost, ActionName("Delete")]
        
        public IActionResult DeleteConfirmed(int id)
        {
            var order = _context.Orders.Find(id);
            if (order != null)
            {
                order.IsDeleted = true;
                _context.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }

        //============================Confirm Order============================================================


        [HttpPost]

        public IActionResult ConfirmOrder(int orderId)
        {

            var order = _orderService.GetOrderById(orderId);
            if (order == null)
            {
                return NotFound(); // Or handle the absence of the order appropriately
            }

            var totalWithTax = _orderService.CalculateOrderTotal(orderId);
            order.TotalPrice = totalWithTax; // Update the TotalPrice

            order.IsConfirmed = true; // Set the order as confirmed
            _context.Update(order); // Assuming _context is your DbContext
            _context.SaveChanges();

            // Continue with the confirmation process
            // buy direct to the details view but show the confirmation status
            // using conditional rendering bool variable IsConfirmed
            
            return RedirectToAction("Details", new { id = orderId });            

        }


        [HttpPost]
        public IActionResult AddBookToOrder(int bookId, int quantity = 1)
        {
            _orderService.AddBookToOrder(bookId, quantity);
            return RedirectToAction("Cart");
        }

    }
}
