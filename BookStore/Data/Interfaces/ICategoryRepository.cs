using BookStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Data.Interfaces
{
    public interface ICategoryRepository
    {

        IEnumerable<Category> Categories { get; }
    }
}
