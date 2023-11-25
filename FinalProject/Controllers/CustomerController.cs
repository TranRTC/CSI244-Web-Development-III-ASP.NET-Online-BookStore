using FinalProject.Data;
using FinalProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Controllers
{
    public class CustomerController : Controller
    {

        private readonly ApplicationDbContext _context;
        public CustomerController(ApplicationDbContext context)
        {
            _context = context;
        }
        
        
        //===========================Index============================
        
        
        public IActionResult Index(bool showDeleted=false)
        {
            var customers = _context.Customers
                      .Where(c => c.IsDeleted == showDeleted)
                      .ToList();
            return View(customers);

            
        }


        //=============================Details=========================

        
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound(); // If no ID is provided, return a NotFound result.
            }

            var customer = _context.Customers
                .FirstOrDefault(c => c.CustomerID == id);

            if (customer == null)
            {
                return NotFound(); // If no customer is found with the provided ID, return NotFound.
            }

            return View(customer); // If a customer is found, pass the customer object to the view.
        }

        //=============================Create===========================

        
        public IActionResult Create()
        {
            return View();
        }
        //=============================Create Post=====================
        
        [HttpPost]
        
        public IActionResult Create([Bind("Name, Email, Address, Phone, IsDeleted, UserId")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                _context.Add(customer);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(customer);
        }


        //============================Edit================================
        
        public IActionResult Edit(int id)
        {
            var customer = _context.Customers.Find(id);
            if (customer == null)
            {
                return NotFound();
            }
            return View(customer);
        }

        
        [HttpPost]
        
        public IActionResult Edit(int id, [Bind("CustomerID, Name, Email, Address, Phone, IsDeleted, UserId")] Customer customer)
        {
            if (id != customer.CustomerID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _context.Update(customer);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(customer);
        }
        //========================Delete==========================
        
        public IActionResult Delete(int id)
        {
            var customer = _context.Customers.Find(id);
            if (customer == null)
            {
                return NotFound();
            }
            return View(customer);
        }

        // two action methods receive argument id
        // below for delete post
        
        [HttpPost, ActionName("Delete")]
        
        public IActionResult DeleteConfirmed(int id)
        {
            var customer = _context.Customers.Find(id);
            customer.IsDeleted = true;
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

    }
}
