using FinalProject.Data;
using FinalProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Controllers
{
    
    public class BookController : Controller
    {
        //=================================Inject Service===========================

        private readonly ApplicationDbContext _context;
        public BookController(ApplicationDbContext context)
        {
            _context = context;
        }

        //===========================Index============================================

        public IActionResult Index(string searchTitle, string searchAuthor)
        {
            var booksQuery = _context.Books
                             .Include(b => b.Author) // Include author information
                             .Where(b => !b.IsDeleted); // Filter out deleted books

            if (!string.IsNullOrEmpty(searchTitle))
            {
                booksQuery = booksQuery.Where(b => b.Title.Contains(searchTitle));
            }

            if (!string.IsNullOrEmpty(searchAuthor))
            {
                booksQuery = booksQuery.Where(b => b.Author.Name.Contains(searchAuthor));
            }

            var books = booksQuery.ToList();

            return View(books);
        }

        //==================================Create=======================================

        //============== for HttpGet ==============

        [Authorize(Roles = "Admin, Manager")]
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        //================= for HttpPost ============

        [Authorize(Roles = "Admin, Manager")]
        [HttpPost]

        public IActionResult Create([Bind("Title, ISBN, Description, Price, AuthorID")] Book book)
        {
            // First, check if the model state is valid
            if (ModelState.IsValid)
            {
                // Check if the provided AuthorID exists in the database and not (soft) deleted
                if (!_context.Authors.Any(a => a.AuthorID == book.AuthorID && ! a.IsDeleted))
                {
                    // If AuthorID does not exist, add a model error
                    ModelState.AddModelError("AuthorID", "Invalid Author ID.");
                }
                else
                {
                    // If AuthorID is valid, proceed to add the book and save changes
                    _context.Add(book);
                    _context.SaveChanges(); 
                    return RedirectToAction("Index");
                }
            }

            // If we got this far, something failed; redisplay the form
            return View(book);
        }


        //==================================Details======================================

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


        //=====================================Edit======================================

        //============Edit for HttpGet======================

        [Authorize(Roles = "Admin, Manager")]
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



        //=================Edit for HttpGet=================

        [Authorize(Roles = "Admin, Manager")]

        [HttpPost]
        
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


                //3. Check if the provided AuthorID exists in the database and not (soft) deleted
                if (!_context.Authors.Any(a => a.AuthorID == book.AuthorID && !a.IsDeleted))
                {
                    //4. If AuthorID does not exist, add a model error
                    ModelState.AddModelError("AuthorID", "Invalid Author ID.");
                }
                else
                {
                    // 5.If AuthorID is valid, proceed to update
                    _context.Update(book);
                    _context.SaveChanges();
                    return RedirectToAction("Index");
                }               
            }

            // 6. Return View with Model
            return View(book);
        }
        //=======================================Delete========================================

        //==============Delete for HttpGet=================
        [Authorize(Roles = "Admin, Manager")]
        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = _context.Books.Include(b => b.Author)
                .FirstOrDefault(m => m.BookID == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        //===========Delete for HttpPost==============
        [Authorize(Roles = "Admin, Manager")]
        [HttpPost]
        
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

        //==============BookExists========================

        // Defind method to check if book is exist
        private bool BookExists(int id)
        {
            return _context.Books.Any(e => e.BookID == id);
        }
    }
}
