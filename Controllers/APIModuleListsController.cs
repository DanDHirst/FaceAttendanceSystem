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
    public class APIModuleListsController : ControllerBase
    {
        private readonly CourseContext _context;

        public APIModuleListsController(CourseContext context)
        {
            _context = context;
        }

        // GET: api/APIModuleLists
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ModuleList>>> GetModuleLists()
        {
            return await _context.ModuleLists.ToListAsync();
        }

        // GET: api/APIModuleLists/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ModuleList>> GetModuleList(int id)
        {
            var moduleList = await _context.ModuleLists.FindAsync(id);

            if (moduleList == null)
            {
                return NotFound();
            }

            return moduleList;
        }

        // PUT: api/APIModuleLists/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutModuleList(int id, ModuleList moduleList)
        {
            if (id != moduleList.ID)
            {
                return BadRequest();
            }

            _context.Entry(moduleList).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ModuleListExists(id))
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

        // POST: api/APIModuleLists
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<ModuleList>> PostModuleList(ModuleList moduleList)
        {
            _context.ModuleLists.Add(moduleList);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetModuleList", new { id = moduleList.ID }, moduleList);
        }

        // DELETE: api/APIModuleLists/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ModuleList>> DeleteModuleList(int id)
        {
            var moduleList = await _context.ModuleLists.FindAsync(id);
            if (moduleList == null)
            {
                return NotFound();
            }

            _context.ModuleLists.Remove(moduleList);
            await _context.SaveChangesAsync();

            return moduleList;
        }

        private bool ModuleListExists(int id)
        {
            return _context.ModuleLists.Any(e => e.ID == id);
        }
    }
}
