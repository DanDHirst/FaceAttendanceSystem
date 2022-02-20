using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FaceAttendance.Data;
using FaceAttendance.Models;

namespace FaceAttendance.Controllers
{
    public class ClassesController : Controller
    {
        private readonly CourseContext _context;

        public ClassesController(CourseContext context)
        {
            _context = context;
        }

        // GET: Classes
        public async Task<IActionResult> Index()
        {
            var courseContext = _context.Classes;
            return View(await courseContext.ToListAsync());
        }

        // GET: Classes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }


            //find all the students attached to this class
            //get find moduleID
            Class cla = await _context.Classes.FindAsync(id);
            //find all the courses that match to the module
            var modules = await (from m in _context.ModuleLists where m.ModuleID == cla.ModuleID select m).ToListAsync();
            List<Course> courses = new List<Course>();  
            foreach (var m in modules)
            {
                var course = await _context.Courses.FindAsync(m.CourseID);
                courses.Add(course);
            }
            //find all the students on the courses
            List<Student> students = new List<Student>();
            foreach (var c in courses)
            {
                var courselist = await (from cl in _context.CourseLists where cl.CourseID == c.ID select cl).ToListAsync();
                foreach (var cl in courselist)
                {
                    var student = await _context.Students.FindAsync(cl.StudentID);
                    students.Add(student);
                }
            }
            //send data to the page
            ViewData["Students"] = students;



            var @class = await _context.Classes
                .FirstOrDefaultAsync(m => m.ID == id);
            if (@class == null)
            {
                return NotFound();
            }

            return View(@class);
        }

        // GET: Classes/Create
        public IActionResult Create()
        {
            ViewData["LecturerID"] = new SelectList(_context.Lecturers, "ID", "ID");
            ViewData["ModuleID"] = new SelectList(_context.Modules, "ID", "ID");
            return View();
        }

        // POST: Classes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,ClassCode,ModuleID,LecturerID,Room,StartDateTime,EndDateTime")] Class @class)
        {
            if (ModelState.IsValid)
            {
                _context.Add(@class);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["LecturerID"] = new SelectList(_context.Lecturers, "ID", "ID", @class.LecturerID);
            ViewData["ModuleID"] = new SelectList(_context.Modules, "ID", "ID", @class.ModuleID);
            return View(@class);
        }

        // GET: Classes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @class = await _context.Classes.FindAsync(id);
            if (@class == null)
            {
                return NotFound();
            }
            ViewData["LecturerID"] = new SelectList(_context.Lecturers, "ID", "ID", @class.LecturerID);
            ViewData["ModuleID"] = new SelectList(_context.Modules, "ID", "ID", @class.ModuleID);
            return View(@class);
        }

        // POST: Classes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,ClassCode,ModuleID,LecturerID,Room,StartDateTime,EndDateTime")] Class @class)
        {
            if (id != @class.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(@class);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClassExists(@class.ID))
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
            ViewData["LecturerID"] = new SelectList(_context.Lecturers, "ID", "ID", @class.LecturerID);
            ViewData["ModuleID"] = new SelectList(_context.Modules, "ID", "ID", @class.ModuleID);
            return View(@class);
        }

        // GET: Classes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @class = await _context.Classes
                .FirstOrDefaultAsync(m => m.ID == id);
            if (@class == null)
            {
                return NotFound();
            }

            return View(@class);
        }

        // POST: Classes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var @class = await _context.Classes.FindAsync(id);
            _context.Classes.Remove(@class);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ClassExists(int id)
        {
            return _context.Classes.Any(e => e.ID == id);
        }
    }
}
