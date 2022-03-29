using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PhotoApplication.Models;
using PhotoApplication.Models.View;
using System.Diagnostics;

namespace PhotoApplication.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationContext db;

        public HomeController(ApplicationContext context)
        {
            db = context;
        }

        public IActionResult Index(List<int>? tagIds)
        {
            IQueryable<Image> images = db.Images.Include(x => x.Tags);

            if (tagIds != null)
            {
                images = images.Where(p => p.Tags.Where(x => tagIds.Contains(x.Id)).Count() == tagIds.Count);
            }

            var tags = new SelectList(db.Tags
                .Select(x => new { x.Id, x.Title }), "Id", "Title");

            IndexViewModel viewModel = new IndexViewModel
            {
                Images = images,
                Tags = tags
            };

            return View(viewModel);

        }

        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id != null)
            {
                Image? image = await db.Images
                    .Include(x => x.Tags)
                    .FirstOrDefaultAsync(p => p.Id == id);

                return View(image);
            }

            return NotFound();
        }

        [HttpGet]
        [ActionName("Delete")]
        public async Task<IActionResult> ConfirmDelete(int? id)
        {
            if (id != null)
            {
                Image? image = await db.Images.FirstOrDefaultAsync(p => p.Id == id);
                if (image != null)
                    return View(image);
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id != null)
            {
                Image? image = await db.Images.FirstOrDefaultAsync(p => p.Id == id);
                if (image != null)
                {
                    var articlesTagsToDelete = db.ImagesTags.Where(x => x.ImageId == image.Id);
                    db.ImagesTags.RemoveRange(articlesTagsToDelete);
                    db.Images.Remove(image);
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
            }
            return NotFound();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}