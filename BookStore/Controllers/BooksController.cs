using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookStore.Data;
using BookStore.Data.Interfaces;
using BookStore.Models;
using BookStore.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Controllers
{
    public class BooksController : Controller
    {
        private readonly IBookRepository _bookRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ApplicationDbContext _context;

        public BooksController(IBookRepository bookRepository, ICategoryRepository categoryRepository, ApplicationDbContext context)
        {
            _bookRepository = bookRepository;
            _categoryRepository = categoryRepository;
            _context = context;

        }

        [Route("/Books/List/Detail/{id}")]
        public async Task<IActionResult> Detail(int? id)
        {
            var book =await _context.Books
                .Include(b => b.Category)
                .FirstOrDefaultAsync(m => m.ID == id);

            return View(book);
        }

        public ViewResult List(string category)
        {
            string _category = category;
            IEnumerable<Book> books;
            

            string currentCategory = string.Empty;

            if (string.IsNullOrEmpty(category))
            {
                books = _bookRepository.Books.OrderBy(p => p.BookName);
                currentCategory = "All books";
            }
            else
            {
                books = null;
                foreach (Category item in _categoryRepository.Categories)
                {
                    if (string.Equals(item.CategoryName, _category, StringComparison.OrdinalIgnoreCase))
                    {
                        books = _bookRepository.Books.Where(p => p.Category.CategoryName.Equals(item.CategoryName)).OrderBy(p => p.BookName);
                        break;
                    }

                }
                //if (string.Equals("Fantasy", _category, StringComparison.OrdinalIgnoreCase))
                //    books = _bookRepository.Books.Where(p => p.Category.CategoryName.Equals("Fantasy")).OrderBy(p => p.BookName);
                //else if (string.Equals("Biography", _category, StringComparison.OrdinalIgnoreCase))
                //    books = _bookRepository.Books.Where(p => p.Category.CategoryName.Equals("Biography")).OrderBy(p => p.BookName);
                //else 
                //    books = _bookRepository.Books.Where(p => p.Category.CategoryName.Equals("Crime")).OrderBy(p => p.BookName);

                currentCategory = _category;
            }
            return View(new BookListViewModel
            {
                Books = books,
                CurrentCategory = currentCategory
            });


        }

        public ViewResult Search(string searchString)
        {
            string _searchString = searchString;
            IEnumerable<Book> books;
            string currentCategory = string.Empty;

            if (string.IsNullOrEmpty(_searchString))
            {
                books = _bookRepository.Books.OrderBy(p => p.ID);
            }
            else
            {
                books = _bookRepository.Books.Where(p => p.BookName.ToLower().Contains(_searchString.ToLower()));
            }

            return View("~/Views/Books/List.cshtml", new BookListViewModel { Books = books, CurrentCategory = "All Books" });
        }

    }
}