#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Lab6.Data;
using Lab6.Models;

namespace Lab6.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly StudentDbContext _context;

        public StudentsController(StudentDbContext context)
        {
            _context = context;
        }

        // GET: api/Students
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)] //returns get when list of students is returned
        [ProducesResponseType(StatusCodes.Status500InternalServerError)] //returns error when students can't be found
        public async Task<ActionResult<IEnumerable<Student>>> GetStudents()
        {
            return Ok(await _context.Students.ToListAsync());
        }

        // GET: api/Students/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)] //returns 200 when get is successfully processed
        [ProducesResponseType(StatusCodes.Status404NotFound)] //returns 404 when the id doesn't exist in the database
        public async Task<ActionResult<Student>> GetStudent(Guid id)
        {
            var student = await _context.Students.FindAsync(id);

            if (student == null)
            {
                return NotFound(NotFound());
            }

            return Ok(student);
        }

        // PUT: api/Students/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)] //returns ok when put has a student to return
        [ProducesResponseType(StatusCodes.Status404NotFound)] //returns not found when theres no student to return
        [ProducesResponseType(StatusCodes.Status500InternalServerError)] //returns internal server error is theres a db error
        public async Task<IActionResult> PutStudent(Guid id, Student student)
        {
            if (id != student.ID)
            {
                return BadRequest();
            }

            _context.Entry(student).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                if (StudentExists(id))
                {
                    //var temp = student;
                    return Ok((IActionResult)student);
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudentExists(id))
                {
                    return NotFound(NotFound());
                }
                else
                {
                    throw;
                     
                }
            }

            return NoContent();
        }

        // POST: api/Students
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)] // returns ok when student is successfully added to db
        [ProducesResponseType(StatusCodes.Status500InternalServerError)] // returns 500 on error
        public async Task<ActionResult<Student>> PostStudent(Student student)
        {
            _context.Students.Add(student);
            await _context.SaveChangesAsync();

            return Ok(CreatedAtAction("GetStudent", new { id = student.ID }, student));
        }

        // DELETE: api/Students/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)] // returns ok when student is successfully delete from db context
        [ProducesResponseType(StatusCodes.Status404NotFound)] //returns not found when theres no student to delete
        [ProducesResponseType(StatusCodes.Status500InternalServerError)] // returns 500 on error
        public async Task<IActionResult> DeleteStudent(Guid id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }

            _context.Students.Remove(student);
            await _context.SaveChangesAsync();

            return Ok(NoContent());
        }

        private bool StudentExists(Guid id)
        {
            return _context.Students.Any(e => e.ID == id);
        }
    }
}
