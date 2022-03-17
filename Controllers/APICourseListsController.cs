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
    public class APICourseListsController : ControllerBase
    {
        private readonly CourseContext _context;
        private string _auth = Constants.AUTH;

        public APICourseListsController(CourseContext context)
        {
            _context = context;
        }

        // GET: api/APICourseLists
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CourseList>>> GetCourseLists( string auth)
        {
            
            if (auth != _auth)
            {

                return BadRequest("Invalid auth token");
            }
            return await _context.CourseLists.ToListAsync();
        }

        // GET: api/APICourseLists/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CourseList>> GetCourseList(int id, string auth)
        {

            if (auth != _auth)
            {

                return BadRequest("Invalid auth token");
            }

            var courseList = await _context.CourseLists.FindAsync(id);

            if (courseList == null)
            {
                return NotFound();
            }

            return courseList;
        }

        // PUT: api/APICourseLists/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCourseList(int id, CourseList courseList, string auth)
        {
            if (auth != _auth)
            {

                return BadRequest("Invalid auth token");
            }

            if (id != courseList.ID)
            {
                return BadRequest();
            }

            _context.Entry(courseList).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CourseListExists(id))
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

        // POST: api/APICourseLists
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<CourseList>> PostCourseList(CourseList courseList, string auth)
        {
            if (auth != _auth)
            {

                return BadRequest("Invalid auth token");
            }
            _context.CourseLists.Add(courseList);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCourseList", new { id = courseList.ID }, courseList);
        }

        // DELETE: api/APICourseLists/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<CourseList>> DeleteCourseList(int id, string auth)
        {
            if (auth != _auth)
            {

                return BadRequest("Invalid auth token");
            }
            var courseList = await _context.CourseLists.FindAsync(id);
            if (courseList == null)
            {
                return NotFound();
            }

            _context.CourseLists.Remove(courseList);
            await _context.SaveChangesAsync();

            return courseList;
        }

        private bool CourseListExists(int id)
        {
            return _context.CourseLists.Any(e => e.ID == id);
        }
    }
}
