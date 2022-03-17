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
    public class APIModulesController : ControllerBase
    {
        private readonly CourseContext _context;
        private string _auth = Constants.AUTH;

        public APIModulesController(CourseContext context)
        {
            _context = context;
        }

        // GET: api/APIModules
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Module>>> GetModules(string auth)
        {
            if (auth != _auth)
            {

                return BadRequest("Invalid auth token");
            }
            return await _context.Modules.ToListAsync();
        }

        // GET: api/APIModules/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Module>> GetModule(int id, string auth)
        {
            if (auth != _auth)
            {

                return BadRequest("Invalid auth token");
            }
            var @module = await _context.Modules.FindAsync(id);

            if (@module == null)
            {
                return NotFound();
            }

            return @module;
        }

        // PUT: api/APIModules/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutModule(int id, Module @module, string auth)
        {
            if (auth != _auth)
            {

                return BadRequest("Invalid auth token");
            }
            if (id != @module.ID)
            {
                return BadRequest();
            }

            _context.Entry(@module).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ModuleExists(id))
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

        // POST: api/APIModules
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Module>> PostModule(Module @module, string auth)
        {
            if (auth != _auth)
            {

                return BadRequest("Invalid auth token");
            }
            _context.Modules.Add(@module);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetModule", new { id = @module.ID }, @module);
        }

        // DELETE: api/APIModules/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Module>> DeleteModule(int id, string auth)
        {
            if (auth != _auth)
            {

                return BadRequest("Invalid auth token");
            }
            var @module = await _context.Modules.FindAsync(id);
            if (@module == null)
            {
                return NotFound();
            }

            _context.Modules.Remove(@module);
            await _context.SaveChangesAsync();

            return @module;
        }

        private bool ModuleExists(int id)
        {
            return _context.Modules.Any(e => e.ID == id);
        }
    }
}
