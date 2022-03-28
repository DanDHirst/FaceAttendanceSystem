using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FaceAttendance.Data;
using FaceAttendance.Models;
using Microsoft.AspNetCore.Identity;

namespace FaceAttendance.Controllers
{
    public class LecturersController : Controller
    {
        private readonly CourseContext _context;
        private readonly UserManager<AppUser> _userManager;

        public LecturersController(CourseContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Lecturers
        public async Task<IActionResult> Index()
        {
            return View(await _context.Lecturers.ToListAsync());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(int lecturerCode)
        {
            if(lecturerCode == 0)
            {
                return View(await _context.Lecturers.ToListAsync());
            }
            var lecturer = await (from l in _context.Lecturers where l.LecturerCode == lecturerCode select l).ToListAsync();
            return View(lecturer);
        }

        // GET: Lecturers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            
            var lecturer = await _context.Lecturers
                .FirstOrDefaultAsync(m => m.ID == id);
            if (lecturer == null)
            {
                return NotFound();
            }

            return View(lecturer);
        }

        // GET: Lecturers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Lecturers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,LecturerCode,LecturerName")] Lecturer lecturer)
        {
            if (ModelState.IsValid)
            {
                _context.Add(lecturer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(lecturer);
        }

        // GET: Lecturers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lecturer = await _context.Lecturers.FindAsync(id);
            if (lecturer == null)
            {
                return NotFound();
            }
            return View(lecturer);
        }

        // POST: Lecturers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,LecturerCode,LecturerName")] Lecturer lecturer)
        {
            if (id != lecturer.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(lecturer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LecturerExists(lecturer.ID))
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
            return View(lecturer);
        }

        // GET: Lecturers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lecturer = await _context.Lecturers
                .FirstOrDefaultAsync(m => m.ID == id);
            if (lecturer == null)
            {
                return NotFound();
            }

            return View(lecturer);
        }

        // POST: Lecturers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var lecturer = await _context.Lecturers.FindAsync(id);
            _context.Lecturers.Remove(lecturer);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LecturerExists(int id)
        {
            return _context.Lecturers.Any(e => e.ID == id);
        }

        public async Task<IActionResult> Dashboard()
        {
            //find code of the lecturer called lastname on appuser
            var user = await _userManager.GetUserAsync(User);
            var userRole = user?.UserRole;
            if (user == null || userRole != "Lecturer" ){
                return BadRequest();
            }

            //find the lecturer from list 
            var lecturer = (from l in _context.Lecturers where l.LecturerCode.ToString() == user.Lastname select l).FirstOrDefault();
            if(lecturer == null)
            {
                return NotFound();
            }
            //find classes that the lecturer has
            //sort them by upcoming and completed
            var UpcomingClasses = (from c in _context.Classes where c.LecturerID == lecturer.ID && c.EndDateTime > DateTime.Now select c).ToList();
            foreach(var c in UpcomingClasses)
            {
                c.Module = (from m in _context.Modules where m.ID == c.ModuleID select m).FirstOrDefault();
            }
            var CompletedClasses = (from c in _context.Classes where c.LecturerID == lecturer.ID && c.EndDateTime < DateTime.Now select c).ToList();
            foreach (var c in CompletedClasses)
            {
                c.Module = (from m in _context.Modules where m.ID == c.ModuleID select m).FirstOrDefault();
            }

            //output classes using viewdata
            ViewData["Upcoming"] = UpcomingClasses;
            ViewData["Completed"] = CompletedClasses;
            return View();
        }
    }
}
