using FreelanceAPI.Requests;
using FreelanceAPI.Responses;
using FreelanceAPI.Services.Interface;
using Google.GenAI;
using Google.GenAI.Types;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Retry;
using System.Text.RegularExpressions;

namespace FreelanceAPI.Services.implementation
{


    public sealed class GroqChatService : IGroqChatService
    {
        private readonly HttpClient _httpClient;
        private readonly GroqOptions _options;
        private readonly ILogger<GroqChatService> _logger;

        public GroqChatService(HttpClient httpClient, IOptions<GroqOptions> options, ILogger<GroqChatService> logger)
        {
            _httpClient = httpClient;
            _options = options.Value;
            _logger = logger;
        }

        public async Task<ChatResponse> GetChatCompletionAsync(string prompt, CancellationToken cancellationToken)
        {
            var request = new GroqChatRequest(
                Model: _options.Model,
                Messages: [new GroqMessage("user", prompt)]);

            _logger.LogInformation("Sending chat completion request to Groq using model {Model}", _options.Model);

            // BaseAddress and Authorization header are pre-configured on this HttpClient in Program.cs
            using var response = await _httpClient.PostAsJsonAsync("chat/completions", request, cancellationToken);

            // Throws HttpRequestException on non-2xx — Polly retries transient ones before we get here
            response.EnsureSuccessStatusCode();

            var completion = await response.Content.ReadFromJsonAsync<GroqChatCompletion>(cancellationToken)
                ?? throw new InvalidOperationException("Groq API returned an empty response body.");

            var content = completion.Choices.FirstOrDefault()?.Message.Content
                ?? throw new InvalidOperationException("Groq API response contained no completion choices.");

            _logger.LogInformation("Received chat completion from Groq successfully");

            return new ChatResponse(content);
        }
    }
}
