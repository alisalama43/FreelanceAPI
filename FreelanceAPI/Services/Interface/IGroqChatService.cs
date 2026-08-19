using FreelanceAPI.Requests;
using FreelanceAPI.Responses;

namespace FreelanceAPI.Services.Interface
{
    public interface IGroqChatService
    {
        Task<ChatResponse> GetChatCompletionAsync(string prompt, CancellationToken cancellationToken);
    }
}
