using FreelanceAPI.Requests;
using FreelanceAPI.Services.implementation;
using FreelanceAPI.Services.Interface;
using FreelanceMarketplace.API.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FreelanceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceController : ControllerBase
    {
        private readonly IServiceService _serviceService;

        public ServiceController(IServiceService serviceService)
        {
            _serviceService = serviceService;
        }

        [HttpPost]
        [Authorize(Roles = "Seller")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Add(CreateServiceRequest request)
        {
            await _serviceService.AddServiceAsync(request);
            return Ok();
        }
        [HttpGet("{id}/getById",Name ="GetById")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> getById(int id,CancellationToken ct)
        {
            if (id <= 0)
                return BadRequest("Id Is not Correct");
            var service= await _serviceService.GetServiceByIdAsync(id); 
            return Ok(service);
        }
        [HttpGet("{KeyWord}/Serch",Name ="Search")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Search(string KeyWord,CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(KeyWord))
                return BadRequest("key Word is incorrect");
            var services=await _serviceService.SearchServicesAsync(KeyWord, ct);
            return Ok(services);
        }
        [HttpGet("{id}/BySellerId",Name ="SellerService")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> GetServiceBySellerId(string sellerId,CancellationToken ct)
        {
            if (!string.IsNullOrWhiteSpace(sellerId))
                return BadRequest("InCorrect Id");
            var Services = await _serviceService.GetServicesBySellerIdAsync(sellerId, ct);
            return Ok(Services);
        }
        [HttpGet("Servicecount")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Count()
        {
            var count = await _serviceService.GetServicesCountAsync();
            return Ok(count);
        }
        [HttpDelete("{serviceId}", Name = "DeleteService")]
        [Authorize(Roles = $"{nameof(UserRole.Admin)},{nameof(UserRole.Seller)}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(
      int serviceId,
      CancellationToken ct)
        {
            if (serviceId <= 0)
                return BadRequest("Service ID must be greater than 0.");

            await _serviceService.DeleteServiceAsync(serviceId, ct);

            return NoContent();
        }
    }
}
