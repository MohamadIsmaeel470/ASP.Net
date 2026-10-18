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
    public class BranchesController : ControllerBase
    {
        private readonly RentACar_DbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<BranchesController> _logger;

        public BranchesController(RentACar_DbContext context, IMapper mapper, ILogger<BranchesController> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Get all branches.
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<BranchDto>>> GetBranches()
        {
            try
            {
                var branches = await _context.Branch.Include(b => b.Address).ToListAsync();
                return Ok(_mapper.Map<IEnumerable<BranchDto>>(branches));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(500, "Something went wrong, please try again later.");
            }
        }

        /// <summary>
        /// Get branch by id.
        /// </summary>
        [HttpGet("{id}", Name = "GetBranch")]
        [AllowAnonymous]
        public async Task<ActionResult<BranchDto>> GetBranch(int id)
        {
            try
            {
                var branch = await _context.Branch.Include(b => b.Address).FirstOrDefaultAsync(b => b.BranchID == id);
                if (branch == null)
                    return NotFound();

                return Ok(_mapper.Map<BranchDto>(branch));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(500, "Something went wrong, please try again later.");
            }
        }

        /// <summary>
        /// Create a new branch. Authentication is required.
        /// </summary>
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<BranchDto>> CreateBranch(BranchForCreation branchForCreation)
        {
            try
            {
                var branch = _mapper.Map<Branch>(branchForCreation);
                branch.Address = new Address
                {
                    City = branchForCreation.City,
                    Stret = branchForCreation.Stret
                };

                _context.Branch.Add(branch);
                await _context.SaveChangesAsync();

                var branchDto = _mapper.Map<BranchDto>(branch);
                return CreatedAtRoute("GetBranch", new { id = branch.BranchID }, branchDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(500, "Something went wrong, please try again later.");
            }
        }

        /// <summary>
        /// Update an existing branch. Authentication is required.
        /// </summary>
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateBranch(int id, BranchForUpdate branchForUpdate)
        {
            try
            {
                var branch = await _context.Branch.Include(b => b.Address).FirstOrDefaultAsync(b => b.BranchID == id);
                if (branch == null)
                    return NotFound();

                _mapper.Map(branchForUpdate, branch);
                branch.Address.City = branchForUpdate.City;
                branch.Address.Stret = branchForUpdate.Stret;

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
        /// Delete a branch. Authentication is required.
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteBranch(int id)
        {
            try
            {
                var branch = await _context.Branch.FirstOrDefaultAsync(b => b.BranchID == id);
                if (branch == null)
                    return NotFound();

                _context.Branch.Remove(branch);
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
