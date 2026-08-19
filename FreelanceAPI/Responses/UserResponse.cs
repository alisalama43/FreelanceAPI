using FreelanceAPI.Models;
using FreelanceMarketplace.API.Enums;

namespace FreelanceAPI.Responses
{
    public class UserResponse
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public UserRole Role { get; set; } 
        public List<ServiceResponse> Services { get; set; } = new List<ServiceResponse>();
    }
}
