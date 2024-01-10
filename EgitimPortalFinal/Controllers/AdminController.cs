using AspNetCoreHero.ToastNotification.Abstractions;
using EgitimPortalFinal.Models;
using EgitimPortalFinal.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;

namespace EgitimPortalFinal.Controllers
{
    [Authorize(Roles = "admin")]

    public class AdminController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IFileProvider _fileProvider;
        private readonly INotyfService _notify;

        public AdminController(AppDbContext context, IFileProvider fileProvider, INotyfService notify)
        {
            _context = context;
            _fileProvider = fileProvider;
            _notify = notify;
        }
        public IActionResult Index()
        {
            return View();
        }


        [Authorize(Roles = "admin")]
        public async Task<IActionResult> List()
        {

            return _context.Course != null ?
                        View(await _context.Course.ToListAsync()) :
                        Problem("Entity set 'AppDbContext.Course'  is null.");
        }

        [Authorize(Roles = "admin")]
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CourseModel newCourse)
        {
            var rootFolder = _fileProvider.GetDirectoryContents("wwwroot");
            var photoUrl = "-";
            if (newCourse.PhotoFile != null)
            {
                var filename = Path.GetFileName(newCourse.PhotoFile.FileName);
                var photoPath = Path.Combine(rootFolder.First(x => x.Name == "CoursePhotos").PhysicalPath, filename);
                using var stream = new FileStream(photoPath, FileMode.Create);
                newCourse.PhotoFile.CopyTo(stream);
                photoUrl = filename;
            }

            var Course = new Course
            {
                Title = newCourse.Title,
                Content = newCourse.Content,
                Description = newCourse.Description,
                Tag = newCourse.Tag,
                PhotoUrl = photoUrl
            };

            _context.Course.Add(Course);
            _context.SaveChanges();
            _notify.Success("Ders Başarıyla Eklendi.");
            return View(newCourse);
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Course == null)
            {
                return NotFound();
            }

            var Course = await _context.Course.FindAsync(id);
            if (Course == null)
            {
                return NotFound();
            }

            CourseModel newCourse = new()
            {
                Id = Course.Id,
                Content = Course.Content,
                Tag = Course.Tag,
                Description = Course.Description,
                Title = Course.Title,
                PhotoFile = null
            };

            return View(newCourse);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CourseModel newCourse)
        {
            try
            {
                var rootFolder = _fileProvider.GetDirectoryContents("wwwroot");
                var photoUrl = "-";

                if (newCourse.PhotoFile != null)
                {
                    var filename = Path.GetFileName(newCourse.PhotoFile.FileName);
                    var photoPath = Path.Combine(rootFolder.First(x => x.Name == "CoursePhotos").PhysicalPath, filename);
                    using var stream = new FileStream(photoPath, FileMode.Create);
                    newCourse.PhotoFile.CopyTo(stream);
                    photoUrl = filename;
                }

                var Course = _context.Course.FirstOrDefault(n => n.Id == newCourse.Id);
                Course.Title = newCourse.Title;
                Course.Content = newCourse.Content;
                Course.Description = newCourse.Description;
                Course.Tag = newCourse.Tag;
                Course.PhotoUrl = photoUrl;
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CourseExists(newCourse.Id))
                {
                    return NotFound();
                }

                throw;
            }

            return RedirectToAction(nameof(List));
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Course == null)
            {
                return NotFound();
            }

            var Course = await _context.Course
                .FirstOrDefaultAsync(m => m.Id == id);
            if (Course == null)
            {
                return NotFound();
            }

            return View(Course);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Course == null)
            {
                return Problem("Entity set 'AppDbContext.Course' is null.");
            }

            var Course = await _context.Course.FindAsync(id);
            if (Course == null)
            {
                return Problem("Data not found.");
            }

            var rootFolder = _fileProvider.GetDirectoryContents("wwwroot");
            if (Course.PhotoUrl != null && Course.PhotoUrl.Length > 1)
            {
                var filename = Path.GetFileName(Course.PhotoUrl);
                var photoPath = Path.Combine(rootFolder.First(x => x.Name == "CoursePhotos").PhysicalPath, filename);
                System.IO.File.Delete(photoPath);
            }

            _context.Course.Remove(Course);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(List));
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Course == null)
            {
                return NotFound();
            }

            var Course = await _context.Course
                .FirstOrDefaultAsync(m => m.Id == id);
            if (Course == null)
            {
                return NotFound();
            }

            return View(Course);
        }

        private bool CourseExists(int id)
        {
            return (_context.Course?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}

