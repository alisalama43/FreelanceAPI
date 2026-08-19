using Microsoft.AspNetCore.Identity;

namespace FreelanceAPI.Models
{
    public class ApplicationRole:IdentityRole
    {
        public string? Description { get; set; }
    }
}
