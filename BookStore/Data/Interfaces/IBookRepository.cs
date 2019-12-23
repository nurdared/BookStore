using BookStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Data.Interfaces
{
    public interface IBookRepository
    {
        IEnumerable<Book> Books { get; }

        IEnumerable<Book> PrefferedBooks { get; }

        Book GetBookById(int bookid);
    }
}
