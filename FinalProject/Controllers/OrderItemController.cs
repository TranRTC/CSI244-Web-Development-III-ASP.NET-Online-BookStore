using FinalProject.Data;
using Microsoft.AspNetCore.Mvc;
using FinalProject.Models;
using Microsoft.EntityFrameworkCore;
namespace FinalProject.Controllers
{
    
    
    public class OrderItemController : Controller

    {
        //=======================Service============================

        private readonly ApplicationDbContext _context;

        public OrderItemController(ApplicationDbContext context)
        {
            _context = context;
        }

        //==========================Index============================
        // for a siplify interface. In reality the OrderItemId can be found base of Custoner and Order or date
        // depend on the speficic rquirement
        public IActionResult Index()
        {
            var OrderItems = _context.OrderItems.Where(oi => oi.IsDeleted == false).ToList();
            return View(OrderItems);
        }

        //============================Details=========================
        //=======Details Get============

        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            var OrderItem = _context.OrderItems
                .Include(oi => oi.Order)
                .Include(oi => oi.Book)
                .FirstOrDefault(oi => oi.OrderItemID == id);
            
            if (OrderItem == null)
            {
                return NotFound();
            }
            return View(OrderItem);
        }

        //============================Create==========================
        //=======Create Get=============
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        //==========Create Post=========

        public IActionResult Create([Bind("OrderID, BookID, Quantity")] OrderItem orderitem) {
        
            if(ModelState.IsValid)
            {
                // Note: Check if does not exist an entry in Order table which have OrderID = 
                // OrderID of the passing object &&
                // Also check in Book table if does not exist an entry where BookID == BookID of the passing object
                // Also check the order is not soft deleted
                if (!_context.Orders.Any(o => o.OrderID == orderitem.OrderID && !o.IsDeleted))                    
                    
                    // Error appear below the OrderID input field when it does not exist
                {
                    ModelState.AddModelError("OrderID", "Invalid Order ID.");
                    return View(orderitem);
                }
                    // Error appear below the BookID input field when it does not exist
                if (!_context.Books.Any(b => b.BookID == orderitem.BookID && !b.IsDeleted))
                {
                    ModelState.AddModelError("BookID", "Invalid Book ID.");
                    return View(orderitem);
                }
                // come to this far mean OrderID & BookID are checked

                // Add new orderitem
                _context.Add(orderitem);
                // save the change, check SQL OrderItem table : Select * from OrderItems
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(orderitem);
        
        }

        //==============================Edit=============================================

        // ========Edit Get============
        [HttpGet]
        public IActionResult Edit(int? id)
        {
            var OrderItem = _context.OrderItems.Find(id);
            if (OrderItem == null)
            {
                return NotFound();
            }
            return View(OrderItem);

        }

        [HttpPost]

        //===========Edit Post=============
        public IActionResult Edit([Bind("OrderItemID, OrderID, BookID, Quantity")] OrderItem orderitem)
        {
            if(ModelState.IsValid)
            {
                _context.Update(orderitem);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(orderitem);
        }

        //================================Delete==============================
        //==========Delete Get==========
        [HttpGet]
        public IActionResult Delete(int? id)
        {
            var orderitem = _context.OrderItems.Find(id);
            if (orderitem == null)
            {
                return NotFound();
            }
            return View(orderitem);
        }

        //============Delete Post========
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfimed(int id)
        {
            var orderitem = _context.OrderItems.Find(id);
            if (orderitem == null)
            {
                return NotFound(id);
            }
            orderitem.IsDeleted = true;
            _context.SaveChanges();
            return RedirectToAction("Index");

        }
    }
}
