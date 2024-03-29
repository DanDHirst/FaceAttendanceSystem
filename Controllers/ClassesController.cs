﻿using System;
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
using Microsoft.AspNetCore.Hosting;
using Microsoft.Azure.CognitiveServices.Vision.Face.Models;
using PersonRecog;

namespace FaceAttendance.Controllers
{
    public class ClassesController : Controller
    {
        private readonly CourseContext _context;
        private IWebHostEnvironment Environment;

        public ClassesController(CourseContext context, IWebHostEnvironment _environment)
        {
            _context = context;
            Environment = _environment;
        }
        public List<Class> AddLecturersAndModules( List<Class> classes)
        {
            foreach (var c in classes)
            {
                c.Lecturer = (from l in _context.Lecturers where c.LecturerID == l.ID select l).First();
                c.Module = (from m in _context.Modules where m.ID == c.ModuleID select m).First();
            }
            return classes;
        }

        // GET: Classes

        public async Task<IActionResult> Index()
        {
            
            var courses = await _context.Classes.ToListAsync();
            var classes = AddLecturersAndModules(courses);
            return View(classes);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(string room, DateTime startTime)
        {
            
            ViewData["room"] = room;
            ViewData["startTime"] = startTime;

            if(room == null && startTime.Year == 0001)
            {
                var course = await _context.Classes.ToListAsync();
                var classes = AddLecturersAndModules(course);
                return View(classes);
            }

            if (room != null && startTime.Year == 0001 )
            {
                var cl = await (from c in _context.Classes where c.Room == room select c).ToListAsync();
                var classes = AddLecturersAndModules(cl);
                return View(classes);
            }
            
            if (room != null && startTime.Year != 0001)
            {
                var cl = await (from c in _context.Classes where c.Room == room && c.StartDateTime > startTime select c).ToListAsync();
                var classes = AddLecturersAndModules(cl);
                return View(classes);
            }
            if (room == null && startTime.Year != 0001)
            {
                var cl = await (from c in _context.Classes where  c.StartDateTime > startTime select c).ToListAsync();
                var classes = AddLecturersAndModules(cl);
                return View(classes);
            }

            var courses = await _context.Classes.ToListAsync();
            var cla = AddLecturersAndModules(courses);
            return View(cla);



        }
        public async Task<List<Student>> GetStudentsAsync(int? id)
        {
            //find all the students attached to this class
            //get find moduleID
            Class cla = await _context.Classes.FindAsync(id);
            //find all the courses that match to the module
            var modules = await(from m in _context.ModuleLists where m.ModuleID == cla.ModuleID select m).ToListAsync();
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
                var courselist = await(from cl in _context.CourseLists where cl.CourseID == c.ID select cl).ToListAsync();
                foreach (var cl in courselist)
                {
                    var student = await _context.Students.FindAsync(cl.StudentID);
                    students.Add(student);
                }
            }
            return students;
        }

        // GET: Classes/Details/5
        public async Task<IActionResult> Details(int? id, string msg)
        {
            if (id == null)
            {
                return NotFound();
            }

            ViewData["Error"] = msg;

            //send data to the page
            var students = await GetStudentsAsync(id);
            var registeredStudents = (from s in _context.RegisteredStudents where s.ClassID == id select s).ToList();

            ViewData["Registered"] = registeredStudents;

            ViewData["Students"] = students;



            var @class = await _context.Classes
                .FirstOrDefaultAsync(m => m.ID == id);
            if (@class == null)
            {
                return NotFound();
            }
            //populate the lecturer and modulde data


            @class.Lecturer = (from l in _context.Lecturers where l.ID == @class.LecturerID select l).FirstOrDefault();
            @class.Module = (from m in _context.Modules where m.ID == @class.ModuleID select m).FirstOrDefault();
            

            return View(@class);
        }

        // GET: Classes/Create
        public IActionResult Create()
        {
            ViewData["LecturerID"] = new SelectList(_context.Lecturers, "ID", "LecturerName");
            ViewData["ModuleID"] = new SelectList(_context.Modules, "ID", "ModuleName");
            ViewData["Rooms"] = new SelectList(Constants.ROOMS, "", "","");
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
            ViewData["Rooms"] = new SelectList(Constants.ROOMS,"","",@class.Room);
            ViewData["LecturerID"] = new SelectList(_context.Lecturers, "ID", "LecturerName", @class.LecturerID);
            ViewData["ModuleID"] = new SelectList(_context.Modules, "ID", "ModuleName", @class.ModuleID);
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

        public async Task<IActionResult> Register(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var students = await GetStudentsAsync(id);
            var registeredStudents = (from s in _context.RegisteredStudents where s.ClassID == id select s).ToList();

            ViewData["Registered"] = registeredStudents;

            ViewData["Students"] = students;

            var @class = await _context.Classes
                 .FirstOrDefaultAsync(m => m.ID == id);
            if (@class == null)
            {
                return NotFound();
            }

            return View(@class);



        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterID(int id,int studentID)
        {
            string msg = "Student Not Found";
            var student = (from s in _context.Students where s.StudentCode == studentID select s).SingleOrDefault();
            if (student == null)
            {
                return RedirectToAction(nameof(Details), new { id,msg });
            }

            RegisteredStudent rs = new RegisteredStudent
            {
                ClassID = id,
                StudentID = student.ID,
                RegisteredTime = DateTime.Now,
            };

            var checkReg = (from r in _context.RegisteredStudents where r.ClassID == rs.ClassID && r.StudentID == rs.StudentID select r).ToList();
            if (checkReg.Count > 0)
            {
                msg = "Student Already Regisitered";
                return RedirectToAction(nameof(Details), new { id,msg });
            }

            _context.RegisteredStudents.Add(rs);
            await _context.SaveChangesAsync();
            

            return RedirectToAction(nameof(Details) , new {id});
        }

        // POST: Classes/register/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(int id, List<IFormFile> postedFiles)
        {
            if (id == 0)
            {
                return NotFound();
            }
            
            string wwwPath = this.Environment.WebRootPath;
            string contentPath = this.Environment.ContentRootPath;

            string path = Path.Combine(this.Environment.WebRootPath, "image");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            List<string> uploadedFiles = new List<string>();
            foreach (IFormFile postedFile in postedFiles)
            {
                //string fileName = Path.GetFileName(postedFile.FileName);
                string fileName = Path.GetFileName("newPhoto.jpg");
                using (FileStream stream = new FileStream(Path.Combine(path, fileName), FileMode.Create))
                {
                    postedFile.CopyTo(stream);
                    uploadedFiles.Add(fileName);
                    ViewBag.Message += string.Format("<b>{0}</b> uploaded.<br />", fileName);
                }
            }

            const string IMAGE_BASE_URL = Constants.IMAGE_BASE_URL;
            const string IMAGE_NEW_URL = Constants.IMAGE_NEW_URL;

            const string SUBSCRIPTION_KEY = Constants.SUBSCRIPTION_KEY;
            const string ENDPOINT = Constants.ENDPOINT;
            const string RECOGNITION_MODEL3 = RecognitionModel.Recognition03;
            var filename = "newPhoto.jpg";
            var students = await GetStudentsAsync(id);
            BlobStorage.UploadAFile("newimage", filename);

            // Authenticate.
            var client = FaceRecognition.Authenticate(ENDPOINT, SUBSCRIPTION_KEY);
            var face = new FaceRecognition();
            // Find Similar - find a similar face from a list of faces.
            face.FindSimilar(client, IMAGE_BASE_URL, IMAGE_NEW_URL, RECOGNITION_MODEL3,students,filename).Wait();
            BlobStorage.DeleteAFile("newimage", filename);

            ImageDetails matchedImage = new ImageDetails();


            matchedImage.url = face.url;
            if (matchedImage.url == null)
            {
                matchedImage.notFound = true;
                System.IO.File.Delete(("./wwwroot/image/" + filename));
                return View(matchedImage);
            }
            matchedImage.confidence = (face.confidence * 100);
            int idWithoutExtension = int.Parse(Path.GetFileNameWithoutExtension(face.name));
            matchedImage.ID = idWithoutExtension;

            var student = await _context.Students.FindAsync(idWithoutExtension);
            matchedImage.studentCode = student.StudentCode;
            matchedImage.username = student.StudentName;
            matchedImage.active = student.active;

            System.IO.File.Delete(("./wwwroot/image/" + filename));

            RegisteredStudent rs = new RegisteredStudent
            {
                ClassID = id,
                StudentID = matchedImage.ID,
                RegisteredTime = DateTime.Now,

          
                
            };
            var checkReg =  (from r in _context.RegisteredStudents where r.ClassID == rs.ClassID && r.StudentID == rs.StudentID select r).ToList();
            if (checkReg.Count < 1)
            {
                _context.RegisteredStudents.Add(rs);
                await _context.SaveChangesAsync();
            }

            

            var registeredStudents = (from s in _context.RegisteredStudents where s.ClassID == id select s).ToList();

            ViewData["Registered"] = registeredStudents;

            ViewData["Students"] = students;
            ViewData["Matched"] = matchedImage;

            var @class = await _context.Classes
                 .FirstOrDefaultAsync(m => m.ID == id);
            if (@class == null)
            {
                return NotFound();
            }

            return View(@class);



            
        }
    }
}
