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
using Microsoft.Extensions.Configuration;
using System.IO;
using Microsoft.Azure.Storage.Blob;
using Microsoft.Azure.Storage;
using Microsoft.AspNetCore.Identity;

namespace BookStore.Views.Slides
{
    public class SlidesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private UserManager<IdentityUser> _userManager;

        public SlidesController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
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

        // GET: Slides
        [Route("Admin/Slides")]
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

            return View(await _context.Slide.ToListAsync());
        }

        // GET: Slides/Details/5
        [Route("Admin/Slides/Details/{id}")]
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

            var slide = await _context.Slide
                .FirstOrDefaultAsync(m => m.ID == id);
            if (slide == null)
            {
                return NotFound();
            }

            return View(slide);
        }

        // GET: Slides/Create
        [Route("Admin/Slides/Create")]
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

        // POST: Slides/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Route("Admin/Slides/Create")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,SlideUrl,SlideBlobRef")] Slide slide, IFormFile file)
        {
            _context.Add(slide);
            CloudBlobContainer container = GetCloudBlobContainer();

            if (file.Length > 0)
            {
                CloudBlockBlob blob = container.GetBlockBlobReference("Slides/" + DateTime.Now);
                slide.SlideUrl = blob.Uri.AbsoluteUri;
                slide.SlideBlobRef = blob.Name;

                using (var fileStream = file.OpenReadStream())
                {
                    await blob.UploadFromStreamAsync(fileStream);
                }

            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));


            //return View(slide);
        }

        // GET: Slides/Edit/5
        [Route("Admin/Slides/Edit/{id}")]
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

            var slide = await _context.Slide.FindAsync(id);
            if (slide == null)
            {
                return NotFound();
            }
            return View(slide);
        }

        // POST: Slides/Edit/5
        [Route("Admin/Slides/Edit/{id}")]
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,SlideUrl,SlideBlobRef")] Slide slide, IFormFile file)
        {
            if (id != slide.ID)
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
                            CloudBlockBlob blobOld = container.GetBlockBlobReference(slide.SlideBlobRef);
                            blobOld.DeleteAsync().Wait();

                            //Upload New Image
                            CloudBlockBlob blob = container.GetBlockBlobReference("Slides/" + DateTime.Now);
                            slide.SlideUrl = blob.Uri.AbsoluteUri;
                            slide.SlideBlobRef = blob.Name;

                            using (var fileStream = file.OpenReadStream())
                            {
                                await blob.UploadFromStreamAsync(fileStream);
                            }
                        }

                    }


                    _context.Update(slide);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SlideExists(slide.ID))
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
            return View(slide);
        }

        // GET: Slides/Delete/5
        [Route("Admin/Slides/Delete/{id}")]
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

            var slide = await _context.Slide
                .FirstOrDefaultAsync(m => m.ID == id);
            if (slide == null)
            {
                return NotFound();
            }

            return View(slide);
        }

        // POST: Slides/Delete/5
        [Route("Admin/Slides/Delete/{id}")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var slide = await _context.Slide.FindAsync(id);
            if (slide.SlideUrl != null)
            {
                //Delete from Blob Storage
                CloudBlobContainer container = GetCloudBlobContainer();
                CloudBlockBlob blob = container.GetBlockBlobReference(slide.SlideBlobRef);
                blob.DeleteAsync().Wait();
            }


            _context.Slide.Remove(slide);
            await _context.SaveChangesAsync();



            return RedirectToAction(nameof(Index));
        }

        private bool SlideExists(int id)
        {
            return _context.Slide.Any(e => e.ID == id);
        }
    }
}
