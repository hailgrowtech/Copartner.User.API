using CopartnerUser.DataAccessLayer.Models;
using CopartnerUser.DataAccessLayer.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CopartnerUserAPI.Controllers
{
    [Route("api/")]
    [ApiController]
    public class MasterController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IMongoRepository<CallAvailability> _repositoryCallAvailability;
        private readonly IMongoRepository<Sequence> _repositorySequence;

        public MasterController(ILogger<MasterController> logger, IMongoRepository<CallAvailability> repositoryCallAvailability, IMongoRepository<Sequence> repositorySequence)
        {
            _logger = logger;
            _repositoryCallAvailability = repositoryCallAvailability;
            _repositorySequence = repositorySequence;
        }

        private async Task<int> GetNextSequenceValue(string sequenceName)
        {
            return await _repositorySequence.GetNextSequenceValue(sequenceName);
        }
        
        [HttpPost()]
        [Route(APIRoutes.AddTimeSlots)]
        public async Task AddTimeSlot(CallAvailability callAvailability)
        {
            var slots = new CallAvailability()
            {
                CallAvailabilityId = await GetNextSequenceValue("CallAvailabilityId"),
                Time = callAvailability.Time,
                AMPM = callAvailability.AMPM,
                isActive = callAvailability.isActive
            };
            await _repositoryCallAvailability.InsertOneAsync(slots);
        }

        [HttpGet()]
        [Route(APIRoutes.GetAllTimeSlots)]
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

        [HttpGet()]
        [Route(APIRoutes.GetTimeSlotById)]
        public IActionResult GetTimeSlotById(int id)
        {
            try
            {
                var field = "CallAvailabilityId";
                var expert = _repositoryCallAvailability.FindById(id, field);
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

        [HttpPut()]
        [Route(APIRoutes.UpdateTimeSlot)]
        public async Task<IActionResult> UpdateTimeSlot(int id, CallAvailability callAvailability)
        {
            try
            {
                var field = "CallAvailabilityId";
                var existingExpert = _repositoryCallAvailability.FindById(id, field);
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

        [HttpDelete()]
        [Route(APIRoutes.DeleteTimeSlot)]
        public async Task<IActionResult> DeleteTimeSlot(int id)
        {
            try
            {
                var field = "CallAvailabilityId";
                var existingExpert = _repositoryCallAvailability.FindById(id, field);
                if (existingExpert == null)
                {
                    return NotFound("Expert not found.");
                }
                await _repositoryCallAvailability.DeleteByIdAsync(id, field);
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
