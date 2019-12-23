using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BookStore.Data;
using BookStore.Models;
using Microsoft.AspNetCore.Identity;

namespace BookStore.Views.Admin.Categories
{
    public class CategoriesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private UserManager<IdentityUser> _userManager;

        public CategoriesController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Categories
        [Route("Admin/Genres")]
        public async Task<IActionResult> Index()
        {
            //Validating the user
            try
            {
                if (!_userManager.GetUserAsync(User).Result.Email.Equals("nurda9727@gmail.com"))
                {
                    return Redirect("/");
                }
            }
            catch (Exception e)
            {
                Console.Write(e); return Redirect("/");
            }

            return View(await _context.Categories.ToListAsync());
        }

        // GET: Categories/Details/5
        [Route("Admin/Genres/Details/{id}")]
        public async Task<IActionResult> Details(int? id)
        {
            //Validating the user
            try
            {
                if (!_userManager.GetUserAsync(User).Result.Email.Equals("nurda9727@gmail.com"))
                {
                    return Redirect("/");
                }
            }
            catch (Exception e)
            {
                Console.Write(e); return Redirect("/");
            }

            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Categories
                .FirstOrDefaultAsync(m => m.CategoryId == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // GET: Categories/Create
        [Route("Admin/Genres/Create")]
        public IActionResult Create()
        {
            //Validating the user
            try
            {
                if (!_userManager.GetUserAsync(User).Result.Email.Equals("nurda9727@gmail.com"))
                {
                    return Redirect("/");
                }
            }
            catch (Exception e)
            {
                Console.Write(e); return Redirect("/");
            }

            return View();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Route("Admin/Genres/Create")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CategoryId,CategoryName")] Category category)
        {
            if (ModelState.IsValid)
            {
                _context.Add(category);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // GET: Categories/Edit/5
        [Route("Admin/Genres/Edit/{id}")]
        public async Task<IActionResult> Edit(int? id)
        {
            //Validating the user
            try
            {
                if (!_userManager.GetUserAsync(User).Result.Email.Equals("nurda9727@gmail.com"))
                {
                    return Redirect("/");
                }
            }
            catch (Exception e)
            {
                Console.Write(e); return Redirect("/");
            }

            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Route("Admin/Genres/Edit/{id}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CategoryId,CategoryName")] Category category)
        {
            if (id != category.CategoryId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(category);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(category.CategoryId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        [Route("Admin/Genres/Delete/{id}")]
        // GET: Categories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            //Validating the user
            try
            {
                if (!_userManager.GetUserAsync(User).Result.Email.Equals("nurda9727@gmail.com"))
                {
                    return Redirect("/");
                }
            }
            catch (Exception e)
            {
                Console.Write(e); return Redirect("/");
            }

            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Categories
                .FirstOrDefaultAsync(m => m.CategoryId == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // POST: Categories/Delete/5
        [Route("Admin/Genres/Delete/{id}")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CategoryExists(int id)
        {
            return _context.Categories.Any(e => e.CategoryId == id);
        }
    }
}
