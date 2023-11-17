using FinalProject.Data;
using FinalProject.Models;
using FinalProject.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Controllers
{
    public class OrderController : Controller
    {

        private readonly ApplicationDbContext _context;
        private readonly OrderService _orderService;

        public OrderController(ApplicationDbContext context, OrderService orderService )
        {
            _context = context;
            _orderService = orderService;
        }
        public IActionResult Index()
        {
            var orders = _context.Orders
            .Where(o => !o.IsDeleted)
            .Include(o => o.Customer)
            .ToList();
            return View(orders);
        }

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
        // GET: Order/Create
        public IActionResult Create()
        {
            // Populate ViewData or ViewBag with necessary data, like CustomerID options
            return View();
        }

        // POST: Order/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("CustomerID,OrderDate,TotalPrice")] Order order)
        {
            if (ModelState.IsValid)
            {
                _context.Add(order);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(order);
        }

        // GET: Order/Edit/5
        public IActionResult Edit(int id)
        {
            var order = _context.Orders.Find(id);
            if (order == null || order.IsDeleted)
            {
                return NotFound();
            }
            return View(order);
        }

        // POST: Order/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("OrderID,CustomerID,OrderDate,TotalPrice")] Order order)
        {
            if (id != order.OrderID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _context.Update(order);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(order);
        }
        // GET: Order/Delete/5
        public IActionResult Delete(int id)
        {
            var order = _context.Orders.Find(id);
            if (order == null || order.IsDeleted)
            {
                return NotFound();
            }
            return View(order);
        }

        // POST: Order/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
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

            // Continue with the confirmation process, like showing a confirmation view
            return View("ConfirmOrder");
        }


    }
}
