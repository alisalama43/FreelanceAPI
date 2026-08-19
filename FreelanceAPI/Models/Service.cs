using System.ComponentModel.DataAnnotations;

namespace FreelanceAPI.Models
{
    public class Service
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Title is required.")]
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int DeliveryTimeInDays { get; set; }
        public bool IsActive { get; set; }
        public User User { get; set; } = null!;
        public string UserId { get; set; }


    }
}
