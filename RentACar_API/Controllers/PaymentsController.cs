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
    public class PaymentsController : ControllerBase
    {
        private readonly RentACar_DbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<PaymentsController> _logger;

        public PaymentsController(RentACar_DbContext context, IMapper mapper, ILogger<PaymentsController> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Get all payments.
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<PaymentDto>>> GetPayments()
        {
            try
            {
                var payments = await _context.Payment.ToListAsync();
                return Ok(_mapper.Map<IEnumerable<PaymentDto>>(payments));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(500, "Something went wrong, please try again later.");
            }
        }

        /// <summary>
        /// Get payment by id.
        /// </summary>
        [HttpGet("{id}", Name = "GetPayment")]
        [AllowAnonymous]
        public async Task<ActionResult<PaymentDto>> GetPayment(int id)
        {
            try
            {
                var payment = await _context.Payment.FirstOrDefaultAsync(p => p.PaymentID == id);
                if (payment == null)
                    return NotFound();

                return Ok(_mapper.Map<PaymentDto>(payment));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(500, "Something went wrong, please try again later.");
            }
        }

        /// <summary>
        /// Calculate company revenue from payments within a date range.
        /// </summary>
        [HttpGet("revenue")]
        [AllowAnonymous]
        public async Task<ActionResult<double>> GetRevenue([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            try
            {
                if (startDate > endDate)
                    return BadRequest("Start date must be before end date.");

                var revenue = await _context.Payment
                    .Where(p => p.CreatedDate >= startDate && p.CreatedDate <= endDate)
                    .SumAsync(p => p.Amount);

                return Ok(revenue);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(500, "Something went wrong, please try again later.");
            }
        }

        /// <summary>
        /// Create a new payment. Authentication is required.
        /// </summary>
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<PaymentDto>> CreatePayment(PaymentForCreation paymentForCreation)
        {
            try
            {
                var rental = await _context.Rental.FirstOrDefaultAsync(r => r.RentalId == paymentForCreation.RentalID);
                if (rental == null)
                    return BadRequest("Rental does not exist.");

                var payment = _mapper.Map<Payment>(paymentForCreation);
                payment.CreatedDate = DateTime.UtcNow;

                _context.Payment.Add(payment);
                await _context.SaveChangesAsync();

                var paymentDto = _mapper.Map<PaymentDto>(payment);
                return CreatedAtRoute("GetPayment", new { id = payment.PaymentID }, paymentDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(500, "Something went wrong, please try again later.");
            }
        }

        /// <summary>
        /// Update an existing payment. Authentication is required.
        /// </summary>
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdatePayment(int id, PaymentForUpdate paymentForUpdate)
        {
            try
            {
                var payment = await _context.Payment.FirstOrDefaultAsync(p => p.PaymentID == id);
                if (payment == null)
                    return NotFound();

                var rental = await _context.Rental.FirstOrDefaultAsync(r => r.RentalId == paymentForUpdate.RentalID);
                if (rental == null)
                    return BadRequest("Rental does not exist.");

                _mapper.Map(paymentForUpdate, payment);
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
        /// Delete a payment. Authentication is required.
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeletePayment(int id)
        {
            try
            {
                var payment = await _context.Payment.FirstOrDefaultAsync(p => p.PaymentID == id);
                if (payment == null)
                    return NotFound();

                _context.Payment.Remove(payment);
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
