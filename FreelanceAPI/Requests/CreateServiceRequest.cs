using System.ComponentModel.DataAnnotations;

namespace FreelanceAPI.Requests
{
    public class CreateServiceRequest
    {
        [Required]
        public string UserId { get; set; }
        [Required (ErrorMessage = "Title is required.")]
        [StringLength(50,MinimumLength = 3, ErrorMessage = "Title must be between 3 and 50 characters.")]
        public string Title { get; set; } = string.Empty;

        [Required (ErrorMessage = "Description is required.")]
        [StringLength(1000,MinimumLength = 10, ErrorMessage = "Description must be between 10 and 1000 characters.")]
        public string Description { get; set; } = string.Empty;

        [Range(1, 100000, ErrorMessage = "Price must be between 1 and 100000.")]
        public decimal Price { get; set; }

        [Range(1, 10, ErrorMessage = "Delivery time must be between 1 and 10 days.")]
        public int DeliveryTimeInDays { get; set; }
    }
}
