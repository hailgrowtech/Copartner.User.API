using CopartnerUser.DataAccessLayer.Models;
using CopartnerUser.DataAccessLayer.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using System.Linq.Expressions;

namespace CopartnerUserAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExpertsController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IMongoRepository<Experts> _repositoryExperts;
        private readonly IMongoRepository<Sequence> _repositorySequence;


        public ExpertsController(IMongoRepository<Experts> repositoryExperts, IMongoRepository<Sequence> repositorySequence, ILogger<ExpertsController> logger)
        {
            _repositoryExperts = repositoryExperts;
            _repositorySequence = repositorySequence;
            _logger = logger;
        }

        //private async Task<int> GetNextSequenceValue(string sequenceName)
        //{
        //    var filter = Builders<Sequence>.Filter.Eq(s => s.Name, sequenceName);
        //    var update = Builders<Sequence>.Update.Inc(s => s.Value, 1);
        //    var options = new FindOneAndUpdateOptions<Sequence>
        //    {
        //        IsUpsert = true,
        //        ReturnDocument = ReturnDocument.After
        //    };

        //    Expression<Func<Sequence, bool>> filterExpression = s => s.Name == sequenceName;
        //    var sequence = await _collection.FindOneAndUpdateAsync(filterExpression, update, options);
        //    return sequence.Value;
        //}

        private async Task<int> GetNextSequenceValue(string sequenceName)
        {
            return await _repositorySequence.GetNextSequenceValue(sequenceName);
        }


        //[HttpPost("registerExpert")]
        //public async Task<IActionResult> AddExpert(Experts expert)
        //{
        //    // Generate ExpertId
        //    expert.ExpertId = await GetNextSequenceValue("ExpertId");

        //    // Generate CallAvailabilityIds
        //    foreach (var availability in expert.CallAvailabilities)
        //    {
        //        availability.CallAvailabilityId = await GetNextSequenceValue("CallAvailabilityId");
        //    }

        //    await _repositoryExperts.InsertOneAsync(expert);
        //    return CreatedAtAction(nameof(GetExpert), new { id = expert.ExpertId }); // Remove the object expression here
        //}


        [HttpPost("registerExpert")]
        public async Task AddExpert(Experts experts)
        {
            var expert = new Experts()
            {
                ExpertId = await GetNextSequenceValue("ExpertId"),
                FirstName = experts.FirstName,
                LastName = experts.LastName,
                Experience = experts.Experience,
                Followers = experts.Followers,
                ExpertType = experts.ExpertType,
                BioDescription = experts.BioDescription,
                Rating = experts.Rating,
                Pic = experts.Pic,
                CallAvailabilities = experts.CallAvailabilities,


            };
            await _repositoryExperts.InsertOneAsync(expert);
        }




        //    //var expert = new Experts()
        //    //{
        //    //    FirstName = experts.FirstName,
        //    //    LastName = experts.LastName,
        //    //    Experience = experts.Experience,
        //    //    Followers = experts.Followers,
        //    //    ExpertType = experts.ExpertType,
        //    //    BioDescription = experts.BioDescription,
        //    //    Rating = experts.Rating,
        //    //    Pic = experts.Pic,
        //    //    CallAvailabilities = experts.CallAvailabilities,


        //    //};
        //    //await _repository.InsertOneAsync(expert);
        //}

        [HttpGet("getAllExperts")]
        public IActionResult GetAllExperts()
        {
            try
            {
                
                var allExperts = _repositoryExperts.AsQueryable().ToList();
                return Ok(allExperts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to retrieve experts.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to retrieve experts.");
            }
        }

        [HttpGet("getExpert/{id}")]
        public IActionResult GetExpert(int id)
        {
            try
            {
                var field = "ExpertId";
                var expert = _repositoryExperts.FindById(id, field);
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
        public async Task<IActionResult> UpdateExpert(int id, Experts expert)
        {
            try
            {
                var field = "ExpertId";
                var existingExpert = _repositoryExperts.FindById(id, field);
                if (existingExpert == null)
                {
                    return NotFound("Expert not found.");
                }
                expert.Id = existingExpert.Id; // Ensure the ID remains the same
                await _repositoryExperts.ReplaceOneAsync(expert);
                return Ok("Expert updated successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update expert.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to update expert.");
            }
        }

        [HttpDelete("deleteExpert/{id}")]
        public async Task<IActionResult> DeleteExpert(int id)
        {
            try
            {
                var field = "ExpertId";
                var existingExpert = _repositoryExperts.FindById(id, field);
                if (existingExpert == null)
                {
                    return NotFound("Expert not found.");
                }
                await _repositoryExperts.DeleteByIdAsync(id, field);
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
