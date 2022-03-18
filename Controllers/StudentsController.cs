using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FaceAttendance.Data;
using FaceAttendance.Models;
using Microsoft.AspNetCore.Http;
using System.IO;
using PersonRecog;
using Microsoft.AspNetCore.Hosting;
using FaceAttendance.ViewModel;

namespace FaceAttendance.Controllers
{
    public class StudentsController : Controller
    {
        private readonly CourseContext _context;
        private IWebHostEnvironment Environment;

        public StudentsController(CourseContext context, IWebHostEnvironment _environment)
        {
            _context = context;
            Environment = _environment;
        }


        // GET: Students
        public async Task<IActionResult> Index()
        {
            return View(await _context.Students.ToListAsync());
        }

        // GET: Students/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students
                .FirstOrDefaultAsync(m => m.ID == id);
            if (student == null)
            {
                return NotFound();
            }

            CourseList co = (from c in _context.CourseLists
                             where c.StudentID == student.ID
                             select c).SingleOrDefault();
            Course courseName = null;
            if (co != null)
            {
                 courseName = await _context.Courses.FindAsync(co.CourseID);
            }

            //find student attendance
            var Registered = (from a in _context.RegisteredStudents where a.StudentID == id select a).ToList();
            
            foreach (RegisteredStudent r in Registered)
            {
                r.Class = await _context.Classes.FindAsync(r.ClassID);
                r.Class.Module = await _context.Modules.FindAsync(r.Class.ModuleID);
            }

            ViewData["Registered"] = Registered;


            ViewData["Course"] = courseName;

            return View(student);
        }

        // GET: Students/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Students/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,StudentCode,StudentName,imageUrl,active")] Student student, List<IFormFile> postedFiles)
        {
            if (ModelState.IsValid == false)
            {
                return View(student);
            }
            _context.Add(student);
            await _context.SaveChangesAsync();



            string wwwPath = this.Environment.WebRootPath;
            string contentPath = this.Environment.ContentRootPath;

            string path = Path.Combine(this.Environment.WebRootPath, "image");
            string fileName = student.ID.ToString() + ".jpg";   //Path.GetFileName(postedFile.FileName);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            List<string> uploadedFiles = new List<string>();
            foreach (IFormFile postedFile in postedFiles)
            {


                using (FileStream stream = new FileStream(Path.Combine(path, fileName), FileMode.Create))
                {
                    postedFile.CopyTo(stream);
                    uploadedFiles.Add(fileName);
                    ViewBag.Message += string.Format("<b>{0}</b> uploaded.<br />", fileName);
                }
                BlobStorage.UploadAFile("images", fileName);
                System.IO.File.Delete(("./wwwroot/image/" + fileName));
            }
            student.imageUrl = Constants.IMAGE_BASE_URL + fileName;

            await _context.SaveChangesAsync();

            
            return RedirectToAction(nameof(Index));


        }
        

        // GET: Students/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            StudentViewModel viewModel = new StudentViewModel();
            List<Course> courses = await _context.Courses.ToListAsync();
            courses.Add(new Course {ID=-1,CourseCode=-1, CourseName="None" });
            var student = await _context.Students.FindAsync(id);
            viewModel.Student = student;
            viewModel.Courses = courses;

            CourseList co = (from c in _context.CourseLists
                      where c.StudentID == student.ID
                      select c).SingleOrDefault();
            if(co != null)
            {
                Course courseName = await _context.Courses.FindAsync(co.CourseID);
                
                ViewData["Courses"] = new SelectList(courses, "ID", "CourseName", courseName.ID);
            }
            else
            {
                ViewData["Courses"] = new SelectList(courses, "ID", "CourseName",-1);
            }


            


            if (student == null)
            {
                return NotFound();
            }
            return View(viewModel);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,StudentCode,StudentName,imageUrl,active")] Student student, List<IFormFile> postedFiles, int selectCourse)
        {

            if (id != student.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(student);
                    if (selectCourse != -1)
                    {
                        
                        var result = await _context.CourseLists.Where(x => x.StudentID == student.ID).ToListAsync();
                        if (result.Count < 1)
                        {
                            CourseList course = new CourseList();
                            course.StudentID = student.ID;
                            course.CourseID = selectCourse;
                            _context.CourseLists.Add(course);
                        }
                        else
                        {
                            var co = (from c in _context.CourseLists
                                      where c.StudentID == student.ID
                                      select c).Single();
                            co.CourseID = selectCourse;
                        }




                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        var result = await _context.CourseLists.Where(x => x.StudentID == student.ID).ToListAsync();
                        if(result.Count > 1)
                        {
                            _context.CourseLists.Remove(result[0]);
                            await _context.SaveChangesAsync();
                        }
                        

                    }
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentExists(student.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                if (postedFiles.Count > 0)
                {
                    try
                    {
                        BlobStorage.DeleteAFile("images", student.ID + ".jpg");
                    }
                    catch(Azure.RequestFailedException)
                    {
                        
                    }

                    string wwwPath = this.Environment.WebRootPath;
                    string contentPath = this.Environment.ContentRootPath;

                    string path = Path.Combine(this.Environment.WebRootPath, "image");
                    string fileName = student.ID.ToString() + ".jpg";   //Path.GetFileName(postedFile.FileName);
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    List<string> uploadedFiles = new List<string>();
                    foreach (IFormFile postedFile in postedFiles)
                    {


                        using (FileStream stream = new FileStream(Path.Combine(path, fileName), FileMode.Create))
                        {
                            postedFile.CopyTo(stream);
                            uploadedFiles.Add(fileName);
                            ViewBag.Message += string.Format("<b>{0}</b> uploaded.<br />", fileName);
                        }
                        BlobStorage.UploadAFile("images", fileName);
                        System.IO.File.Delete(("./wwwroot/image/" + fileName));
                    }
                    student.imageUrl = Constants.IMAGE_BASE_URL + fileName;
                    _context.Update(student);
                    await _context.SaveChangesAsync();
                }

                return RedirectToAction(nameof(Index));
            }
            return View(student);
        }

        // GET: Students/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students
                .FirstOrDefaultAsync(m => m.ID == id);
            if (student == null)
            {
                return NotFound();
            }


            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var student = await _context.Students.FindAsync(id);
            _context.Students.Remove(student);
            if (student.imageUrl != null)
            {
                var blobName = student.ID + ".jpg";
                var blobs  = BlobStorage.GetBlobs("images");
                foreach(var blob in blobs)
                {
                    if(blobName == blob)
                    {
                        BlobStorage.DeleteAFile("images", student.ID + ".jpg");
                    }
                }
                
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StudentExists(int id)
        {
            return _context.Students.Any(e => e.ID == id);
        }
    }
}
