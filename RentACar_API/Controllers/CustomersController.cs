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
    public class CustomersController : ControllerBase
    {
        private readonly RentACar_DbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<CustomersController> _logger;

        public CustomersController(RentACar_DbContext context, IMapper mapper, ILogger<CustomersController> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Get all customers.
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<CustomerDto>>> GetCustomers()
        {
            try
            {
                var customers = await _context.Customer.ToListAsync();
                return Ok(_mapper.Map<IEnumerable<CustomerDto>>(customers));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(500, "Something went wrong, please try again later.");
            }
        }

        /// <summary>
        /// Get customer by id.
        /// </summary>
        [HttpGet("{id}", Name = "GetCustomer")]
        [AllowAnonymous]
        public async Task<ActionResult<CustomerDto>> GetCustomer(int id)
        {
            try
            {
                var customer = await _context.Customer.FirstOrDefaultAsync(c => c.CustomerID == id);
                if (customer == null)
                    return NotFound();

                return Ok(_mapper.Map<CustomerDto>(customer));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(500, "Something went wrong, please try again later.");
            }
        }

        /// <summary>
        /// Get all vehicles rented previously or currently by a customer.
        /// </summary>
        [HttpGet("{customerId}/vehicles")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<VehicleDto>>> GetCustomerVehicles(int customerId)
        {
            try
            {
                var customer = await _context.Customer.FirstOrDefaultAsync(c => c.CustomerID == customerId);
                if (customer == null)
                    return NotFound();

                var vehicles = await _context.Rental
                    .Where(r => r.CustomerID == customerId)
                    .Select(r => r.Vehicle)
                    .Distinct()
                    .ToListAsync();

                return Ok(_mapper.Map<IEnumerable<VehicleDto>>(vehicles));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(500, "Something went wrong, please try again later.");
            }
        }

        /// <summary>
        /// Create a new customer. Authentication is required.
        /// </summary>
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<CustomerDto>> CreateCustomer(CustomerForCreation customerForCreation)
        {
            try
            {
                var customer = _mapper.Map<Customer>(customerForCreation);
                _context.Customer.Add(customer);
                await _context.SaveChangesAsync();

                var customerDto = _mapper.Map<CustomerDto>(customer);
                return CreatedAtRoute("GetCustomer", new { id = customer.CustomerID }, customerDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(500, "Something went wrong, please try again later.");
            }
        }

        /// <summary>
        /// Update an existing customer. Authentication is required.
        /// </summary>
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateCustomer(int id, CustomerForUpdate customerForUpdate)
        {
            try
            {
                var customer = await _context.Customer.FirstOrDefaultAsync(c => c.CustomerID == id);
                if (customer == null)
                    return NotFound();

                _mapper.Map(customerForUpdate, customer);
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
        /// Delete a customer. Authentication is required.
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            try
            {
                var customer = await _context.Customer.FirstOrDefaultAsync(c => c.CustomerID == id);
                if (customer == null)
                    return NotFound();

                _context.Customer.Remove(customer);
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
