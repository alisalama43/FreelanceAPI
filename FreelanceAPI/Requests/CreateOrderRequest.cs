using FreelanceAPI.Models.Enum;

namespace M03.RepositoryPattern.Requests;

public class CreateOrderRequest
{

    public int Id { get; set; }

    public int ServiceId { get; set; }
    public string ServiceTitle { get; set; } = string.Empty;

    public string? BuyerId { get; set; }

    public DateTime OrderDate { get; set; }

    public OrderStatus Status { get; set; }
}
