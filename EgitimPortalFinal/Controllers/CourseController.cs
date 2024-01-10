using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using AspNetCoreHero.ToastNotification.Abstractions;
using EgitimPortalFinal.Models;
using EgitimPortalFinal.ViewModels;

namespace HaberPortal.Controllers


{

    public class CourseController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IFileProvider _fileProvider;
        private readonly INotyfService _notify;

        public CourseController(AppDbContext context, IFileProvider fileProvider, INotyfService notify)
        {
            _context = context;
            _fileProvider = fileProvider;
            _notify = notify;
        }


        public async Task<IActionResult> Course(string searchString)
        {
            if (_context.Course == null)
            {
                return Problem("Course is null.");
            }

            var CourseQuery = from n in _context.Course
                            select n;

            if (!String.IsNullOrEmpty(searchString))
            {
                CourseQuery = CourseQuery.Where(s => s.Title!.Contains(searchString));
            }

            var sortedCourse = await CourseQuery.OrderByDescending(n => n.Id).ToListAsync();

            return View(sortedCourse);
        }

        public async Task<IActionResult> Backend()
        {
            var Course = _context.Course != null ?
                            await _context.Course.OrderByDescending(n => n.Id).ToListAsync() :
                            null;

            return Course != null ? View(Course) : Problem("Entity set 'AppDbContext.Course' is null.");
        }



        public async Task<IActionResult> Game()
        {
            var Course = _context.Course != null ?
                            await _context.Course.OrderByDescending(n => n.Id).ToListAsync() :
                            null;

            return Course != null ? View(Course) : Problem("Entity set 'AppDbContext.Course' is null.");
        }



        public async Task<IActionResult> Design()
        {
            var Course = _context.Course != null ?
                             await _context.Course.OrderByDescending(n => n.Id).ToListAsync() :
                             null;

            return Course != null ? View(Course) : Problem("Entity set 'AppDbContext.Course' is null.");
        }


        public async Task<IActionResult> Database()
        {
            var Course = _context.Course != null ?
                            await _context.Course.OrderByDescending(n => n.Id).ToListAsync() :
                            null;

            return Course != null ? View(Course) : Problem("Entity set 'AppDbContext.Course' is null.");
        }

        public async Task<IActionResult> Frontend()
        {
            var Course = _context.Course != null ?
                            await _context.Course.OrderByDescending(n => n.Id).ToListAsync() :
                            null;

            return Course != null ? View(Course) : Problem("Entity set 'AppDbContext.Course' is null.");
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

        
    }
}
