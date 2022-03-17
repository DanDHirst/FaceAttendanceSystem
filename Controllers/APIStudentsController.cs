using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FaceAttendance.Data;
using FaceAttendance.Models;

namespace FaceAttendance.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class APIStudentsController : ControllerBase
    {
        private readonly CourseContext _context;
        private string _auth = Constants.AUTH;

        public APIStudentsController(CourseContext context)
        {
            _context = context;
        }

        // GET: api/APIStudents
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Student>>> GetStudents(string auth)
        {
            if (auth != _auth)
            {

                return BadRequest("Invalid auth token");
            }
            return await _context.Students.ToListAsync();
        }

        // GET: api/APIStudents/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Student>> GetStudent(int id, string auth)
        {
            if (auth != _auth)
            {

                return BadRequest("Invalid auth token");
            }
            var student = await _context.Students.FindAsync(id);

            if (student == null)
            {
                return NotFound();
            }

            return student;
        }

        // PUT: api/APIStudents/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStudent(int id, Student student, string auth)
        {
            if (auth != _auth)
            {

                return BadRequest("Invalid auth token");
            }
            if (id != student.ID)
            {
                return BadRequest();
            }

            _context.Entry(student).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudentExists(id))
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

        // POST: api/APIStudents
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Student>> PostStudent(Student student, string auth)
        {
            if (auth != _auth)
            {

                return BadRequest("Invalid auth token");
            }
            _context.Students.Add(student);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetStudent", new { id = student.ID }, student);
        }

        // DELETE: api/APIStudents/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Student>> DeleteStudent(int id, string auth)
        {
            if (auth != _auth)
            {

                return BadRequest("Invalid auth token");
            }
            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }

            _context.Students.Remove(student);
            await _context.SaveChangesAsync();

            return student;
        }

        private bool StudentExists(int id)
        {
            return _context.Students.Any(e => e.ID == id);
        }
    }
}
