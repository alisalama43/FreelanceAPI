using FreelanceAPI.Models.Enum;
using System.ComponentModel.DataAnnotations;

namespace M03.RepositoryPattern.Requests;

public class UpdateOrderRequest
{
  public OrderStatus Status { get; set; }
}
