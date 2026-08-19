using System.Text.Json.Serialization;

namespace FreelanceAPI.Requests
{
    public class GroqOptions
    {
        public const string SectionName = "Groq";

        public string BaseUrl { get; init; } = string.Empty;
        public string ApiKey { get; init; } = string.Empty;
        public string Model { get; init; } = string.Empty;
        public int TimeoutSeconds { get; init; } = 30;
    }
    // Groq's OpenAI-compatible request shape
    internal sealed record GroqChatRequest(
        [property: JsonPropertyName("model")] string Model,
        [property: JsonPropertyName("messages")] GroqMessage[] Messages);

    internal sealed record GroqMessage(
        [property: JsonPropertyName("role")] string Role,
        [property: JsonPropertyName("content")] string Content);

    // Groq's response shape (only fields we need)
    internal sealed record GroqChatCompletion(
        [property: JsonPropertyName("choices")] GroqChoice[] Choices);

    internal sealed record GroqChoice(
        [property: JsonPropertyName("message")] GroqMessage Message);
}
