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

namespace BookStore.Views.Contacts
{
    public class ContactsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private UserManager<IdentityUser> _userManager;

        public ContactsController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Contacts
        [Route("Admin/Feedbacks")]
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

            return View(await _context.Contact.ToListAsync());
        }

        // GET: Contacts/Details/5
        [Route("Admin/Feedbacks/Details/{id}")]
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

            var contact = await _context.Contact
                .FirstOrDefaultAsync(m => m.ID == id);
            if (contact == null)
            {
                return NotFound();
            }

            return View(contact);
        }

        // GET: Contacts/Create
        [Route("Contact")]
        public IActionResult Create()
        {


            return View();
        }

        // POST: Contacts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Contact")]
        public async Task<IActionResult> Create([Bind("ID,Name,Email,Message")] Contact contact)
        {
            if (ModelState.IsValid)
            {
                _context.Add(contact);
                await _context.SaveChangesAsync();
                return Redirect("/");
            }
            return View(contact);
        }


        // GET: Contacts/Delete/5
        [Route("Admin/Feedbacks/Delete/{id}")]
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

            var contact = await _context.Contact
                .FirstOrDefaultAsync(m => m.ID == id);
            if (contact == null)
            {
                return NotFound();
            }

            return View(contact);
        }

        // POST: Contacts/Delete/5
        [Route("Admin/Feedbacks/Delete/{id}")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var contact = await _context.Contact.FindAsync(id);
            _context.Contact.Remove(contact);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ContactExists(int id)
        {
            return _context.Contact.Any(e => e.ID == id);
        }
    }
}
