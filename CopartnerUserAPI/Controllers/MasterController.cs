using CopartnerUser.DataAccessLayer.Models;
using CopartnerUser.DataAccessLayer.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CopartnerUserAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MasterController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IMongoRepository<CallAvailability> _repositoryCallAvailability;

        public MasterController(IMongoRepository<CallAvailability> repositoryCallAvailability, ILogger<MasterController> logger)
        {
            _repositoryCallAvailability = repositoryCallAvailability;
            _logger = logger; 
        }


        [HttpPost("addTimeSlot")]
        public async Task AddTimeSlot(CallAvailability callAvailability)
        {
            var slots = new CallAvailability()
            {
                Time = callAvailability.Time,
                AMPM = callAvailability.AMPM,
                isActive = callAvailability.isActive
            };
            await _repositoryCallAvailability.InsertOneAsync(slots);
        }

        [HttpGet("getAllTimeSlots")]
        public IActionResult GetAllTimeSlot()
        {
            try
            {
                var alltimeslots = _repositoryCallAvailability.AsQueryable().ToList();
                return Ok(alltimeslots);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to retrieve timeslots.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to retrieve experts.");
            }
        }

        [HttpGet("getTimeSlot/{id}")]
        public IActionResult GetTimeSlotById(string id)
        {
            try
            {
                var expert = _repositoryCallAvailability.FindById(id);
                if (expert == null)
                {
                    return NotFound("Expert not found.");
                }
                return Ok(expert);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to retrieve expert.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to retrieve expert.");
            }
        }

        [HttpPut("updateTimeSlot/{id}")]
        public async Task<IActionResult> UpdateTimeSlot(string id, CallAvailability callAvailability)
        {
            try
            {
                var existingExpert = _repositoryCallAvailability.FindById(id);
                if (existingExpert == null)
                {
                    return NotFound("Expert not found.");
                }
                callAvailability.Id = existingExpert.Id; // Ensure the ID remains the same
                await _repositoryCallAvailability.ReplaceOneAsync(callAvailability);
                return Ok("Expert updated successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update expert.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to update expert.");
            }
        }

        [HttpDelete("deleteExpert/{id}")]
        public async Task<IActionResult> DeleteTimeSlot(string id)
        {
            try
            {
                var existingExpert = _repositoryCallAvailability.FindById(id);
                if (existingExpert == null)
                {
                    return NotFound("Expert not found.");
                }
                await _repositoryCallAvailability.DeleteByIdAsync(id);
                return Ok("Expert deleted successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to delete expert.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to delete expert.");
            }
        }
    }
}
