using CopartnerUser.Common.Models;
using CopartnerUser.ServiceLayer.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace CopartnerUserAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> AddUser([FromBody] User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var id = await _userService.AddUserAsync(user);
            if (id == 0)
            {
                return BadRequest("Failed to add user.");
            }

            user.Id = id; // Set the Id property of the user object
            return CreatedAtAction(nameof(GetUserById), new { id }, user);
        }

        [HttpPut()]
        public async Task<IActionResult> UpdateUser([FromBody] User user)
        {
            if (user.Id <= 0)
            {
                return BadRequest("User Id is required.");
            }

            var result = await _userService.UpdateUserAsync(user);
            if (!result)
            {
                return NotFound();
            }

            // Serialize the updated user object to a JSON string
            var updatedUserJson = JsonSerializer.Serialize(user);

            // Add the serialized user object to the response headers
            Response.Headers.Add("X-Updated-User", updatedUserJson);

            return NoContent(); // or return Ok() if you prefer
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var success = await _userService.DeleteUserAsync(id);
            if (!success)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
