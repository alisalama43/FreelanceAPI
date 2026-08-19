using FreelanceAPI.Models.Enum;

namespace FreelanceAPI.Models
{
    public class Order
    {

      public int Id { get; set; }
      public int ServiceId { get; set; }
      public string? BuyerId { get; set; }
      public DateTime OrderDate { get; set; }
      public OrderStatus Status { get; set; }
      public ICollection<Review> Reviews { get; set; }= new List<Review>();
      public Service Service { get; set; } = null!;
      public User Buyer { get; set; } = null!;

    }

   
}
