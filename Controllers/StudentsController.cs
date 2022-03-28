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
        // post: search Students
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(int studentcode)
        {
            if(studentcode == 0)
            {
                return View(await _context.Students.ToListAsync());
            }
            var students = await (from s in _context.Students where s.StudentCode == studentcode select s).ToListAsync();
            return View(students);
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
            //find total classes that are available for the student
            List<CourseList> course = await (from c in _context.CourseLists where c.StudentID == id select c).ToListAsync();
            //
            ViewData["classesMissed"] = null;
            ViewData["classesAttended"] = null;
            ViewData["totalClassesMissed"] = 0;
            ViewData["totalClasses"] = 0;
            ViewData["percentage"] = 0;
            if ( course.Count > 0)
            {
                var modlist = (from m in _context.ModuleLists where course[0].ID == m.CourseID select m).ToList();
                List<Module> modules = new List<Module>();
                foreach (var m in modlist)
                {
                    modules.Add((from mo in _context.Modules where mo.ID == m.ModuleID select mo).Single());

                }
                List<Class> classesAttended = new List<Class>();
                List<Class> classesMissed = new List<Class>();
                var totalClassesMissed = 0;
                foreach (var m in modules)
                {
                    var cl = ((from c in _context.Classes where c.ModuleID == m.ID select c).ToList());
                    foreach(var cla in cl)
                    {
                        
                        if(cla.EndDateTime < DateTime.Now){
                            cla.Module = (from mods in _context.Modules where mods.ID == cla.ModuleID select mods).SingleOrDefault();
                            var registered = (from r in _context.RegisteredStudents where r.StudentID == id && r.ClassID == cla.ID select r).ToList();
                            if(registered.Count < 1)
                            {
                                totalClassesMissed += 1;
                                classesMissed.Add(cla);
                            }
                            else
                            {
                                classesAttended.Add(cla);
                            }
                            
                        }
                    }
                }
                ViewData["classesMissed"] = classesMissed;
                ViewData["classesAttended"] = classesAttended;
                ViewData["totalClassesMissed"] = totalClassesMissed;
                ViewData["totalClasses"] = classesAttended.Count + classesMissed.Count;

                double percentage = 0;
                double totalClasses = (classesAttended.Count + classesMissed.Count);
                double classesAtteneded = classesAttended.Count;
                percentage = classesAtteneded / totalClasses;

                
                percentage = percentage *100;
                ViewData["percentage"] = percentage;








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
