using FreelanceAPI.Models.Enum;
using FreelanceAPI.Repositories.Interfaces;
using FreelanceAPI.Requests;
using FreelanceAPI.Responses;
using FreelanceAPI.Services.Interface;
using FreelanceMarketplace.API.Enums;
using M03.RepositoryPattern.Requests;
using M03.RepositoryPattern.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FreelanceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            this._orderService = orderService;
        }
      
        
        [HttpPost("Order")]
        [Authorize(Roles = "Client,Admin")]
        [ProducesResponseType(typeof(OrderResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<OrderResponse>> Add(
    CreateOrderRequest request,
    CancellationToken ct)
        {
            if (request is null)
                return BadRequest("Request cannot be null.");
            var order = await _orderService.AddOrderAsync(request, ct);

            return CreatedAtRoute(
                "GetOrderById", 
                order);
        }
        [HttpPost("Review")]
        [Authorize(Roles = "Client,Admin")]
        [ProducesResponseType(typeof(ReviewResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ReviewResponse>> AddReview(CreateOrderReviewRequest request)
        {
            if (request is null)
                return BadRequest("Request cannot be null.");
            var review = await _orderService.AddOrderReviewAsync(request);
            return Ok(review);

        }
        [HttpGet("{orderId:int}", Name = "GetOrderById")]
        
        [Authorize]
        [ProducesResponseType(typeof(OrderResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<OrderResponse>> GetById(int orderId)
        {
            var order = await _orderService.GetOrderByIdAsync(orderId);
            if (order == null)
                return NotFound();
            return Ok(order);
        }
        [HttpGet("{id}", Name = "GetPage")]
       
        [Authorize]
        [ProducesResponseType(typeof(OrderResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> GetPage(int page, int pagesize, CancellationToken ct)
        {
            if (page < 1)
                return BadRequest("Page must be greater than 0.");

            if (pagesize < 1 || pagesize > 100)
                return BadRequest("Page size must be between 1 and 100.");
            var users = await _orderService.GetOrdersPageAsync(page, pagesize, ct);
            return Ok(users);
        }
        [HttpGet("{id}/reviews", Name = "GetOrderReviews")]
    
        [Authorize]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> GetOrder(int orderId, CancellationToken ct)
        {
            if (orderId <= 0)
                return BadRequest("Order Is not true");
            var reviews = await _orderService.GetOrderReviewsAsync(orderId, ct);
            return Ok(reviews);
        }
        [HttpGet(Name = "CountOrder")]
       
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> GetCount(CancellationToken ct)
        {
            var count= await _orderService.GetOrdersCountAsync(ct);
            return Ok(count);
        }
        [HttpGet("{id}/bybuyer",Name ="Orderbybuyer")]
        
        [Authorize]
        [ProducesResponseType(typeof(IEnumerable<OrderResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> OrderByBuyerId(string id,CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(id))
                return BadRequest("id is not correct");
            var orders =await _orderService.GetOrdersByBuyerIdAsync(id, ct);
            return Ok(orders);
        }
        [HttpGet("{status}/byStatus",Name ="OrderByStatus")]
        
        [Authorize]
        [ProducesResponseType(typeof(IEnumerable<OrderResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> OrderByStatus(OrderStatus status)
        {
            var orders=await _orderService.GetOrdersByStatusAsync(status); 
            return Ok(orders);
        }
        [HttpDelete("{id}", Name = "DeleteOrder")]
       
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            if (id <= 0)
                return BadRequest("Invalid order ID.");

            await _orderService.DeleteOrderAsync(id, ct);

            return NoContent();
        }
        [HttpPut("{id}", Name = "UpdateOrder")]
        
        [Authorize(Roles = "Client")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(
         int id,
        UpdateOrderRequest request,
         CancellationToken ct)
        {
            if (id <= 0)
                return BadRequest("Invalid order ID.");

            if (request is null)
                return BadRequest("Request cannot be null.");

            await _orderService.UpdateOrderAsync(id, request, ct);

            return NoContent();
        }
      
    }
}

