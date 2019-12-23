using BookStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.ViewModels
{
    public class HomeViewModel
    {
        public IEnumerable<Book> PrefferedBooks { get; set; }
        public IEnumerable<Slide> Slide { get; set; }
    }
}
