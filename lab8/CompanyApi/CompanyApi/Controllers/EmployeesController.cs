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
    public class EmployeesController : ControllerBase
    {
        private readonly CompanyDbContext _context;

        public EmployeesController(CompanyDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeDTO>>> GetEmployee()
        {
            return await _context.Employees
                .Select(e => EmployeeToDTO(e))
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EmployeeDTO>> GetEmployee(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
                return NotFound();

            return EmployeeToDTO(employee);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmployee(int id, EmployeeDTO employeeDTO)
        {
            if (id != employeeDTO.EmployeeId)
                return BadRequest();

            _context.Entry(DTOToEmployee(employeeDTO)).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeExists(id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<EmployeeDTO>> PostEmployee(EmployeeDTO employeeDTO)
        {
            _context.Employees.Add(DTOToEmployee(employeeDTO));
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetEmployee), new { id = employeeDTO.EmployeeId }, employeeDTO);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<EmployeeDTO>> DeleteEmployee(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
                return NotFound();

            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();

            return EmployeeToDTO(employee);
        }

        private static EmployeeDTO EmployeeToDTO(Employee e) =>
            new EmployeeDTO
            {
                EmployeeId = e.EmployeeId,
                FirstName = e.FirstName,
                LastName = e.LastName,
                ManagerId = e.ManagerId,
                Salary = e.Salary,
                Bonus = e.Bonus,
                DepartmentId = e.DepartmentId
            };

        private static Employee DTOToEmployee(EmployeeDTO dto) =>
            new Employee
            {
                EmployeeId = dto.EmployeeId,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                ManagerId = dto.ManagerId,
                Salary = dto.Salary,
                Bonus = dto.Bonus,
                DepartmentId = dto.DepartmentId
            };

        private bool EmployeeExists(int id) =>
            _context.Employees.Any(e => e.EmployeeId == id);
    }
}
