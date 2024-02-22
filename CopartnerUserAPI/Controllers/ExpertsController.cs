using CopartnerUser.DataAccessLayer.Models;
using CopartnerUser.DataAccessLayer.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CopartnerUserAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExpertsController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IMongoRepository<Experts> _repository;


        public ExpertsController(IMongoRepository<Experts> repository, ILogger<ExpertsController> logger)
        {
            _repository = repository;
            _logger = logger;
        }


        [HttpPost("registerExpert")]
        public async Task AddExpert(Experts experts)
        {
            var expert = new Experts()
            {
                FirstName = experts.FirstName,
                LastName = experts.LastName,
                Experience = experts.Experience,
                Followers = experts.Followers,
                ExpertTypeId = experts.ExpertTypeId,
                BioDescription = experts.BioDescription,
                Rating = experts.Rating,
                Pic = experts.Pic,
                CallAvailabilityIds = experts.CallAvailabilityIds,


            };
            await _repository.InsertOneAsync(expert);
        }

        [HttpGet("getAllExperts")]
        public IActionResult GetAllExperts()
        {
            try
            {
                var allExperts = _repository.AsQueryable().ToList();
                return Ok(allExperts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to retrieve experts.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to retrieve experts.");
            }
        }

        [HttpGet("getExpert/{id}")]
        public IActionResult GetExpert(string id)
        {
            try
            {
                var expert = _repository.FindById(id);
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

        [HttpPut("updateExpert/{id}")]
        public async Task<IActionResult> UpdateExpert(string id, Experts expert)
        {
            try
            {
                var existingExpert = _repository.FindById(id);
                if (existingExpert == null)
                {
                    return NotFound("Expert not found.");
                }
                expert.Id = existingExpert.Id; // Ensure the ID remains the same
                await _repository.ReplaceOneAsync(expert);
                return Ok("Expert updated successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update expert.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to update expert.");
            }
        }

        [HttpDelete("deleteExpert/{id}")]
        public async Task<IActionResult> DeleteExpert(string id)
        {
            try
            {
                var existingExpert = _repository.FindById(id);
                if (existingExpert == null)
                {
                    return NotFound("Expert not found.");
                }
                await _repository.DeleteByIdAsync(id);
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
