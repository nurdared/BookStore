using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BookStore.Data;
using BookStore.Models;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.Extensions.Configuration;

using Microsoft.Azure.Storage.Blob;
using Microsoft.Azure.Storage;
using Microsoft.AspNetCore.Identity;

namespace BookStore.Views.Admin.Books
{
    public class BooksController : Controller
    {
        private readonly ApplicationDbContext _context;
        private UserManager<IdentityUser> _userManager;

        public BooksController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }



        public CloudBlobContainer GetCloudBlobContainer()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");
            IConfigurationRoot Configuration = builder.Build();
            CloudStorageAccount storageAccount =
            CloudStorageAccount.Parse(Configuration[
                    "ConnectionStrings:AzureStorageConnectionString-1"]);
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference("bookstore-container");
            return container;
        }

        //create container page action
        public ActionResult CreateBlobContainer()
        {
            // Gets the container info from the GetCloudBlobContainer() in above
            CloudBlobContainer container = GetCloudBlobContainer();
            ViewBag.Success = container.CreateIfNotExistsAsync().Result;
            //create the container in storage
            ViewBag.BlobContainerName = container.Name;

            return View();
        }


        // GET: Books
        [Route("Admin/Books")]
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

            var applicationDbContext = _context.Books.Include(b => b.Category);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Books/Details/5
        [Route("Admin/Books/Details/{id}")]
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

            var book = await _context.Books
                .Include(b => b.Category)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // GET: Books/Create
        [Route("Admin/Books/Create")]
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

            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName");
            return View();
        }

        // POST: Books/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Route("Admin/Books/Create")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Book book, IFormFile file)
        {
            if (ModelState.IsValid)
            {
                _context.Add(book);

                CloudBlobContainer container = GetCloudBlobContainer();
                if (file != null)
                {
                    if (file.Length > 0)
                    {
                        CloudBlockBlob blob = container.GetBlockBlobReference("BookImages/" + book.BookName + DateTime.Now);
                        book.ImageUrl = blob.Uri.AbsoluteUri;
                        book.BlobReference = blob.Name;

                        using (var fileStream = file.OpenReadStream())
                        {
                            await blob.UploadFromStreamAsync(fileStream);
                        }
                    }
                }
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }



            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryId", book.CategoryId);
            await _context.SaveChangesAsync();


            return View(book);
        }

        // GET: Books/Edit/5
        [Route("Admin/Books/Edit/{id}")]
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

            var book = await _context.Books.FindAsync(id);

            if (book == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName", book.CategoryId);
            return View(book);
        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Route("Admin/Books/Edit/{id}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,BookName,BookAuthor,BookYear,BookDesc,ImageUrl,BlobReference,IsPrefferedBook,BookPrice,BookInStock,CategoryId")] Book book, IFormFile file)
        {
            if (id != book.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (file != null)
                    {
                        if (file.Length > 0)
                        {

                            CloudBlobContainer container = GetCloudBlobContainer();
                            //Delete Previous Book From Blob
                            CloudBlockBlob blobOld = container.GetBlockBlobReference(book.BlobReference);
                            blobOld.DeleteAsync().Wait();

                            //Upload New Image
                            CloudBlockBlob blob = container.GetBlockBlobReference("BookImages/" + book.BookName + DateTime.Now);
                            book.ImageUrl = blob.Uri.AbsoluteUri;
                            book.BlobReference = blob.Name;

                            using (var fileStream = file.OpenReadStream())
                            {
                                await blob.UploadFromStreamAsync(fileStream);
                            }
                        }

                    }


                    _context.Update(book);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookExists(book.ID))
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
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryId", book.CategoryId);
            return View(book);
        }

        // GET: Books/Delete/5
        [Route("Admin/Books/Delete/{id}")]
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

            var book = await _context.Books
                .Include(b => b.Category)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // POST: Books/Delete/5
        [Route("Admin/Books/Delete/{id}")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book.ImageUrl != null)
            {
                //Delete from Blob Storage
                CloudBlobContainer container = GetCloudBlobContainer();
                CloudBlockBlob blob = container.GetBlockBlobReference(book.BlobReference);
                blob.DeleteAsync().Wait();
            }


            _context.Books.Remove(book);
            await _context.SaveChangesAsync();



            return RedirectToAction(nameof(Index));
        }

        private bool BookExists(int id)
        {
            return _context.Books.Any(e => e.ID == id);
        }
    }
}
