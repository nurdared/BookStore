using BookStore.Data.Interfaces;
using BookStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Data.Repositories
{
    public class SlideRepository:ISlideRepository
    {
        private readonly ApplicationDbContext _appDbContext;
        public SlideRepository(ApplicationDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public IEnumerable<Slide> Slide => _appDbContext.Slide;
    }
}
