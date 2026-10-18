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
    public class VehiclesController : ControllerBase
    {
        private readonly RentACar_DbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<VehiclesController> _logger;

        public VehiclesController(RentACar_DbContext context, IMapper mapper, ILogger<VehiclesController> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Get all vehicles.
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<VehicleDto>>> GetVehicles()
        {
            try
            {
                var vehicles = await _context.Vehicle.ToListAsync();
                return Ok(_mapper.Map<IEnumerable<VehicleDto>>(vehicles));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(500, "Something went wrong, please try again later.");
            }
        }

        /// <summary>
        /// Get vehicle by id.
        /// </summary>
        [HttpGet("{id}", Name = "GetVehicle")]
        [AllowAnonymous]
        public async Task<ActionResult<VehicleDto>> GetVehicle(int id)
        {
            try
            {
                var vehicle = await _context.Vehicle.FirstOrDefaultAsync(v => v.VehicleID == id);
                if (vehicle == null)
                    return NotFound();

                return Ok(_mapper.Map<VehicleDto>(vehicle));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(500, "Something went wrong, please try again later.");
            }
        }

        /// <summary>
        /// Get all vehicles rented within a date range.
        /// </summary>
        [HttpGet("rented")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<VehicleDto>>> GetRentedVehicles([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            try
            {
                if (startDate > endDate)
                    return BadRequest("Start date must be before end date.");

                var vehicles = await _context.Rental
                    .Where(r => r.StartDate <= endDate && r.EndDate >= startDate)
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
        /// Create a new vehicle. Authentication is required.
        /// </summary>
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<VehicleDto>> CreateVehicle(VehicleForCreation vehicleForCreation)
        {
            try
            {
                var branch = await _context.Branch.FirstOrDefaultAsync(b => b.BranchID == vehicleForCreation.BranchID);
                var vehicleType = await _context.VehicleType.FirstOrDefaultAsync(v => v.VehicleTypeID == vehicleForCreation.VehicleTypeID);

                if (branch == null || vehicleType == null)
                    return BadRequest("Branch or VehicleType does not exist.");

                var vehicle = _mapper.Map<Vehicle>(vehicleForCreation);
                _context.Vehicle.Add(vehicle);
                await _context.SaveChangesAsync();

                var vehicleDto = _mapper.Map<VehicleDto>(vehicle);
                return CreatedAtRoute("GetVehicle", new { id = vehicle.VehicleID }, vehicleDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(500, "Something went wrong, please try again later.");
            }
        }

        /// <summary>
        /// Update an existing vehicle. Authentication is required.
        /// </summary>
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateVehicle(int id, VehicleForUpdate vehicleForUpdate)
        {
            try
            {
                var vehicle = await _context.Vehicle.FirstOrDefaultAsync(v => v.VehicleID == id);
                if (vehicle == null)
                    return NotFound();

                var branch = await _context.Branch.FirstOrDefaultAsync(b => b.BranchID == vehicleForUpdate.BranchID);
                var vehicleType = await _context.VehicleType.FirstOrDefaultAsync(v => v.VehicleTypeID == vehicleForUpdate.VehicleTypeID);

                if (branch == null || vehicleType == null)
                    return BadRequest("Branch or VehicleType does not exist.");

                _mapper.Map(vehicleForUpdate, vehicle);
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
        /// Delete a vehicle. Authentication is required.
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteVehicle(int id)
        {
            try
            {
                var vehicle = await _context.Vehicle.FirstOrDefaultAsync(v => v.VehicleID == id);
                if (vehicle == null)
                    return NotFound();

                _context.Vehicle.Remove(vehicle);
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
