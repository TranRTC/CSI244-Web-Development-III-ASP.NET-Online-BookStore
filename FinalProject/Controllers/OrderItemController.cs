using FinalProject.Data;
using Microsoft.AspNetCore.Mvc;
using FinalProject.Models;

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
        public IActionResult Index()
        {
            
            return View();
        }

        //============================Details=========================
        //=======Details Get============

        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            var OrderItem = _context.OrderItems.FirstOrDefault(oi => oi.OrderItemID == id);
            
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

        public IActionResult Create([Bind("OrderItemID, OrderID, BookID, Quantity")] OrderItem orderitem) {
        
            if(ModelState.IsValid)
            {
                _context.Add(orderitem);
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

        [HttpGet]

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
