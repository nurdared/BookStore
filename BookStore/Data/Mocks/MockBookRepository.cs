using BookStore.Data.Interfaces;
using BookStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Data.Mocks
{
    public class MockBookRepository:IBookRepository
    {
        private readonly ICategoryRepository _categoryRepository = new MockCategoryRepository();

        public IEnumerable<Book> Books {
            get
            {
                return new List<Book>
                {
                    new Book {
                                BookName = "Wayne Rooney: My Decade in the Premier League",
                                BookAuthor = "Wayne Rooney",
                                BookYear = "2013",
                                BookDesc = "My Decade in the Premier League is Wayne's first hand account of his 10 years playing at the highest level in English football - and for the biggest club in the world.",
                                ImageUrl = "https://s3-ap-southeast-1.amazonaws.com/mph-images/9780007242641_b.jpg",
                                IsPrefferedBook = true,
                                BookPrice = 20,
                                BookInStock = true,
                                Category = _categoryRepository.Categories.First(),
                    },
                    new Book {
                                BookName = "Fergie: The Greatest The Biography Of Sir Alex Ferguson",
                                BookAuthor = "Frank Worrall",
                                BookYear = "2014",
                                BookDesc = "The story of Sir Alex Ferguson is a true rags-to-riches fairytale. When he announced he was to step down as manager of the world's biggest football club after 27 years in charge, he signed off in style with an honours list that confirmed him as the most successful manager in British history.",
                                
                                ImageUrl = "https://s3-ap-southeast-1.amazonaws.com/mph-images/9781782197300_b.jpg",
                                IsPrefferedBook = false,
                                BookPrice = 20,
                                BookInStock = true,
                                Category = _categoryRepository.Categories.First(),

                    },
                    new Book {
                                BookName = "Harry Potter and the Goblet of Fire",
                                BookAuthor = "J.K Rowling",
                                BookYear = "2018",
                                BookDesc = "When the Quidditch World Cup is disrupted by Voldemort's rampaging supporters alongside the resurrection of the terrifying Dark Mark, it is obvious to Harry Potter that, far from weakening, Voldemort is getting stronger. ",
                                
                                ImageUrl = "https://mphimages.s3.amazonaws.com/f9f0cd0eaf8343dd8bf4ea3fe9b0a56e.jpg",
                                IsPrefferedBook = true,
                                BookPrice = 20,
                                BookInStock = true,
                                Category = _categoryRepository.Categories.Last(),
                    },
                    new Book {
                                BookName = "The Fever Code ",
                                BookAuthor = "James Dashner",
                                BookYear = "2017",
                                BookDesc = "Once there was a world’s end.The forests burned, the lakes and rivers dried up",
                                
                                ImageUrl = "https://mphimages.s3.amazonaws.com/2f4367b465524edbbc1d403b41585b93.jpg",
                                IsPrefferedBook = false,
                                BookPrice = 20,
                                BookInStock = true,
                                Category = _categoryRepository.Categories.Last(),
                    },
                };

            }
        }
        public IEnumerable<Book> PrefferedBooks { get; }

        public Book GetBookById(int bookid)
        {
            throw new NotImplementedException();
        }
    }
}
