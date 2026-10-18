using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentACar_API.Models;
using RentACar_Data;
using RentACar_Domin;

namespace RentACar_API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/[controller]")]
    public class EmployeesController : ControllerBase
    {
        private readonly RentACar_DbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<EmployeesController> _logger;

        public EmployeesController(RentACar_DbContext context, IMapper mapper, ILogger<EmployeesController> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Get all employees.
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<EmployeeDto>>> GetEmployees()
        {
            try
            {
                var employees = await _context.Employees.ToListAsync();
                return Ok(_mapper.Map<IEnumerable<EmployeeDto>>(employees));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(500, "Something went wrong, please try again later.");
            }
        }

        /// <summary>
        /// Get employee by id.
        /// </summary>
        [HttpGet("{id}", Name = "GetEmployee")]
        [AllowAnonymous]
        public async Task<ActionResult<EmployeeDto>> GetEmployee(int id)
        {
            try
            {
                var employee = await _context.Employees.FirstOrDefaultAsync(e => e.EmployeeID == id);
                if (employee == null)
                    return NotFound();

                return Ok(_mapper.Map<EmployeeDto>(employee));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(500, "Something went wrong, please try again later.");
            }
        }

        /// <summary>
        /// Create a new employee. Authentication is required.
        /// </summary>
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<EmployeeDto>> CreateEmployee(EmployeeForCreation employeeForCreation)
        {
            try
            {
                var branch = await _context.Branch.FirstOrDefaultAsync(b => b.BranchID == employeeForCreation.BranchID);
                if (branch == null)
                    return BadRequest("Branch does not exist.");

                var employee = _mapper.Map<Employee>(employeeForCreation);
                _context.Employees.Add(employee);
                await _context.SaveChangesAsync();

                var employeeDto = _mapper.Map<EmployeeDto>(employee);
                return CreatedAtRoute("GetEmployee", new { id = employee.EmployeeID }, employeeDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(500, "Something went wrong, please try again later.");
            }
        }

        /// <summary>
        /// Update an existing employee. Authentication is required.
        /// </summary>
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateEmployee(int id, EmployeeForUpdate employeeForUpdate)
        {
            try
            {
                var employee = await _context.Employees.FirstOrDefaultAsync(e => e.EmployeeID == id);
                if (employee == null)
                    return NotFound();

                var branch = await _context.Branch.FirstOrDefaultAsync(b => b.BranchID == employeeForUpdate.BranchID);
                if (branch == null)
                    return BadRequest("Branch does not exist.");

                _mapper.Map(employeeForUpdate, employee);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(500, "Something went wrong, please try again later.");
            }
        }

        /// <summary>
        /// Delete an employee. Authentication is required.
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            try
            {
                var employee = await _context.Employees.FirstOrDefaultAsync(e => e.EmployeeID == id);
                if (employee == null)
                    return NotFound();

                _context.Employees.Remove(employee);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(500, "Something went wrong, please try again later.");
            }
        }
    }
}
