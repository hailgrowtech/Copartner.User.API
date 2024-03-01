using CopartnerUser.Common.Models;
using CopartnerUser.ServiceLayer.Services;
using CopartnerUser.ServiceLayer.Services.Interfaces;
using CopartnerUser.ServiceLayer.UserService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CopartnerUserAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AvailabilityController : ControllerBase
    {
        private readonly IAvailabilityTypesService _availabilityTypeService;

        public AvailabilityController(IAvailabilityTypesService availabilityTypeService)
        {
            _availabilityTypeService = availabilityTypeService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAvailabilityType()
        {
            var users = await _availabilityTypeService.GetAllAvailabilityTypesAsync();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAvailabilityTypeById(int id)
        {
            var user = await _availabilityTypeService.GetAvailabilityTypeByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> AddAvailabilityType([FromBody] AvailabilityTypes availabilityTypes)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var id = await _availabilityTypeService.AddAvailabilityTypeAsync(availabilityTypes);
            return CreatedAtAction(nameof(GetAvailabilityTypeById), new { id }, availabilityTypes);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAvailabilityType(int id, [FromBody] AvailabilityTypes availabilityTypes)
        {
            if (id != availabilityTypes.Id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var success = await _availabilityTypeService.UpdateAvailabilityTypeAsync(availabilityTypes);
            if (!success)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAvailabilityType(int id)
        {
            var success = await _availabilityTypeService.DeleteAvailabilityTypeAsync(id);
            if (!success)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
