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
    public class APIClassesController : ControllerBase
    {
        private readonly CourseContext _context;
        private string _auth = Constants.AUTH;

        public APIClassesController(CourseContext context)
        {
            _context = context;
        }

        // GET: api/APIClasses
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Class>>> GetClasses( string auth)
        {
            if (auth != _auth)
            {

                return BadRequest("Invalid auth token");
            }
            return await _context.Classes.ToListAsync();
        }

        // GET: api/APIClasses/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Class>> GetClass(int id, string auth)
        {
            if (auth != _auth)
            {

                return BadRequest("Invalid auth token"); 
            }


            var @class = await _context.Classes.FindAsync(id);

            if (@class == null)
            {
                return NotFound();
            }

            return @class;
        }

        // PUT: api/APIClasses/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutClass(int id, Class @class, string auth)
        {

            if (auth != _auth)
            {

                return BadRequest("Invalid auth token");
            }

            if (id != @class.ID)
            {
                return BadRequest();
            }

            _context.Entry(@class).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClassExists(id))
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

        // POST: api/APIClasses
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Class>> PostClass(Class @class, string auth)
        {
            if (auth != _auth)
            {

                return BadRequest("Invalid auth token");
            }

            _context.Classes.Add(@class);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetClass", new { id = @class.ID }, @class);
        }

        // DELETE: api/APIClasses/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Class>> DeleteClass(int id,string auth)
        {
            if (auth != _auth)
            {

                return BadRequest("Invalid auth token");
            }

            var @class = await _context.Classes.FindAsync(id);
            if (@class == null)
            {
                return NotFound();
            }

            _context.Classes.Remove(@class);
            await _context.SaveChangesAsync();

            return @class;
        }

        private bool ClassExists(int id)
        {
            return _context.Classes.Any(e => e.ID == id);
        }
    }
}
