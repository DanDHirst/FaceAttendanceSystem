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
    public class APILecturersController : ControllerBase
    {
        private readonly CourseContext _context;
        private string _auth = Constants.AUTH;

        public APILecturersController(CourseContext context)
        {
            _context = context;
        }

        // GET: api/APILecturers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Lecturer>>> GetLecturers(string auth)
        {
            if (auth != _auth)
            {

                return BadRequest("Invalid auth token");
            }
            return await _context.Lecturers.ToListAsync();
        }

        // GET: api/APILecturers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Lecturer>> GetLecturer(int id, string auth)
        {
            if (auth != _auth)
            {

                return BadRequest("Invalid auth token");
            }
            var lecturer = await _context.Lecturers.FindAsync(id);

            if (lecturer == null)
            {
                return NotFound();
            }

            return lecturer;
        }

        // PUT: api/APILecturers/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLecturer(int id, Lecturer lecturer, string auth)
        {
            if (auth != _auth)
            {

                return BadRequest("Invalid auth token");
            }
            if (id != lecturer.ID)
            {
                return BadRequest();
            }

            _context.Entry(lecturer).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LecturerExists(id))
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

        // POST: api/APILecturers
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Lecturer>> PostLecturer(Lecturer lecturer, string auth)
        {
            if (auth != _auth)
            {

                return BadRequest("Invalid auth token");
            }
            _context.Lecturers.Add(lecturer);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLecturer", new { id = lecturer.ID }, lecturer);
        }

        // DELETE: api/APILecturers/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Lecturer>> DeleteLecturer(int id, string auth)
        {
            if (auth != _auth)
            {

                return BadRequest("Invalid auth token");
            }
            var lecturer = await _context.Lecturers.FindAsync(id);
            if (lecturer == null)
            {
                return NotFound();
            }

            _context.Lecturers.Remove(lecturer);
            await _context.SaveChangesAsync();

            return lecturer;
        }

        private bool LecturerExists(int id)
        {
            return _context.Lecturers.Any(e => e.ID == id);
        }
    }
}
