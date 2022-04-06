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
    public class ModulesController : Controller
    {
        private readonly CourseContext _context;

        public ModulesController(CourseContext context)
        {
            _context = context;
        }

        // GET: Modules
        public async Task<IActionResult> Index()
        {
            return View(await _context.Modules.ToListAsync());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(string moduleCode, string moduleName)
        {
            ViewData["moduleName"] = moduleName;
            ViewData["moduleCode"] = moduleCode;
            if (moduleCode == null && moduleName == null)
            {
                return View(await _context.Modules.ToListAsync());
            }
            if (moduleCode != null && moduleName != null)
            {
                var Modules = await (from m in _context.Modules where m.ModuleCode.Contains(moduleCode) && m.ModuleName.Contains(moduleName) select m).ToListAsync();
                return View(Modules);
            }
            if (moduleCode != null)
            {
                var Modules = await (from m in _context.Modules where m.ModuleCode.Contains( moduleCode) select m).ToListAsync();
                return View(Modules);
            }
            if (moduleName != null)
            {
                var Modules = await (from m in _context.Modules where m.ModuleName.Contains(moduleName) select m).ToListAsync();
                return View(Modules);
            }
            return View(await _context.Modules.ToListAsync());
        }


        // GET: Modules/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            //get all courses that contain this module

            List<ModuleList> modList = await (from m in _context.ModuleLists where m.ModuleID == id select m).ToListAsync();
            List<Course> courses = new List<Course>();

            foreach(var m in modList)
            {
                Course c = await _context.Courses.FindAsync(m.CourseID);
                courses.Add(c);
            }


            //get all the clas ses on the module
            List<Class> classes = await (from c in _context.Classes where c.ModuleID == id select c).ToListAsync();



            var @module = await _context.Modules
                .FirstOrDefaultAsync(m => m.ID == id);
            if (@module == null)
            {
                return NotFound();
            }

            //populate the lecturer and modulde data
            foreach (var c in classes)
            {
                c.Lecturer = (from l in _context.Lecturers where l.ID == c.LecturerID select l).FirstOrDefault();
                c.Module = (from m in _context.Modules where m.ID == c.ModuleID select m).FirstOrDefault();
            }

            ViewData["Courses"] = courses;
            ViewData["Classes"] = classes;
            return View(@module);
        }

        // GET: Modules/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Modules/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,ModuleName,ModuleCode")] Module @module)
        {
            if (ModelState.IsValid)
            {
                _context.Add(@module);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(@module);
        }

        // GET: Modules/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @module = await _context.Modules.FindAsync(id);
            if (@module == null)
            {
                return NotFound();
            }
            return View(@module);
        }

        // POST: Modules/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,ModuleName,ModuleCode")] Module @module)
        {
            if (id != @module.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(@module);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ModuleExists(@module.ID))
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
            return View(@module);
        }

        // GET: Modules/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @module = await _context.Modules
                .FirstOrDefaultAsync(m => m.ID == id);
            if (@module == null)
            {
                return NotFound();
            }

            return View(@module);
        }

        // POST: Modules/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var @module = await _context.Modules.FindAsync(id);
            _context.Modules.Remove(@module);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ModuleExists(int id)
        {
            return _context.Modules.Any(e => e.ID == id);
        }
    }
}
