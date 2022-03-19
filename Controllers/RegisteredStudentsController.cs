using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FaceAttendance.Data;
using FaceAttendance.Models;
using PersonRecog;
using Microsoft.Azure.CognitiveServices.Vision.Face.Models;
using System.IO;

namespace FaceAttendance.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegisteredStudentsController : ControllerBase
    {
        private readonly CourseContext _context;
        private string _auth = Constants.AUTH;

        public RegisteredStudentsController(CourseContext context)
        {
            _context = context;
        }
        public async Task<List<Student>> GetStudentsAsync(int? id)
        {
            
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
            return students;
        }

        // GET: api/RegisteredStudents
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RegisteredStudent>>> GetRegisteredStudents(string auth)
        {
            if (auth != _auth)
            {

                return BadRequest("Invalid auth token");
            }
            return await _context.RegisteredStudents.ToListAsync();
        }

        // GET: api/RegisteredStudents/5
        [HttpGet("{id}")]
        public async Task<ActionResult<RegisteredStudent>> GetRegisteredStudent(int id, string auth)
        {
            if (auth != _auth)
            {

                return BadRequest("Invalid auth token");
            }
            var registeredStudent = await _context.RegisteredStudents.FindAsync(id);

            if (registeredStudent == null)
            {
                return NotFound();
            }

            return registeredStudent;
        }

        // PUT: api/RegisteredStudents/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRegisteredStudent(int id, RegisteredStudent registeredStudent, string auth)
        {
            if (auth != _auth)
            {

                return BadRequest("Invalid auth token");
            }
            if (id != registeredStudent.ID)
            {
                return BadRequest();
            }

            _context.Entry(registeredStudent).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RegisteredStudentExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/RegisteredStudents
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<ImageDetails>> PostRegisteredStudent(string filename, string room, string auth)
        {
            if (auth != _auth)
            {

                return BadRequest("Invalid auth token");
            }

            const string IMAGE_BASE_URL = Constants.IMAGE_BASE_URL;
            const string IMAGE_NEW_URL = Constants.IMAGE_NEW_URL;

            const string SUBSCRIPTION_KEY = Constants.SUBSCRIPTION_KEY;
            const string ENDPOINT = Constants.ENDPOINT;
            const string RECOGNITION_MODEL3 = RecognitionModel.Recognition03;
            
            //find class from room 
            var Class = (from c in _context.Classes where c.Room == room select c).Single();
            //remove when python upload is created
            //BlobStorage.UploadAFile("newimage", filename);





            var students = await GetStudentsAsync(Class.ID);

            // Authenticate.
            var client = FaceRecognition.Authenticate(ENDPOINT, SUBSCRIPTION_KEY);
            var face = new FaceRecognition();
            // Find Similar - find a similar face from a list of faces.
            face.FindSimilar(client, IMAGE_BASE_URL, IMAGE_NEW_URL, RECOGNITION_MODEL3, students,filename).Wait();
            BlobStorage.DeleteAFile("newimage", filename);

            ImageDetails matchedImage = new ImageDetails();


            matchedImage.url = face.url;
            if (matchedImage.url == null)
            {
                matchedImage.notFound = true;
                System.IO.File.Delete(("./wwwroot/image/" + filename));
                return matchedImage;

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
                ClassID = Class.ID,
                StudentID = matchedImage.ID,
                RegisteredTime = DateTime.Now,



            };

            //check if student already registered 
            var checkReg = (from r in _context.RegisteredStudents where r.ClassID == rs.ClassID && r.StudentID == rs.StudentID select r).ToList();
            if (checkReg.Count > 0)
            {
                return (matchedImage);
            }

            _context.RegisteredStudents.Add(rs);
            await _context.SaveChangesAsync();

            return (matchedImage);
        }

       
        // DELETE: api/RegisteredStudents/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<RegisteredStudent>> DeleteRegisteredStudent(int id, string auth)
        {
            if (auth != _auth)
            {

                return BadRequest("Invalid auth token");
            }
            var registeredStudent = await _context.RegisteredStudents.FindAsync(id);
            if (registeredStudent == null)
            {
                return NotFound();
            }

            
            _context.RegisteredStudents.Remove(registeredStudent);
            await _context.SaveChangesAsync();

            return registeredStudent;
        }

        private bool RegisteredStudentExists(int id)
        {
            return _context.RegisteredStudents.Any(e => e.ID == id);
        }
    }
}
