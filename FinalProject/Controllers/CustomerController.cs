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

        // GET: Customer/Details/5
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

        // GET: Customer/Create
        public IActionResult Create()
        {
            return View();
        }
        //=============================Create Post=====================
        // POST: Customer/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Name, Email, ...")] Customer customer)
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
        // GET: Customer/Edit/5
        public IActionResult Edit(int id)
        {
            var customer = _context.Customers.Find(id);
            if (customer == null)
            {
                return NotFound();
            }
            return View(customer);
        }

        // POST: Customer/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Name, Email, ...")] Customer customer)
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
        //=====================================Delete==========================
        // GET: Customer/Delete/5
        public IActionResult Delete(int id)
        {
            var customer = _context.Customers.Find(id);
            if (customer == null)
            {
                return NotFound();
            }
            return View(customer);
        }

        // POST: Customer/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var customer = _context.Customers.Find(id);
            customer.IsDeleted = true;
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }



    }
}
