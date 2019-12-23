using BookStore.Data.Interfaces;
using BookStore.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Data.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly ApplicationDbContext _appDbContext;

        public BookRepository(ApplicationDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public IEnumerable<Book> Books => _appDbContext.Books.Include(c => c.Category);

        public IEnumerable<Book> PrefferedBooks => _appDbContext.Books.Where(p => p.IsPrefferedBook).Include(c => c.Category);

        public Book GetBookById(int bookId) => _appDbContext.Books.FirstOrDefault(p => p.ID == bookId);
    }
}
