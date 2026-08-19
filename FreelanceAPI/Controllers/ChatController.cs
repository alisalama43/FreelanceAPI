using FreelanceAPI.Requests;
using FreelanceAPI.Responses;
using FreelanceAPI.Services.implementation;
using FreelanceAPI.Services.Interface;
using Google.GenAI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FreelanceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
  
    public sealed class ChatController : ControllerBase
    {
        private readonly IGroqChatService _chatService;
        private readonly ILogger<ChatController> _logger;

        public ChatController(IGroqChatService chatService, ILogger<ChatController> logger)
        {
            _chatService = chatService;
            _logger = logger;
        }

        [HttpPost]
        
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ChatResponse>> PostAsync(
            [FromBody] ChatRequest request,
            CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.Prompt))
            {
                return BadRequest("Prompt must not be empty.");
            }

            try
            {
                var result = await _chatService.GetChatCompletionAsync(request.Prompt, cancellationToken);
                return Ok(result);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Groq API call failed after retries");
                return StatusCode(StatusCodes.Status502BadGateway, "Upstream chat provider is unavailable.");
            }
        }
    }
}