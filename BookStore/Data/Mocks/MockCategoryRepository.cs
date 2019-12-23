using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookStore.Data.Interfaces;
using BookStore.Models;

namespace BookStore.Data.Mocks
{
    public class MockCategoryRepository:ICategoryRepository
    {
        public IEnumerable<Category> Categories
        {
            get
            {
                return new List<Category>
                     {
                         new Category { CategoryName = "Fantasy" },
                         new Category { CategoryName = "Biography"},
                         new Category { CategoryName = "Action and Adventure"},
                         new Category { CategoryName = "Comics"},
                         new Category { CategoryName = "Horror"},
                         new Category { CategoryName = "Romance and Love"},
                         new Category { CategoryName = "Crime"},
                     };
            }
        }
    }
}
