using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BookStore.Models;
using BookStore.Data.Interfaces;
using BookStore.ViewModels;
using BookStore.Data;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Controllers
{
    public class HomeController : Controller
    {
        private readonly IBookRepository _bookRepository;
        private readonly ISlideRepository _slideRepository;
        private readonly ApplicationDbContext _context;

        public HomeController(IBookRepository bookRepository, ISlideRepository sliderepository, ApplicationDbContext context)
        {
            _bookRepository = bookRepository;
            _slideRepository = sliderepository;
            _context = context;
        }

        public ViewResult Index()
        {
            var homeViewModel = new HomeViewModel
            {
                PrefferedBooks = _bookRepository.PrefferedBooks,
                Slide = _slideRepository.Slide

            };
            return View(homeViewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [Route("/Orders")]
        public async Task<IActionResult> Orders()
        {
 
            var applicationDbContext = _context.OrderDetails.Include(b => b.Book).Include(o => o.Order);
            
            return View(await applicationDbContext.ToListAsync());
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


    }
}
