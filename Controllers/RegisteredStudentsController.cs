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
    public class RegisteredStudentsController : ControllerBase
    {
        private readonly CourseContext _context;

        public RegisteredStudentsController(CourseContext context)
        {
            _context = context;
        }

        // GET: api/RegisteredStudents
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RegisteredStudent>>> GetRegisteredStudents()
        {
            return await _context.RegisteredStudents.ToListAsync();
        }

        // GET: api/RegisteredStudents/5
        [HttpGet("{id}")]
        public async Task<ActionResult<RegisteredStudent>> GetRegisteredStudent(int id)
        {
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
        public async Task<IActionResult> PutRegisteredStudent(int id, RegisteredStudent registeredStudent)
        {
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
        public async Task<ActionResult<RegisteredStudent>> PostRegisteredStudent(RegisteredStudent registeredStudent)
        {
            _context.RegisteredStudents.Add(registeredStudent);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRegisteredStudent", new { id = registeredStudent.ID }, registeredStudent);
        }

        // DELETE: api/RegisteredStudents/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<RegisteredStudent>> DeleteRegisteredStudent(int id)
        {
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
