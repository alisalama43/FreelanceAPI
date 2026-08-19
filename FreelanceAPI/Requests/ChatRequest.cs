using System.ComponentModel.DataAnnotations;

namespace FreelanceAPI.Requests
{
    public sealed record ChatRequest(string Prompt);
}