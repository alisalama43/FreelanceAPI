using FreelanceAPI.Models.Enum;
using FreelanceAPI.Validators;
using FreelanceMarketplace.API.Enums;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace FreelanceAPI.Models
{
    public class User: IdentityUser
    {
        [Required(ErrorMessage = "Name is required.")]
        [StringLength(150,MinimumLength = 3,ErrorMessage = "Name must be between 3 and 150 characters.")]
        public string Name { get; set; }
     

        public ICollection<Service> Services { get; set; } = new List<Service>();
        public ICollection<Order> Orders { get; set; } = new List<Order>();
        [Required(ErrorMessage = "CreatedAt is required.")]
        [CustomValidation(typeof(Datevalidator), nameof(Datevalidator.IsvalidDate))]
        public DateTime CreatedAt { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }
        public bool IsActive { get; set; } = true;

    }
}
