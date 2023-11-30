using FinalProject.Data;
using FinalProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinalProject.Controllers
{
    [Authorize(Roles = "Admin, Manager")]
    public class AuthorController : Controller
    {
        //============================Service============================
        
        private readonly ApplicationDbContext _context;
        public AuthorController(ApplicationDbContext context)
        {
            _context = context;
        }
        //==========================Index================================
        [Authorize]
        public IActionResult Index()

        {
            var authors = _context.Authors.Where(b => b.IsDeleted != true).ToList();

            return View(authors);
             
        }
        //==========================Details=============================

        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var author = _context.Authors
                .FirstOrDefault(m => m.AuthorID == id);
            if (author == null)
            {
                return NotFound();
            }

            return View(author);
        }

        //=======================================Create==========================

        //================Create Get================
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        //===============Create Post===============

        [HttpPost]
        
        public IActionResult Create([Bind("Name,Biography,IsDeleted")] Author author)
        {
            if (ModelState.IsValid)
            {
                _context.Add(author);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(author);
        }

        //===================================Edit======================================

        // ====================Edit Get===================
        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var author = _context.Authors.Find(id);
            if (author == null)
            {
                return NotFound();
            }
            return View(author);
        }

        //=================Edit Post=======================
        [HttpPost]
        
        public IActionResult Edit(int id, [Bind("AuthorID,Name,Biography")] Author author)
        {
            if (id != author.AuthorID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _context.Update(author);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(author);
        }

        //================================Delete======================================
        //==============Delete Get=============
        
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var author = _context.Authors
                .FirstOrDefault(m => m.AuthorID == id);
            if (author == null)
            {
                return NotFound();
            }

            return View(author);
        }


        //============Delete Post=========

        
        [HttpPost, ActionName("Delete")]
        
        public IActionResult DeleteConfirmed(int id)
        {
            var author = _context.Authors.Find(id);
            if (author != null)
            {
                author.IsDeleted = true;
                _context.SaveChanges();
            }

            //if need "hard delete" use this code:
            //_context.Authors.Remove(author);           

            return RedirectToAction(nameof(Index));
        }


       
    }
}
