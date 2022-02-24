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
    public class CoursesController : Controller
    {
        private readonly CourseContext _context;

        public CoursesController(CourseContext context)
        {
            _context = context;
        }

        // GET: Courses
        public async Task<IActionResult> Index()
        {
            return View(await _context.Courses.ToListAsync());
        }

        // GET: Courses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            //get all students enroled on course
            List<Student> students = await _context.Students.ToListAsync();

            List<Student> studentsOnCourse = new List<Student>();
            var courseList = (from c in _context.CourseLists where id == c.CourseID select c).ToList();



            foreach (CourseList c in courseList)
            {

                Student student = await _context.Students.FindAsync(c.StudentID);
                studentsOnCourse.Add(student);
                          
            }
            //get all modules attached to the course
            var moduleList = (from c in _context.ModuleLists where id == c.CourseID select c).ToList();
            List<Module> modules = new List<Module>();
            foreach (ModuleList m in moduleList)
            {
                Module mod = await _context.Modules.FindAsync(m.ModuleID);
                modules.Add(mod);
            }







                var course = await _context.Courses
                .FirstOrDefaultAsync(m => m.ID == id);
            if (course == null)
            {
                return NotFound();
            }
            ViewData["Modules"] = modules;

            ViewData["Students"] = studentsOnCourse;
            return View(course);
        }

        // GET: Courses/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Courses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,CourseCode,CourseName")] Course course)
        {
            if (ModelState.IsValid)
            {
                _context.Add(course);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(course);
        }

        // GET: Courses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Courses.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }
            return View(course);
        }

        // POST: Courses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,CourseCode,CourseName")] Course course, int StudentID, string ModuleCode)
        {
            //add Module to course
            if(ModuleCode != null)
            {
                //find module
                var module = (from m in _context.Modules where m.ModuleCode == ModuleCode select m).SingleOrDefault();
                if(module == null){
                    return LocalRedirect("/Courses/Details/" + id);
                }

                //check module isnt already assigned
                var isInCourse = (from m in _context.ModuleLists where m.ModuleID == module.ID && m.CourseID == id select m).SingleOrDefault();

                if(isInCourse == null)
                {
                    ModuleList modList = new ModuleList();
                    modList.CourseID = id;
                    modList.ModuleID = module.ID;
                    modList.ModuleListID = 0;
                    _context.ModuleLists.Add(modList);
                    await _context.SaveChangesAsync();
                }
                return LocalRedirect("/Courses/Details/" + id);

                //add module to course

            }

            //add student to course
            if(StudentID > 0)
            {

                CourseList clist = new CourseList();

                var student = (from c in _context.Students
                                    where c.StudentCode == StudentID
                                    select c).SingleOrDefault();
                clist.StudentID = student.ID;
                
                clist.CourseID = id;
                
                var isInDatabase = (from c in _context.CourseLists
                               where c.StudentID == student.ID
                                    select c).SingleOrDefault();
                if (isInDatabase != null)
                {
                    isInDatabase.CourseID = id;
                }
                else
                {
                    await _context.CourseLists.AddAsync(clist);
                }
                await _context.SaveChangesAsync();

                return LocalRedirect("/Courses/Details/"+id);
            }


            if (id != course.ID)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(course);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CourseExists(course.ID))
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
            return View(course);
        }

        // GET: Courses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Courses
                .FirstOrDefaultAsync(m => m.ID == id);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        // POST: Courses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost, ActionName("RemoveModule")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveModule(int id, int moduleID)
        {
            //get id of modulelists
            var modulelist = await (from ml in _context.ModuleLists where ml.CourseID == id && ml.ModuleID == moduleID select ml).SingleAsync();
            _context.ModuleLists.Remove(modulelist);
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", new { id = id });
        }
        [HttpPost, ActionName("RemoveStudent")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveStudent(int id, int studentID)
        {
            //get id of student in courselists
            var student = await (from cl in _context.CourseLists where cl.CourseID == id && cl.StudentID == studentID select cl).SingleAsync();
            _context.CourseLists.Remove(student);
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", new { id = id });
        }

        private bool CourseExists(int id)
        {
            return _context.Courses.Any(e => e.ID == id);
        }
    }
}
