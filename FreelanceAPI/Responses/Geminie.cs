using System.Text.Json.Serialization;

namespace FreelanceAPI.Responses
{
    public class GeminiApiRequest
    {
        [JsonPropertyName("contents")]
        public List<Content> Contents { get; set; } = new();
    }

    public class Content
    {
        [JsonPropertyName("parts")]
        public List<Part> Parts { get; set; } = new();
    }

    public class Part
    {
        [JsonPropertyName("text")]
        public string Text { get; set; } = string.Empty;
    }

    // هيكل الـ Response القادم من Gemini API
    public class GeminiApiResponse
    {
        [JsonPropertyName("candidates")]
        public List<Candidate>? Candidates { get; set; }
    }

    public class Candidate
    {
        [JsonPropertyName("content")]
        public Content? Content { get; set; }
    }
}
