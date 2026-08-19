using FreelanceAPI.Requests;
using FreelanceAPI.Responses;
using FreelanceAPI.Services.Interface;
using FreelanceMarketplace.API.Enums;
using M03.RepositoryPattern.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FreelanceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService userService;

        public UserController(IUserService userService)
        {
            this.userService = userService;
        }
       
 

        [HttpGet("count", Name = "GetUserCount")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<int>> GetCount()
        {
            var count = await userService.GetUsersCountAsync();
            return Ok(count);
        }

        [HttpGet("GetUsersInPages")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PagedResult<UserResponse>>> GetPage(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            CancellationToken ct = default)
        {
            if (page < 1)
                return BadRequest("Page must be greater than 0.");

            if (pageSize < 1 || pageSize > 100)
                return BadRequest("Page size must be between 1 and 100.");

            var users = await userService.GetUsersPageAsync(page, pageSize, ct);
            return Ok(users);
        }

        [HttpGet("{id}/get", Name = "GetUserById")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserResponse>> GetById(
            string id,
            CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(id))
                return BadRequest("User id is required.");

            var user = await userService.GetUserByIdAsync(id, ct);
            return Ok(user);
        }

        [HttpGet("{id}/services", Name = "GetUserServices")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<ServiceResponse>>> GetUserServices(
            string id,
            CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(id))
                return BadRequest("User id is required.");

            var services = await userService.GetUserServicesAsync(id, ct);
            return Ok(services);
        }

        [HttpGet("{id}/orders", Name = "GetUserOrders")]
        [Authorize(Roles = "Admin,Client")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<OrderResponse>>> GetUserOrders(
            string id,
            CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(id))
                return BadRequest("User id is required.");

            var orders = await userService.GetUserOrdersAsync(id, ct);
            return Ok(orders);
        }

        [HttpGet("search", Name = "SearchByName")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<UserResponse>>> Search(
            [FromQuery] string keyword,
            CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(keyword))
                return BadRequest("Keyword is required.");

            var users = await userService.SearchUsersAsync(keyword.Trim(), ct);
            return Ok(users);
        }

        [HttpGet("{id}/exists", Name = "UserExists")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<bool>> Exists(
            string id,
            CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(id))
                return BadRequest("User id is required.");

            var exists = await userService.ExistsByIdAsync(id, ct);
            return Ok(exists);
        }
        [HttpPut("{userId}", Name = "UpdateUser")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(
      string userId,
      UpdateUser request,
      CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(userId))
                return BadRequest("User ID is required.");

            if (request is null)
                return BadRequest("Request cannot be null.");

            var result = await userService.UpdateUserAsync(userId, request, ct);

            return result ? NoContent() : BadRequest("Failed to update user.");
        }
    }
}
