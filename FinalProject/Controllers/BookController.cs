using FinalProject.Data;
using FinalProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Controllers
{
    
    public class BookController : Controller
    {

        private readonly ApplicationDbContext _context;
        public BookController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {

            var books = _context.Books
                        .Include(b => b.Author) // Include author information
                        .Where(b => !b.IsDeleted) // Filter out deleted books
                        .ToList();

            return View(books);
        }

        public IActionResult Details(int id)
        {
            var book = _context.Books
                .Include(b => b.Author)
                .FirstOrDefault(b => b.BookID == id);

            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // GET: Book/Edit/5
        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = _context.Books.Find(id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        [HttpPost]

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("BookID,Title,ISBN,Description,Price,AuthorID")] Book book)
        {
            // 1. ID Mismatch Check
            if (id != book.BookID)
            {
                return NotFound();
            }

            // 2. Model State Validation
            if (ModelState.IsValid)
            {
                try
                {
                    // 3. Updating the Book
                    _context.Update(book);
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    // 4. Concurrency Exception Handling
                    if (!BookExists(book.BookID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                // 5. Redirect on Success
                return RedirectToAction(nameof(Index));
            }

            // 6. Return View with Model
            return View(book);
        }


        // GET: Books/Delete/5
        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = _context.Books
                .FirstOrDefault(m => m.BookID == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // POST: Books/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var book = _context.Books.Find(id);
            if (book != null)
            {
                book.IsDeleted = true;  // Soft delete by setting the IsDeleted flag
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }



        // Defind method to check if book is exist
        private bool BookExists(int id)
        {
            return _context.Books.Any(e => e.BookID == id);
        }
    }
}
