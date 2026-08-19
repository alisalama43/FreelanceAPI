using FreelanceAPI.Models;
using FreelanceAPI.Models.Enum;


namespace M03.RepositoryPattern.Responses;

public class OrderResponse
{
    public int Id { get; set; }

    public int ServiceId { get; set; }
    public string ServiceTitle { get; set; } = string.Empty;

    public string? BuyerId { get; set; }

    public DateTime OrderDate { get; set; }

    public OrderStatus Status { get; set; }
    public List<ProductReviewResponse>? Reviews { get; set; } = default;
   



    public static OrderResponse FromModel(Order product, IEnumerable<Review>? reviews = null)
    {
        if (product == null)
            throw new ArgumentNullException(nameof(product), "Cannot create a response from a null product");

        var response = new OrderResponse
        {
            Id = product.Id,
            

        };

        if (reviews != null)
            response.Reviews = ProductReviewResponse.FromModels(reviews).ToList();


        return response;
    }

    public static IEnumerable<OrderResponse> FromModels(IEnumerable<Order> products)
    {
        if (products == null)
            throw new ArgumentNullException(nameof(products), "Cannot create responses from a null collection");

        return products.Select(p => FromModel(p));
    }
}