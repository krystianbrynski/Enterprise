using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CompanyApi.Models;

namespace CompanyApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DepartmentsController : ControllerBase
    {
        private readonly CompanyDbContext _context;

        public DepartmentsController(CompanyDbContext context)
            => _context = context;

        // GET: api/Departments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DepartmentDTO>>> GetDepartments()
        {
            return await _context.Departments
                .Select(d => DepartmentToDTO(d))
                .ToListAsync();
        }

        // GET: api/Departments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DepartmentDTO>> GetDepartment(int id)
        {
            var dept = await _context.Departments.FindAsync(id);
            if (dept == null)
                return NotFound();
            return DepartmentToDTO(dept);
        }

        // PUT: api/Departments/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDepartment(int id, DepartmentDTO dto)
        {
            if (id != dto.DepartmentId)
                return BadRequest();

            _context.Entry(DTOToDepartment(dto)).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DepartmentExists(id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        // POST: api/Departments
        [HttpPost]
        public async Task<ActionResult<DepartmentDTO>> PostDepartment(DepartmentDTO dto)
        {
            _context.Departments.Add(DTOToDepartment(dto));
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetDepartment),
                new { id = dto.DepartmentId },
                dto);
        }

        // DELETE: api/Departments/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<DepartmentDTO>> DeleteDepartment(int id)
        {
            var dept = await _context.Departments.FindAsync(id);
            if (dept == null)
                return NotFound();

            _context.Departments.Remove(dept);
            await _context.SaveChangesAsync();

            return DepartmentToDTO(dept);
        }

        // --- Mapping methods ---

        private static DepartmentDTO DepartmentToDTO(Department dept) => new DepartmentDTO
        {
            DepartmentId = dept.DepartmentId,
            Name = dept.Name
        };

        private static Department DTOToDepartment(DepartmentDTO dto) => new Department
        {
            DepartmentId = dto.DepartmentId,
            Name = dto.Name
        };

        private bool DepartmentExists(int id)
            => _context.Departments.Any(e => e.DepartmentId == id);
    }
}
